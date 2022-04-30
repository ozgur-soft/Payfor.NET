[![license](https://img.shields.io/:license-mit-blue.svg)](https://github.com/ozgur-soft/Qnbpay.NET/blob/main/LICENSE.md)

# Qnbpay.NET
Qnbpay (Finansbank) Virtual POS API with .NET

# Installation
```bash
dotnet add package Qnbpay --version 1.0.0
```

# Sanalpos satış işlemi
```c#
namespace Qnbpay {
    internal class Program {
        static void Main(string[] args) {
            var qnbpay = new Qnbpay("PROD"); // PROD || TEST
            qnbpay.SetMbrId(""); // Mbr Id
            qnbpay.SetMerchantId(""); // Merchant id
            qnbpay.SetMerchantPass(""); // Merchant pass (storekey)
            qnbpay.SetUsercode(""); // Usercode
            qnbpay.SetUserPass(""); // Userpass
            qnbpay.SetCardNumber("4242424242424242"); // Kart numarası
            qnbpay.SetCardExpiry("02", "20"); // Son kullanma tarihi (Ay ve Yılın son 2 hanesi)
            qnbpay.SetCardCode("123"); // Cvv2 Kodu (kartın arka yüzündeki 3 haneli numara)
            qnbpay.SetAmount("1.00", "TRY"); // Satış tutarı ve para birimi
            qnbpay.SetInstallment(""); // Taksit sayısı (varsa)
            qnbpay.SetCardHolder(""); // Kart sahibi
            qnbpay.SetLanguage("TR"); // TR || EN
            var response = qnbpay.Pay();
            if (response != null) {
                Console.WriteLine(Qnbpay.JsonString<Qnbpay.CC5Response>(response));
            }
        }
    }
}
```

# Sanalpos iade işlemi
```c#
namespace Qnbpay {
    internal class Program {
        static void Main(string[] args) {
            var qnbpay = new Qnbpay("PROD"); // PROD || TEST
            qnbpay.SetMbrId(""); // Mbr Id
            qnbpay.SetMerchantId(""); // Merchant id
            qnbpay.SetMerchantPass(""); // Merchant pass (storekey)
            qnbpay.SetUsercode(""); // Usercode
            qnbpay.SetUserPass(""); // Userpass
            qnbpay.SetAmount("1.00", "TRY"); // İade tutarı ve para birimi
            qnbpay.SetOrgOrderId("SYS_"); // Sipariş numarası
            qnbpay.SetLanguage("TR"); // TR || EN
            var response = qnbpay.Refund();
            if (response != null) {
                Console.WriteLine(Qnbpay.JsonString<Qnbpay.CC5Response>(response));
            }
        }
    }
}
```

# Sanalpos iptal işlemi
```c#
namespace Qnbpay {
    internal class Program {
        static void Main(string[] args) {
            var qnbpay = new Qnbpay("PROD"); // PROD || TEST
            qnbpay.SetMbrId(""); // Mbr Id
            qnbpay.SetMerchantId(""); // Merchant id
            qnbpay.SetMerchantPass(""); // Merchant pass (storekey)
            qnbpay.SetUsercode(""); // Usercode
            qnbpay.SetUserPass(""); // Userpass
            qnbpay.SetAmount("1.00", "TRY"); // İptal tutarı ve para birimi
            qnbpay.SetOrgOrderId("SYS_"); // Sipariş numarası
            qnbpay.SetLanguage("TR"); // TR || EN
            var response = qnbpay.Cancel();
            if (response != null) {
                Console.WriteLine(Qnbpay.JsonString<Qnbpay.CC5Response>(response));
            }
        }
    }
}
```