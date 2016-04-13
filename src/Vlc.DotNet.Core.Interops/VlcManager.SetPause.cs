using System;
using Vlc.DotNet.Core.Interops.Signatures;

namespace Vlc.DotNet.Core.Interops
{
    public sealed partial class VlcManager
    {
        public void Resume(VlcMediaPlayerInstance mediaPlayerInstance)
        {
            SetPause(mediaPlayerInstance, false);
        }

        public void SetPause(VlcMediaPlayerInstance mediaPlayerInstance, bool paused)
        {
            if (mediaPlayerInstance == IntPtr.Zero)
                throw new ArgumentException("Media player instance is not initialized.");
            GetInteropDelegate<SetPause>().Invoke(mediaPlayerInstance, paused ? 0 : 1);
        }
    }
}
