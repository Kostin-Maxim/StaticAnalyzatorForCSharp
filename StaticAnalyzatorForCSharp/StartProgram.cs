using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StaticAnalyzatorForCSharp
{
    internal class StartProgram
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.Run(new InitializationForm());
        }
    }
}
