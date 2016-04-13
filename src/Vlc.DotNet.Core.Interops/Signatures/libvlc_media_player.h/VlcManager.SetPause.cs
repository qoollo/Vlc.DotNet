using System;
using Vlc.DotNet.Core.Interops.Signatures;

namespace Vlc.DotNet.Core.Interops
{
    public sealed partial class VlcManager
    {
        public void Resume(VlcMediaPlayerInstance mediaPlayerInstance, bool value)
        {
            SetPause(mediaPlayerInstance, false);
        }

        public void SetPause(VlcMediaPlayerInstance mediaPlayerInstance, bool value)
        {
            if (mediaPlayerInstance == IntPtr.Zero)
                throw new ArgumentException("Media player instance is not initialized.");
            GetInteropDelegate<SetPause>().Invoke(mediaPlayerInstance, value ? 1 : 0);
        }
    }
}
