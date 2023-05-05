# Payfor.NET
Payfor (Finansbank) Virtual POS API with .NET

# Installation
```bash
dotnet add package Payfor --version 1.2.0
```

# Satış
```c#
namespace Payfor {
    internal class Program {
        static void Main(string[] args) {
            var payfor = new Payfor(MODE.TEST); // PROD || TEST
            payfor.SetMbrId("5"); // Mbr Id
            payfor.SetMerchantId("085300000009704"); // Merchant id
            payfor.SetMerchantPass("12345678"); // Merchant pass (storekey)
            payfor.SetUserCode("QNB_API_KULLANICI_3DPAY"); // Usercode
            payfor.SetUserPass("UcBN0"); // Userpass
            var request = new Payfor.PayforRequest();
            request.SetCardHolder(""); // Kart sahibi
            request.SetCardNumber("4155650100416111"); // Kart numarası
            request.SetCardExpiry("01", "25"); // Son kullanma tarihi (Ay ve Yılın son 2 hanesi)
            request.SetCardCode("123"); // Cvv2 Kodu (kartın arka yüzündeki 3 haneli numara)
            request.SetAmount("1.00", "TRY"); // Satış tutarı ve para birimi
            request.SetInstallment(""); // Taksit sayısı (varsa)
            request.SetLanguage("TR"); // TR || EN
            var response = payfor.Auth(request);
            if (response != null) {
                Console.WriteLine(Payfor.JsonString<Payfor.PayforResponse>(response));
            }
        }
    }
}
```

# İade
```c#
namespace Payfor {
    internal class Program {
        static void Main(string[] args) {
            var payfor = new Payfor(MODE.TEST); // PROD || TEST
            payfor.SetMbrId("5"); // Mbr Id
            payfor.SetMerchantId("085300000009704"); // Merchant id
            payfor.SetMerchantPass("12345678"); // Merchant pass (storekey)
            payfor.SetUserCode("QNB_API_KULLANICI_3DPAY"); // Usercode
            payfor.SetUserPass("UcBN0"); // Userpass
            var request = new Payfor.PayforRequest();
            request.SetAmount("1.00", "TRY"); // Satış tutarı ve para birimi
            request.SetOrgOrderId("SYS_"); // Sipariş numarası
            request.SetLanguage("TR"); // TR || EN
            var response = payfor.Refund(request);
            if (response != null) {
                Console.WriteLine(Payfor.JsonString<Payfor.PayforResponse>(response));
            }
        }
    }
}
```

# İptal
```c#
namespace Payfor {
    internal class Program {
        static void Main(string[] args) {
            var payfor = new Payfor(MODE.TEST); // PROD || TEST
            payfor.SetMbrId("5"); // Mbr Id
            payfor.SetMerchantId("085300000009704"); // Merchant id
            payfor.SetMerchantPass("12345678"); // Merchant pass (storekey)
            payfor.SetUserCode("QNB_API_KULLANICI_3DPAY"); // Usercode
            payfor.SetUserPass("UcBN0"); // Userpass
            var request = new Payfor.PayforRequest();
            request.SetCurrency("TRY"); // Satış tutarı ve para birimi
            request.SetOrgOrderId("SYS_"); // Sipariş numarası
            request.SetLanguage("TR"); // TR || EN
            var response = payfor.Cancel(request);
            if (response != null) {
                Console.WriteLine(Payfor.JsonString<Payfor.PayforResponse>(response));
            }
        }
    }
}
```