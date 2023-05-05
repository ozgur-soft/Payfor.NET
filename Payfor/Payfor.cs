using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Payfor {
    public class Payfor {
        public string Endpoint { get; set; }
        public string MbrId { set; get; }
        public string MerchantId { get; set; }
        public string MerchantPass { get; set; }
        public string UserCode { set; get; }
        public string UserPass { set; get; }
        internal void SetMbrId(string mbrid) {
            MbrId = mbrid;
        }
        internal void SetMerchantId(string merchantid) {
            MerchantId = merchantid;
        }
        internal void SetMerchantPass(string merchantpass) {
            MerchantPass = merchantpass;
        }
        internal void SetUserCode(string usercode) {
            UserCode = usercode;
        }
        internal void SetUserPass(string userpass) {
            UserPass = userpass;
        }
        public Payfor(string mode) {
            Endpoint = mode switch {
                "PROD" => "https://vpos.qnbfinansbank.com/Gateway/XmlGate.aspx",
                "TEST" => "https://vpostest.qnbfinansbank.com/Gateway/XmlGate.aspx",
                _ => null
            };
        }
        [Serializable, XmlRoot("PayforRequest")]
        public class PayforRequest {
            [XmlElement("MbrId", IsNullable = false)]
            public string MbrId { set; get; }
            [XmlElement("MerchantId", IsNullable = false)]
            public string MerchantId { set; get; }
            [XmlElement("UserCode", IsNullable = false)]
            public string UserCode { set; get; }
            [XmlElement("UserPass", IsNullable = false)]
            public string UserPass { set; get; }
            [XmlElement("SecureType", IsNullable = false)]
            public string SecureType { set; get; }
            [XmlElement("TxnType", IsNullable = false)]
            public string TxnType { set; get; }
            [XmlElement("PurchAmount", IsNullable = false)]
            public string Amount { set; get; }
            [XmlElement("Currency", IsNullable = false)]
            public string Currency { set; get; }
            [XmlElement("InstallmentCount", IsNullable = false)]
            public string Installment { set; get; }
            [XmlElement("CardHolderName", IsNullable = false)]
            public string CardHolder { set; get; }
            [XmlElement("Pan", IsNullable = false)]
            public string CardNumber { set; get; }
            [XmlElement("Expiry", IsNullable = false)]
            public string CardExpiry { set; get; }
            [XmlElement("Cvv2", IsNullable = false)]
            public string CardCode { set; get; }
            [XmlElement("OrderId", IsNullable = false)]
            public string OrderId { set; get; }
            [XmlElement("OrgOrderId", IsNullable = false)]
            public string OrgOrderId { set; get; }
            [XmlElement("OkUrl", IsNullable = false)]
            public string OkUrl { set; get; }
            [XmlElement("FailUrl", IsNullable = false)]
            public string FailUrl { set; get; }
            [XmlElement("Rnd", IsNullable = false)]
            public string Rnd { set; get; }
            [XmlElement("Hash", IsNullable = false)]
            public string Hash { set; get; }
            [XmlElement("MOTO", IsNullable = false)]
            public string MOTO { set; get; }
            [XmlElement("Lang", IsNullable = false)]
            public string Language { set; get; }
            internal void SetOrgOrderId(string orderid) {
                OrgOrderId = orderid;
            }
            internal void SetAmount(string amount, string currency) {
                Amount = amount;
                Currency = currency switch {
                    "TRY" => "949",
                    "YTL" => "949",
                    "TRL" => "949",
                    "TL" => "949",
                    "USD" => "840",
                    "EUR" => "978",
                    "GBP" => "826",
                    "JPY" => "392",
                    _ => currency
                };
            }
            internal void SetInstallment(string installment) {
                Installment = installment;
            }
            internal void SetCardHolder(string cardholder) {
                CardHolder = cardholder;
            }
            internal void SetCardNumber(string cardnumber) {
                CardNumber = cardnumber;
            }
            internal void SetCardExpiry(string cardmonth, string cardyear) {
                CardExpiry = cardmonth + cardyear;
            }
            internal void SetCardCode(string cardcode) {
                CardCode = cardcode;
            }
            internal void SetLanguage(string language) {
                Language = language;
            }
            internal void SetMOTO(string moto) {
                MOTO = moto;
            }
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
        public PayforResponse Auth(PayforRequest data) {
            var payforrequest = new XmlSerializer(typeof(PayforRequest));
            var payforresponse = new XmlSerializer(typeof(PayforResponse));
            using var writer = new Writer();
            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            payforrequest.Serialize(writer, data, ns);
            try {
                using var http = new HttpClient();
                using var request = new HttpRequestMessage(HttpMethod.Post, Endpoint) { Content = new StringContent(writer.ToString(), Encoding.UTF8, "text/xml") };
                using var response = http.Send(request);
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
        public PayforResponse Refund(PayforRequest data) {
            var payforrequest = new XmlSerializer(typeof(PayforRequest));
            var payforresponse = new XmlSerializer(typeof(PayforResponse));
            using var writer = new Writer();
            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            payforrequest.Serialize(writer, data, ns);
            try {
                using var http = new HttpClient();
                using var request = new HttpRequestMessage(HttpMethod.Post, Endpoint) { Content = new StringContent(writer.ToString(), Encoding.UTF8, "text/xml") };
                using var response = http.Send(request);
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
        public PayforResponse Cancel(PayforRequest data) {
            var payforrequest = new XmlSerializer(typeof(PayforRequest));
            var payforresponse = new XmlSerializer(typeof(PayforResponse));
            using var writer = new Writer();
            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            payforrequest.Serialize(writer, data, ns);
            try {
                using var http = new HttpClient();
                using var request = new HttpRequestMessage(HttpMethod.Post, Endpoint) { Content = new StringContent(writer.ToString(), Encoding.UTF8, "text/xml") };
                using var response = http.Send(request);
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
        public static string JsonString<T>(T data) where T : class {
            return JsonSerializer.Serialize(data, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true });
        }
    }
}