csharp-client
===========

Klient SMSAPI napisany w języku C#, pozwalający na wysyłanie wiadomości SMS, MMS, VMS oraz zarządzanie kontem w serwisie SMSAPI.pl

dodana klasa SMSApiProvidera do krótszego wysyłania smsów

Przykład wysyłki:
```c#
var provider = new SMSApiProvider("chotkos@gmail.com", "7194fe56e7f53fd06a23ebf0f85078ac");
            provider.sendSMS("514899374", "Test", "Info");
            Console.ReadLine();
```

## Wymagania

* C# >= 3.5 + System.Runtime.Serialization, System.ServiceModel.Web
* C# >= 4.0

## LICENSE
[Apache 2.0 License](https://github.com/smsapi/smsapi-php-client/blob/master/LICENSE)
"# SMSApi" 
