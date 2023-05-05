# Payfor.NET
Payfor (Finansbank) POS API with .NET

# Installation
```bash
dotnet add package Payfor --version 1.3.1
```

# Satış
```c#
namespace Payfor {
    internal class Program {
        static void Main(string[] args) {
            var payfor = new Payfor(MODE.Test); // Çalışma ortamı
            payfor.SetMbrId("5"); // Mbr Id
            payfor.SetMerchantId("085300000009704"); // İşyeri numarası
            payfor.SetMerchantPass("12345678"); // İşyeri anahtarı
            payfor.SetUsername("QNB_API_KULLANICI_3DPAY"); // Kullanıcı adı
            payfor.SetPassword("UcBN0"); // Kullanıcı şifresi
            var request = new Payfor.PayforRequest();
            request.SetCardHolder(""); // Kart sahibi
            request.SetCardNumber("4155650100416111"); // Kart numarası
            request.SetCardExpiry("01", "25"); // Son kullanma tarihi - AA,YY
            request.SetCardCode("123"); // Kart arkasındaki 3 haneli numara
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
            var payfor = new Payfor(MODE.Test); // Çalışma ortamı
            payfor.SetMbrId("5"); // Mbr Id
            payfor.SetMerchantId("085300000009704"); // İşyeri numarası
            payfor.SetMerchantPass("12345678"); // İşyeri anahtarı
            payfor.SetUsername("QNB_API_KULLANICI_3DPAY"); // Kullanıcı adı
            payfor.SetPassword("UcBN0"); // Kullanıcı şifresi
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
            var payfor = new Payfor(MODE.Test); // Çalışma ortamı
            payfor.SetMbrId("5"); // Mbr Id
            payfor.SetMerchantId("085300000009704"); // İşyeri numarası
            payfor.SetMerchantPass("12345678"); // İşyeri anahtarı
            payfor.SetUsername("QNB_API_KULLANICI_3DPAY"); // Kullanıcı adı
            payfor.SetPassword("UcBN0"); // Kullanıcı şifresi
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