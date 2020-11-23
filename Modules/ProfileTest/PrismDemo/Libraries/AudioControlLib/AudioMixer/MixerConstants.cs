
namespace AudioControlLib.AudioMixer
{
    class MixerConstants
    {
        #region Constants
        public const int MMSYSERR_BASE = 0;
        public const int WAVERR_BASE = 32;
        public const int MIXER_SHORT_NAME_CHARS = 16;
        public const int MIXER_LONG_NAME_CHARS = 64;
        public const int MAXPNAMELEN = 32;     /* max product name length (including NULL) */
        public const int MIXERR_BASE = 1024;
        public const int CALLBACK_WINDOW = 0x00010000;    /* dwCallback is a HWND */
        public const int MM_MIXM_LINE_CHANGE = 0x3D0;       /* mixer line change notify */
        public const int MM_MIXM_CONTROL_CHANGE = 0x3D1;
        #endregion
    }
}
