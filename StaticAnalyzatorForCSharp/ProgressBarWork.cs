using System;
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
