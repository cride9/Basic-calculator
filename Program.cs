#pragma warning disable CS8604
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
namespace Calculator {
    internal class Program {
        static void Main(string[] args) {

            /*
                Thanks for visiting my repository and checking out this program 
                The calculator itself is located in the Calculator.cs file
                
                Usage:
                    new Calculator("2+2").GetResult();
            */
            Console.WriteLine("Usage example: 1.5+2*4\nCurrent operators: (+, -, *, /, ^) EVERY OPERATOR COULD BE USED ONCE");
            Console.WriteLine($"\nResult: {new Calculator(Console.ReadLine()).GetResult()}");
        }
    }
}
#pragma warning restore CS8604