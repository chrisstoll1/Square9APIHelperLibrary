using Square9APIHelperLibrary;
using Square9APIHelperLibrary.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create a new connection to the application
            Square9API NewConnection = new Square9API("http://10.0.0.198/Square9API", "SSAdministrator", "Square9!");
        }
    }
}
