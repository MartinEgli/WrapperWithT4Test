using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace TestLibrary
{
    public interface Interface1
    {
        void Method1();
        void Method2(string str);
        void Method3([NotNull][Attribute] string str);
        void Method4([NotNull][Attribute] ref string str);
        void Method5([NotNull][Attribute] out string str);
        void Method6([NotNull][Attribute] string str, bool isBool);
        void Method7([NotNull][Attribute] string str, bool isBool = true);

        string this[int i]
        {
            get;
            set;
        }

        string this[int i, int j]
        {
            get;
            set;
        }

        string ReadWrite { get; set; }
        string Read { get; }
    }

    public class AttributeAttribute : Attribute{}
}
