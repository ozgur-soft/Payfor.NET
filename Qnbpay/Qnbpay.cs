using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Qnbpay {
    public interface IQnbpay {
        void SetMerchantId(string merchantid);
        void SetMerchantPass(string merchantpass);
        void SetUserCode(string usercode);
        void SetUserPass(string userpass);
        Qnbpay.PayforResponse Pay(string cardnumber, string cardmonth, string cardyear, string cardcode, string cardholder, string price, string currency, string language, string moto);
    }
    public class Qnbpay : IQnbpay {
        private string Endpoint { get; set; }
        private string MbrId { get; set; }
        private string MerchantId { get; set; }
        private string MerchantPass { get; set; }
        private string UserCode { get; set; }
        private string UserPass { get; set; }
        public Qnbpay() {
            Endpoint = "https://vpos.qnbfinansbank.com/Gateway/XmlGate.aspx";
        }
        [Serializable, XmlRoot("PayforRequest")]
        public class PayforRequest {
            [XmlElement("MbrId", IsNullable = false)]
            public string MbrId { init; get; }
            [XmlElement("MerchantId", IsNullable = false)]
            public string MerchantId { init; get; }
            [XmlElement("UserCode", IsNullable = false)]
            public string UserCode { init; get; }
            [XmlElement("UserPass", IsNullable = false)]
            public string UserPass { init; get; }
            [XmlElement("SecureType", IsNullable = false)]
            public string SecureType { init; get; }
            [XmlElement("TxnType", IsNullable = false)]
            public string TxnType { init; get; }
            [XmlElement("PurchAmount", IsNullable = false)]
            public string Amount { init; get; }
            [XmlElement("Currency", IsNullable = false)]
            public string Currency { init; get; }
            [XmlElement("InstallmentCount", IsNullable = false)]
            public string Installment { init; get; }
            [XmlElement("CardHolderName", IsNullable = false)]
            public string CardHolder { init; get; }
            [XmlElement("Pan", IsNullable = false)]
            public string CardNumber { init; get; }
            [XmlElement("Expiry", IsNullable = false)]
            public string CardExpiry { init; get; }
            [XmlElement("Cvv2", IsNullable = false)]
            public string CardCode { init; get; }
            [XmlElement("OrderId", IsNullable = false)]
            public string OrderId { init; get; }
            [XmlElement("OrgOrderId", IsNullable = false)]
            public string OrgOrderId { init; get; }
            [XmlElement("OkUrl", IsNullable = false)]
            public string OkUrl { init; get; }
            [XmlElement("FailUrl", IsNullable = false)]
            public string FailUrl { init; get; }
            [XmlElement("Rnd", IsNullable = false)]
            public string Rnd { init; get; }
            [XmlElement("Hash", IsNullable = false)]
            public string Hash { init; get; }
            [XmlElement("MOTO", IsNullable = false)]
            public string MOTO { init; get; }
            [XmlElement("Lang", IsNullable = false)]
            public string Lang { init; get; }
        }

        [Serializable, XmlRoot("PayforResponse")]
        public class PayforResponse {
            [XmlElement("OrderId")]
            public string OrderId { init; get; }
            [XmlElement("TransId")]
            public string TransId { init; get; }
            [XmlElement("AuthCode")]
            public string AuthCode { init; get; }
            [XmlElement("HostRefNum")]
            public string HostRefNum { init; get; }
            [XmlElement("ProcReturnCode")]
            public string ProcReturnCode { init; get; }
            [XmlElement("3DStatus")]
            public string Status3D { init; get; }
            [XmlElement("ResponseRnd")]
            public string ResponseRnd { init; get; }
            [XmlElement("ResponseHash")]
            public string ResponseHash { init; get; }
            [XmlElement("TxnResult")]
            public string TxnResult { init; get; }
            [XmlElement("ErrMsg")]
            public string ErrMsg { init; get; }
        }
        public class Writer : StringWriter {
            public override Encoding Encoding => Encoding.UTF8;
        }
        public void SetMbrId(string mbrid) {
            MbrId = mbrid;
        }
        public void SetMerchantId(string merchantid) {
            MerchantId = merchantid;
        }
        public void SetMerchantPass(string merchantpass) {
            MerchantPass = merchantpass;
        }
        public void SetUserCode(string usercode) {
            UserCode = usercode;
        }
        public void SetUserPass(string userpass) {
            UserPass = userpass;
        }
        public static string Hash(string data) {
            var hash = Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(data)));
            return hash;
        }
        public string Create3DHash(PayforRequest data) {
            var str = data.MbrId + data.OrderId + data.Amount + data.OkUrl + data.FailUrl + data.TxnType + data.Installment + data.Rnd + MerchantPass;
            var hash = Hash(str);
            return hash;
        }
        public bool Check3DHash(PayforResponse data) {
            var str = MerchantId + MerchantPass + data.OrderId + data.AuthCode + data.ProcReturnCode + data.Status3D + data.ResponseRnd + UserCode;
            var hash = Hash(str);
            return hash == data.ResponseHash;
        }
        public PayforResponse Pay(string cardnumber, string cardmonth, string cardyear, string cardcode, string cardholder, string price, string currency, string language, string moto = "0") {
            var data = new PayforRequest {
                MbrId = MbrId,
                MerchantId = MerchantId,
                UserCode = UserCode,
                UserPass = UserPass,
                TxnType = "Auth",
                SecureType = "NonSecure",
                CardHolder = cardholder,
                CardNumber = cardnumber,
                CardExpiry = cardmonth + cardyear,
                CardCode = cardcode,
                Amount = price,
                Currency = currency,
                MOTO = moto,
                Lang = language
            };
            var payforrequest = new XmlSerializer(typeof(PayforRequest));
            var payforresponse = new XmlSerializer(typeof(PayforResponse));
            var writer = new Writer();
            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            payforrequest.Serialize(writer, data, ns);
            try {
                var http = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
                    Content = new StringContent(writer.ToString(), Encoding.UTF8, "text/xml")
                };
                var response = http.Send(request);
                var result = (PayforResponse)payforresponse.Deserialize(response.Content.ReadAsStream());
                return result;
            } catch (Exception err) {
                if (err.InnerException != null) {
                    Console.WriteLine(err.InnerException.Message);
                } else {
                    Console.WriteLine(err.Message);
                }
            }
            return null;
        }
    }
}