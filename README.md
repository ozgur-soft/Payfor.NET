[![license](https://img.shields.io/:license-mit-blue.svg)](https://github.com/ozgur-soft/Qnbpay.NET/blob/main/LICENSE.md)

# Qnbpay.NET
Qnbpay (Finansbank) Virtual POS API with .NET

# Installation
```bash
dotnet add package Qnbpay --version 1.0.0
```

# Usage
```c#
using Qnbpay;

var qnbpay = new Qnbpay();
qnbpay.SetMbrId("Mbr Id");
qnbpay.SetMerchantId("Merchant id");
qnbpay.SetMerchantPass("Merchant pass (storekey)");
qnbpay.SetUserCode("API usercode");
qnbpay.SetUserPass("API userpass");
qnbpay.Pay(
    "Credit card number (Eg: 4321432143214321)",
    "Card month (Eg: 02)",
    "Card year (Eg: 22)",
    "Card security code: (Eg: 123)",
    "Card holdername",
    "Amount (Eg: 1.00)",
    "Currency code ( $: 840 || €: 978 || ₺: 949 )",
    "Language (TR || EN)"
);
```
