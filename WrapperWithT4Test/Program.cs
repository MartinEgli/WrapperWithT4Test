using System;

namespace WrapperWithT4Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var x = new Siemens.Sinumerik.Operate.Services.AlarmSvc("");
        }
    }
}