using System;

namespace Orbbec
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var w = new HDRWindow();
            w.ShowDialog();
        }
    }
}