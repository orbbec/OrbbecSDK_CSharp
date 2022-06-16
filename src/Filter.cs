using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Orbbec
{
    internal delegate void FilterCallbackInternal(IntPtr framePtr, IntPtr userDataPtr);
    public delegate void FilterCallback(Frame frame);

    public class Filter : IDisposable
    {
        public void Dispose()
        {
        }
    }
}