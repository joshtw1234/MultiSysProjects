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
            if (SetValue.GetType().Namespace.Equals(BuildInName))
            {
                //Build in types
                if (SetValue is int)
                {
                    return BitConverter.GetBytes((int)SetValue);
                }
            }
            if (SetValue is OMENChannel)
            {
                return BitConverter.GetBytes((int)SetExtraValue);
            }
            return ObjectToByteArray(SetValue);
        }

        public byte[] SetExtraValueToByteArray()
        {
            if (SetExtraValue == null)
            {
                return null;
            }
            if (SetExtraValue.GetType().Namespace.Equals(BuildInName))
            {
                //Build in types
                if (SetExtraValue is int)
                {
                    return BitConverter.GetBytes((int)SetExtraValue);
                }
            }
            if (SetExtraValue is OMENChannel)
            {
                return BitConverter.GetBytes((int)SetExtraValue);
            }
            return ObjectToByteArray(SetExtraValue);
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

    enum OMENChannel
    {
        Master = -1,
        FrontLeft,
        FrontRight
    }

    struct OMENChannelControlSturcture
    {
        public OMENChannel ChannelIndex { get; set; }
        public double ChannelValue { get; set; }
    }
    class OMENVolumeControlStructure
    {
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public double StepValue { get; set; }
        public double ScalarValue { get; set; }
        public bool IsMuted { get; set; }
        public List<OMENChannelControlSturcture> ChannelValues { get; set; }
    }

    enum OMENDataFlow
    {
        Render,
        Capture
    }
}
