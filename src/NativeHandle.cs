using System;

namespace Orbbec
{
    public delegate void ReleaseAction(IntPtr ptr);

    /*
     * \internal
     */
    public sealed class NativeHandle : IDisposable
    {
        private readonly ReleaseAction _action;

        public IntPtr Ptr { get; private set; }
        public bool IsValid { get { return Ptr != IntPtr.Zero; } }

        public NativeHandle(IntPtr ptr, ReleaseAction releaseAction)
        {
            ThrowIfNull(ptr);

            Ptr = ptr;
            _action = releaseAction;
        }

        private void ThrowIfNull(IntPtr ptr)
        {
            if(ptr == IntPtr.Zero)
            {
                throw new NullReferenceException("Handle is null");
            }
        }

        private void ThrowIfInvalid()
        {
            if (IsValid)
                return;

            throw new NullReferenceException("NativeHandle has previously been released");
        }

        public void Release()
        {
            ThrowIfInvalid();
            TryRelease();
        }

        private void TryRelease()
        {
            if (!IsValid)
                return;

            if (_action != null)
            {
                _action(Ptr);
            }

            Ptr = IntPtr.Zero;
        }

        public void Dispose()
        {
            TryRelease();
            GC.SuppressFinalize(this);
        }

        ~NativeHandle()
        {
            TryRelease();
        }
    }
}
