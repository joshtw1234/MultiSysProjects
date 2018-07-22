using PipeClient;
using System;
using System.Text;

namespace UpdateUI
{
    enum CommandID
    {
        COMMAND_X = 0xB1,
        COMMAND_GetOMENUpdate = 0xB2,
        COMMAND_IsHPSARunning = 0xB3,
        COMMAND_GetNetWork = 0xB4,
    };
    public class PipeClientUpdate : PipeClientBase
    {
        public PipeClientUpdate(string path) : base(path)
        {
        }


        PipeData InitPipeData(CommandID cmmID, object vol)
        {
            PipeData pData = new PipeData();
            pData.commandId = (int)cmmID;
            pData.Data = new byte[256];
            string strVal = string.Empty;
            switch (cmmID)
            {
                //case CommandID.COMMAND_SetMultiper:
                //    strVal = vol.ToString();
                //    break;
                //case CommandID.COMMAND_SetVoltage:
                //    strVal = ((double)vol).ToString(MainControl.inSideculInfo);
                //    break;
                //case CommandID.COMMAND_GetWMIValue:
                //    strVal = ((int)vol).ToString(MainControl.inSideculInfo);
                //    break;
                //case CommandID.COMMAND_GetMultiper:
                //case CommandID.COMMAND_GetVoltage:
                //case CommandID.COMMAND_GetFrequency:
                //case CommandID.COMMAND_GetDefault:
                //case CommandID.COMMAND_GetCurrent:
                //case CommandID.COMMAND_SetBootAble:
                //    break;
                default:
                    break;
            }
            BitConverter.GetBytes(strVal.Length).CopyTo(pData.Data, 0);
            Encoding.ASCII.GetBytes(strVal).CopyTo(pData.Data, 1);
            return pData;
        }
        bool WritePipeData(PipeData pData)
        {
            if (this.protocol == null)
            {
                return false;
            }
            return this.protocol.Write(pData);
        }

        public void SendCommandX()
        {
            WritePipeData(InitPipeData(CommandID.COMMAND_X, string.Empty));
        }

        public void GetIsHPSARunning()
        {
            WritePipeData(InitPipeData(CommandID.COMMAND_IsHPSARunning, string.Empty));
        }

        public void GetOMENUpdate()
        {
            WritePipeData(InitPipeData(CommandID.COMMAND_GetOMENUpdate, string.Empty));
        }

        public void GetNetWorkConnect()
        {
            WritePipeData(InitPipeData(CommandID.COMMAND_GetNetWork, string.Empty));
        }
    }
}
