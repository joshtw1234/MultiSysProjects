// *****************************************************************************************************
// 
//   ClassAudioEndPointVolumeCallBack.cs
// 
// 
//   Description:
//      The implementation for IAudioEndPointVolumeCallBack
//
// *****************************************************************************************************
using System;

namespace AudioControlLib.Structures
{
    /// <summary>
    /// Implement this class prevent OnNotify function show up in public API
    /// </summary>
    class ClassAudioEndPointVolumeCallBack : IAudioEndpointVolumeCallback
    {
        /// <summary>
        /// For receive End point volume change, do not call this function out side
        /// </summary>
        /// <param name="pNotify"></param>
        public void OnNotify(IntPtr pNotify)
        {
            if (null == pNotify) return;

            callBack?.Invoke(AudioVolumeNotificationData.MarshalFromPtr(pNotify));
        }

        /// <summary>
        /// The Volume Change Call back
        /// </summary>
        CallBacks.AudioVolumeChangeCallBack callBack;
        /// <summary>
        /// Register Volume Change Call Back
        /// </summary>
        /// <param name="_callBack"></param>
        public void RegisterVolumeChangeCallBack(CallBacks.AudioVolumeChangeCallBack _callBack)
        {
            callBack += _callBack;
        }
        /// <summary>
        /// UnRegister Volume Change Call Back
        /// </summary>
        /// <param name="_callBack"></param>
        public void UnRegisterVolumeChangeCallBack(CallBacks.AudioVolumeChangeCallBack _callBack)
        {
            callBack -= _callBack;
        }
    }
}
