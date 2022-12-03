[![license](https://img.shields.io/:license-mit-blue.svg)](https://github.com/ozgur-soft/Payfor.NET/blob/main/LICENSE.md)

# Payfor.NET
Payfor (Finansbank) Virtual POS API with .NET

# Installation
```bash
dotnet add package Payfor --version 1.1.1
```

# Sanalpos satış işlemi
```c#
namespace Payfor {
    internal class Program {
        static void Main(string[] args) {
            var qnbpay = new Payfor("PROD"); // PROD || TEST
            qnbpay.SetMbrId(""); // Mbr Id
            qnbpay.SetMerchantId(""); // Merchant id
            qnbpay.SetMerchantPass(""); // Merchant pass (storekey)
            qnbpay.SetUserCode(""); // Usercode
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
                Console.WriteLine(Payfor.JsonString<Payfor.PayforResponse>(response));
            }
        }
    }
}
```

# Sanalpos iade işlemi
```c#
namespace Payfor {
    internal class Program {
        static void Main(string[] args) {
            var qnbpay = new Payfor("PROD"); // PROD || TEST
            qnbpay.SetMbrId(""); // Mbr Id
            qnbpay.SetMerchantId(""); // Merchant id
            qnbpay.SetMerchantPass(""); // Merchant pass (storekey)
            qnbpay.SetUserCode(""); // Usercode
            qnbpay.SetUserPass(""); // Userpass
            qnbpay.SetAmount("1.00", "TRY"); // İade tutarı ve para birimi
            qnbpay.SetOrgOrderId("SYS_"); // Sipariş numarası
            qnbpay.SetLanguage("TR"); // TR || EN
            var response = qnbpay.Refund();
            if (response != null) {
                Console.WriteLine(Payfor.JsonString<Payfor.PayforResponse>(response));
            }
        }
    }
}
```

# Sanalpos iptal işlemi
```c#
namespace Payfor {
    internal class Program {
        static void Main(string[] args) {
            var qnbpay = new Payfor("PROD"); // PROD || TEST
            qnbpay.SetMbrId(""); // Mbr Id
            qnbpay.SetMerchantId(""); // Merchant id
            qnbpay.SetMerchantPass(""); // Merchant pass (storekey)
            qnbpay.SetUserCode(""); // Usercode
            qnbpay.SetUserPass(""); // Userpass
            qnbpay.SetAmount("1.00", "TRY"); // İptal tutarı ve para birimi
            qnbpay.SetOrgOrderId("SYS_"); // Sipariş numarası
            qnbpay.SetLanguage("TR"); // TR || EN
            var response = qnbpay.Cancel();
            if (response != null) {
                Console.WriteLine(Payfor.JsonString<Payfor.PayforResponse>(response));
            }
        }
    }
}
```