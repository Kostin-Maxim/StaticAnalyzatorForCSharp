using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StaticAnalyzatorForCSharp
{
    internal class ProgressBarWork
    {
        private static double progress;
        public static double SetProgress
        {
            get => progress;
            set => progress = value;
        }
        public static void Start(ProgressBar progressBar)
        {
            while (true)
            {
                if (progressBar.Value == 100)
                {
                    break;
                }

                progressBar.Invoke(new Action(() => progressBar.Value = (int)progress));
            }
        }
    }
}
