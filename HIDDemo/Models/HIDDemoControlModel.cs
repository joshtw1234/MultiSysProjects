using System.Collections.Generic;
using HIDLib;

namespace HIDDemo.Models
{
    class HIDDemoControlModel : IHIDDemoControlModel
    {
        List<HIDInfo> lstHIDDevs;
        MessageTextDCT msgText;

        /// <summary>
        /// Print byte array to string.
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="msg"></param>
        private void PrintByteToString(byte[] pData, string msg = "")
        {
#if DEBUG
            if (null == pData)
            {
                msgText.MsgText += string.Format("\r\nPrintByte Msg {0} Data 0", msg);
                return;
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //
            foreach (byte bData in pData)
            {
                sb.Append(bData.ToString("X2"));
                sb.Append("|");
            }
            msgText.MsgText += ($"\r\n{msg} Data {sb.ToString()}");
            sb.Clear();
#endif
        }

        public List<HIDInfo> GetHIDInfoCollections
        {
            get
            {
                BaseHID bhid = new BaseHID();
                lstHIDDevs = new List<HIDInfo>();
                lstHIDDevs.AddRange(bhid.BrowseHID());
                return lstHIDDevs;
            }
        }

        public MessageTextDCT GetMessageText
        {
            get
            {
                msgText = new MessageTextDCT()
                {
                   MsgText = "Hello world!!!"
                };
                return msgText;
            }
        }

        public void SetHIDClose(int selectHIDIdx)
        {
            lstHIDDevs[selectHIDIdx].HIDClose();
        }

        public bool SetHIDOpen(int selectHIDIdx)
        {
            return lstHIDDevs[selectHIDIdx].HIDOpen();
        }

        public void SetHIDSend(int selectHIDIdx, byte[] data)
        {
            //byte[] wData = PriMaxKBHID.GetCmdKeyboardLang();
            PrintByteToString(data);
            if (lstHIDDevs[selectHIDIdx].HIDWrite(data))
            {
                byte[] revData = lstHIDDevs[selectHIDIdx].HIDRead();
                PrintByteToString(revData);
            }
            else
            {
                msgText.MsgText += "\r\nHID Write Error";
            }
        }

        public bool SetHIDOpenAsync(int selectHIDIdx)
        {
            return lstHIDDevs[selectHIDIdx].HIDOpenAsync();
        }
    }
}
