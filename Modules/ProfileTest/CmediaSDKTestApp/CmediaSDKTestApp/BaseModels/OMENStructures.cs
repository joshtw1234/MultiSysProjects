using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CmediaSDKTestApp.BaseModels
{
    class OMENStructures
    {
    }

    struct OMENClientData
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
                if (objData is double)
                {
                    return BitConverter.GetBytes((double)objData);
                }
                if (objData is bool)
                {
                    return BitConverter.GetBytes((bool)objData);
                }
            }
            if (objData is VolumeChannel)
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

    struct OMENREVData
    {
        public int RevCode { get; set; }
        public string RevValue { get; set; }
        public string RevMessage { get; set; }
        public string RevExtraValue { get; set; }
    }

    enum VolumeChannel
    {
        Master = -1,
        FrontLeft,
        FrontRight
    }

    struct VolumeChannelSturcture
    {
        public VolumeChannel ChannelIndex { get; set; }
        public double ChannelValue { get; set; }
    }

    class BaseVolumeControlStructure
    {
        public int IsMuted { get; set; }
        public List<VolumeChannelSturcture> ChannelValues { get; set; }
    }

    class VolumeControlStructure : BaseVolumeControlStructure
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
