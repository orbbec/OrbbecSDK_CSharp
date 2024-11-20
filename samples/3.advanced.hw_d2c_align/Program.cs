using System;

namespace Orbbec
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var w = new HWD2CAlignWindow();
            w.ShowDialog();
        }
    }
}