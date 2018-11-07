namespace CmediaSDKTestApp.BaseModels
{
    class OMENStructures
    {
    }

    struct OMENClientData
    {
        public string ApiName { get; set; }
        public byte[] WriteValue { get; set; }
        public byte[] WriteExtraValue { get; set; }
    }

    struct OMENREVData
    {
        public int RevCode { get; set; }
        public string RevValue { get; set; }
        public string RevMessage { get; set; }
        public string RevExtraValue { get; set; }
    }
}
