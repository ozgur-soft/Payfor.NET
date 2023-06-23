using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Payfor {
    public enum MODE {
        Test,
        Prod
    }
    public class Payfor {
        public string Endpoint { get; set; }
        private string MbrId { get; set; }
        private string MerchantId { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private string StoreKey { get; set; }
        public void SetMbrId(string mbrid) {
            MbrId = mbrid;
        }
        public void SetMerchantId(string merchantid) {
            MerchantId = merchantid;
        }
        public void SetUsername(string username) {
            Username = username;
        }
        public void SetPassword(string password) {
            Password = password;
        }
        public void SetStoreKey(string storekey) {
            StoreKey = storekey;
        }
        public Payfor(MODE mode) {
            Endpoint = mode switch {
                MODE.Test => "https://vpostest.qnbfinansbank.com/Gateway",
                MODE.Prod => "https://vpos.qnbfinansbank.com/Gateway",
                _ => null
            };
        }
        [XmlRoot("PayforRequest")]
        public class PayforRequest {
            [FormElement("MbrId")]
            [XmlElement("MbrId", IsNullable = false)]
            public string MbrId { get; set; }
            [FormElement("MerchantId")]
            [XmlElement("MerchantId", IsNullable = false)]
            public string MerchantId { get; set; }
            [FormElement("UserCode")]
            [XmlElement("UserCode", IsNullable = false)]
            public string UserCode { get; set; }
            [XmlElement("UserPass", IsNullable = false)]
            public string UserPass { get; set; }
            [FormElement("SecureType")]
            [XmlElement("SecureType", IsNullable = false)]
            public string SecureType { get; set; }
            [FormElement("TxnType")]
            [XmlElement("TxnType", IsNullable = false)]
            public string TransactionType { get; set; }
            [FormElement("PurchAmount")]
            [XmlElement("PurchAmount", IsNullable = false)]
            public string Amount { get; set; }
            [FormElement("Currency")]
            [XmlElement("Currency", IsNullable = false)]
            public string Currency { get; set; }
            [FormElement("InstallmentCount")]
            [XmlElement("InstallmentCount", IsNullable = false)]
            public string Installment { get; set; }
            [FormElement("CardHolderName")]
            [XmlElement("CardHolderName", IsNullable = false)]
            public string CardHolder { get; set; }
            [FormElement("Pan")]
            [XmlElement("Pan", IsNullable = false)]
            public string CardNumber { get; set; }
            [FormElement("Expiry")]
            [XmlElement("Expiry", IsNullable = false)]
            public string CardExpiry { get; set; }
            [FormElement("Cvv2")]
            [XmlElement("Cvv2", IsNullable = false)]
            public string CardCode { get; set; }
            [FormElement("OrderId")]
            [XmlElement("OrderId", IsNullable = false)]
            public string OrderId { get; set; }
            [XmlElement("OrgOrderId", IsNullable = false)]
            public string OrgOrderId { get; set; }
            [FormElement("OkUrl")]
            [XmlElement("OkUrl", IsNullable = false)]
            public string OkUrl { get; set; }
            [FormElement("FailUrl")]
            [XmlElement("FailUrl", IsNullable = false)]
            public string FailUrl { get; set; }
            [FormElement("Rnd")]
            [XmlElement("Rnd", IsNullable = false)]
            public string Random { get; set; }
            [FormElement("Hash")]
            [XmlElement("Hash", IsNullable = false)]
            public string Hash { get; set; }
            [FormElement("MOTO")]
            [XmlElement("MOTO", IsNullable = false)]
            public string MOTO { get; set; }
            [FormElement("Lang")]
            [XmlElement("Lang", IsNullable = false)]
            public string Language { get; set; }
            public void SetOrgOrderId(string orderid) {
                OrgOrderId = orderid;
            }
            public void SetAmount(string amount, string currency) {
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
            public void SetCurrency(string currency) {
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
            public void SetInstallment(string installment) {
                Installment = installment;
            }
            public void SetCardHolder(string cardholder) {
                CardHolder = cardholder;
            }
            public void SetCardNumber(string cardnumber) {
                CardNumber = cardnumber;
            }
            public void SetCardExpiry(string cardmonth, string cardyear) {
                CardExpiry = cardmonth + cardyear;
            }
            public void SetCardCode(string cardcode) {
                CardCode = cardcode;
            }
            public void SetLanguage(string language) {
                Language = language;
            }
            public void SetMOTO(string moto) {
                MOTO = moto;
            }
        }
        [XmlRoot("PayforResponse")]
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
        public class FormElementAttribute : Attribute {
            public string Key { get; }
            public FormElementAttribute(string key) {
                Key = key;
            }
        }
        public class Writer : StringWriter {
            public override Encoding Encoding => Encoding.UTF8;
        }
        public static string Json<T>(T data) where T : class {
            return JsonSerializer.Serialize(data, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true });
        }
        public static byte[] Byte(string data) {
            return Encoding.ASCII.GetBytes(data);
        }
        public static string Hash(string data) {
            return Convert.ToBase64String(SHA1.Create().ComputeHash(Byte(data)));
        }
        public PayforResponse PreAuth(PayforRequest data) {
            data.MbrId = MbrId;
            data.MerchantId = MerchantId;
            data.UserCode = Username;
            data.UserPass = Password;
            data.TransactionType = "PreAuth";
            data.SecureType = "NonSecure";
            data.MOTO ??= "0";
            data.Language ??= "TR";
            return _Transaction(data);
        }
        public PayforResponse PostAuth(PayforRequest data) {
            data.MbrId = MbrId;
            data.MerchantId = MerchantId;
            data.UserCode = Username;
            data.UserPass = Password;
            data.TransactionType = "PostAuth";
            data.SecureType = "NonSecure";
            data.MOTO ??= "0";
            data.Language ??= "TR";
            return _Transaction(data);
        }
        public PayforResponse Auth(PayforRequest data) {
            data.MbrId = MbrId;
            data.MerchantId = MerchantId;
            data.UserCode = Username;
            data.UserPass = Password;
            data.TransactionType = "Auth";
            data.SecureType = "NonSecure";
            data.MOTO ??= "0";
            data.Language ??= "TR";
            return _Transaction(data);
        }
        public PayforResponse Refund(PayforRequest data) {
            data.MbrId = MbrId;
            data.MerchantId = MerchantId;
            data.UserCode = Username;
            data.UserPass = Password;
            data.TransactionType = "Refund";
            data.SecureType = "NonSecure";
            data.MOTO ??= "0";
            data.Language ??= "TR";
            return _Transaction(data);
        }
        public PayforResponse Cancel(PayforRequest data) {
            data.MbrId = MbrId;
            data.MerchantId = MerchantId;
            data.UserCode = Username;
            data.UserPass = Password;
            data.TransactionType = "Void";
            data.SecureType = "NonSecure";
            data.MOTO ??= "0";
            data.Language ??= "TR";
            return _Transaction(data);
        }
        public PayforResponse PreAuth3d(PayforRequest data) {
            data.SecureType = "3DModelPayment";
            data.MOTO ??= "0";
            data.Language ??= "TR";
            return _Transaction(data);
        }
        public PayforResponse Auth3d(PayforRequest data) {
            data.SecureType = "3DModelPayment";
            data.MOTO ??= "0";
            data.Language ??= "TR";
            return _Transaction(data);
        }
        public Dictionary<string, string> PreAuth3dForm(PayforRequest data) {
            data.MbrId = MbrId;
            data.MerchantId = MerchantId;
            data.UserCode = Username;
            data.TransactionType = "PreAuth";
            data.SecureType = "3DModel";
            data.Installment ??= "0";
            data.MOTO ??= "0";
            data.Language ??= "TR";
            data.Random = new Random().Next(100000, 999999).ToString();
            data.Hash = Hash(data.MbrId + data.OrderId + data.Amount + data.OkUrl + data.FailUrl + data.TransactionType + data.Installment + data.Random + StoreKey);
            var form = new Dictionary<string, string>();
            if (data != null) {
                var elements = data.GetType().GetProperties().Where(x => x.GetCustomAttribute<FormElementAttribute>() != null);
                foreach (var element in elements) {
                    var key = element.GetCustomAttribute<FormElementAttribute>().Key;
                    var value = element.GetValue(data)?.ToString();
                    if (!string.IsNullOrEmpty(value)) {
                        form.Add(key, value);
                    }
                }
            }
            return form;
        }
        public Dictionary<string, string> Auth3dForm(PayforRequest data) {
            data.MbrId = MbrId;
            data.MerchantId = MerchantId;
            data.UserCode = Username;
            data.TransactionType = "Auth";
            data.SecureType = "3DModel";
            data.Installment ??= "0";
            data.MOTO ??= "0";
            data.Language ??= "TR";
            data.Random = new Random().Next(100000, 999999).ToString();
            data.Hash = Hash(data.MbrId + data.OrderId + data.Amount + data.OkUrl + data.FailUrl + data.TransactionType + data.Installment + data.Random + StoreKey);
            var form = new Dictionary<string, string>();
            if (data != null) {
                var elements = data.GetType().GetProperties().Where(x => x.GetCustomAttribute<FormElementAttribute>() != null);
                foreach (var element in elements) {
                    var key = element.GetCustomAttribute<FormElementAttribute>().Key;
                    var value = element.GetValue(data)?.ToString();
                    if (!string.IsNullOrEmpty(value)) {
                        form.Add(key, value);
                    }
                }
            }
            return form;
        }
        private PayforResponse _Transaction(PayforRequest data) {
            var payforrequest = new XmlSerializer(typeof(PayforRequest));
            var payforresponse = new XmlSerializer(typeof(PayforResponse));
            using var writer = new Writer();
            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            payforrequest.Serialize(writer, data, ns);
            try {
                using var http = new HttpClient();
                using var request = new HttpRequestMessage(HttpMethod.Post, Endpoint + "/XmlGate.aspx") { Content = new StringContent(writer.ToString(), Encoding.UTF8, "text/xml") };
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
    }
}