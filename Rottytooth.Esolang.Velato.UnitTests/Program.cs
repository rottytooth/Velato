using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rottytooth.Esolang.Velato;

namespace Rottytooth.Esolang.Velato.UnitTests
{
    class Program
    {
        static void Main(string[] args)
        {
            ConverterTests converterTests = new ConverterTests();
            converterTests.TestIntervals();
            //CompilerTests.BuildHelloWorldChords();
        }
    }
}
