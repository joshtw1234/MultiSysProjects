using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace MYAudioSDK.CAudioSDK.Structures
{
    #region 7.1 Surround
    enum HPSurroundCommand
    {
        XEAR_SURR_HP_ENABLE = -1,
        XEAR_SURR_HP_ROOM,
        XEAR_SURR_HP_MODE,
        XEAR_SURR_HP_TOP
    }
    enum HPSurroundValueType
    {
        ValueType_LONG = 0,
        ValueType_FLOAT = 1
    }
    enum HPSurroundFunction
    {
        KSPROPERTY_VIRTUALSURROUND_GETGUID = 1,
        KSPROPERTY_VIRTUALSURROUND_GETNUMOFPARAMELEMENT = 2,
        KSPROPERTY_VIRTUALSURROUND_GETPARAMRANGE = 3,
        KSPROPERTY_VIRTUALSURROUND_PARAMSVALUE = 4
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct CmediaRegisterOperation
    {
        public HPSurroundFunction Operation;
        public HPSurroundCommand Feature;
        public HPSurroundValueType ValueType;
        public byte[] ToBytes()
        {
            byte[] bytes = new byte[Marshal.SizeOf(typeof(CmediaRegisterOperation))];
            GCHandle pinStructure = GCHandle.Alloc(this, GCHandleType.Pinned);
            try
            {
                Marshal.Copy(pinStructure.AddrOfPinnedObject(), bytes, 0, bytes.Length);
                return bytes;
            }
            finally
            {
                pinStructure.Free();
            }
        }
    }
    #endregion

    #region Cmedia Enums
  
    
   

   

   
    #endregion

    #region Cmedia Function Point
    

    #endregion

    #region Custom structure

    

   

    
    #endregion
}
