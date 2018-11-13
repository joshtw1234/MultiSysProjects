using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace OMENCmediaSDK.OMENSDK.Structures
{
    class OMENStructures
    {
    }

    public enum OMENVolumeChannel
    {
        Master = -1,
        FrontLeft,
        FrontRight
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void OMENSDKCallback(int type, int id, int componentType, ulong eventId);

    public struct OMENReturnValue
    {
        public int RevCode { get; set; }
        public string RevValue { get; set; }
        public string RevMessage { get; set; }
        public string RevExtraValue { get; set; }
    }

    public struct OMENClientData
    {
        const string BuildInName = "System";
        public string ApiName { get; set; }
        public object SetValue { get; set; }
        public object SetExtraValue { get; set; }
        public byte[] SetValueToByteArray()
        {
            if (SetValue == null)
            {
                return null;
            }
            return GetObjectBytes(SetValue);
        }

        public byte[] SetExtraValueToByteArray()
        {
            if (SetExtraValue == null)
            {
                return null;
            }
            return GetObjectBytes(SetExtraValue);
        }

        private byte[] GetObjectBytes(object objData)
        {
            if (objData.GetType().Namespace.Equals(BuildInName))
            {
                //Build in types
                if (objData is int)
                {
                    return BitConverter.GetBytes((int)objData);
                }
                if (objData is float)
                {
                    return BitConverter.GetBytes((float)objData);
                }
                if (objData is double)
                {
                    return BitConverter.GetBytes((double)objData);
                }
                if (objData is bool)
                {
                    return BitConverter.GetBytes((bool)objData);
                }
            }
            if (objData is OMENVolumeChannel)
            {
                return BitConverter.GetBytes((int)objData);
            }
            return ObjectToByteArray(objData);
        }

        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null) return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }

    public struct VolumeChannelSturcture
    {
        public OMENVolumeChannel ChannelIndex { get; set; }
        public float ChannelValue { get; set; }
    }

    public class BaseVolumeControlStructure
    {
        public int IsMuted { get; set; }
        public List<VolumeChannelSturcture> ChannelValues { get; set; }
    }

    public class VolumeControlStructure : BaseVolumeControlStructure
    {
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public double StepValue { get; set; }
        public double ScalarValue { get; set; }
    }

    enum OMENDataFlow
    {
        Render,
        Capture
    }
}
