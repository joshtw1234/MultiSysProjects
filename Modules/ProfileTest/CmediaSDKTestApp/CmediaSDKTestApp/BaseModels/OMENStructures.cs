using System;
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
            if (SetValue == null || !SetValue.GetType().Name.Equals(BuildInName))
            {
                return null;
            }

            return ObjectToByteArray(SetValue);
        }

        public byte[] SetExtraValueToByteArray()
        {
            if (SetExtraValue == null || !SetExtraValue.GetType().Name.Equals(BuildInName))
            {
                return null;
            }

            return ObjectToByteArray(SetExtraValue);
        }

        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null) return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    struct OMENREVData
    {
        public int RevCode { get; set; }
        public string RevValue { get; set; }
        public string RevMessage { get; set; }
        public string RevExtraValue { get; set; }
    }

    struct OMENChannelControlSturcture
    {
        public double ChannelIndex;
        public double ChannelValue;
    }
    class OMENVolumeControlStructure
    {
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public double StepValue { get; set; }
        public double ScalarValue { get; set; }
        public bool IsMuted { get; set; }
        public OMENChannelControlSturcture ChannelValue { get; set; }
    }

    enum OMENDataFlow
    {
        Render,
        Capture
    }
}
