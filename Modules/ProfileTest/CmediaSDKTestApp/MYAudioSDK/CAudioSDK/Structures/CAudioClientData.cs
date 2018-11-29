using MYAudioSDK.CAudioSDK.Enums;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MYAudioSDK.CAudioSDK.Structures
{
    struct CAudioClientData
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
            if (objData is CAudioVolumeChannel)
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
}
