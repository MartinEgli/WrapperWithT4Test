
using System;
using System.Reflection;
using TestLibrary;

namespace ConsoleApp
{

    class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = typeof(Class).Assembly;

            foreach (var type in assembly.GetTypes())
            {
                Console.WriteLine("Type: " + type);
                foreach (var method in type.GetMethods())
                {
                    Console.WriteLine("Method: " + method);
                    foreach (var parameter in method.GetParameters())
                    {
                        Console.WriteLine("Parameter: " + parameter);
                        foreach (var attribute in parameter.GetCustomAttributes(true))
                        {
                            Console.WriteLine("Attribute: " + attribute);
                          
                        }
                    }
                }
            }
        }
    }
}
