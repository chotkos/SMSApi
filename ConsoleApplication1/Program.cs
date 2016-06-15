using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMSApi;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            var provider = new SMSApiProvider("chotkos@gmail.com", "7194fe56e7f53fd06a23ebf0f85078ac");
            provider.sendSMS("514899374", "Test", "Info");
            Console.ReadLine();
        }
    }
}
