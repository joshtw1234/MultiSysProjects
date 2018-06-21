namespace HIDHeadSet.Models
{
    public interface IHeadSetModel
    {
        bool Initialize();
        bool OpenHID();
        void CloseHID();
        bool WriteHID(byte[] data);
    }
}
