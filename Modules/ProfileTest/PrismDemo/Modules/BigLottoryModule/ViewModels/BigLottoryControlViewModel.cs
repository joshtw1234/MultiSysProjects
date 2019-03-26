using System;
using System.IO;
using System.Net;
using BigLottoryModule.Interface;

namespace BigLottoryModule.ViewModels
{
    class BigLottoryControlViewModel
    {
        private IBigLottoryControlModel _model;
        public BigLottoryControlViewModel(IBigLottoryControlModel model)
        {
            _model = model;
            string webLink = "https://www.pilio.idv.tw/ltobig/ServerC/list.asp?indexpage=1&orderby=new";
            string currentPath = Directory.GetCurrentDirectory();
            string lottoryDir = $"{currentPath}\\LottoryData";
            string saveWebFile = "Lottory";
            if (!Directory.Exists(lottoryDir))
            {
                Directory.CreateDirectory(lottoryDir);
            }
            for (int i = 1; i < 72; i++)
            {
                webLink = $"https://www.pilio.idv.tw/ltobig/ServerC/list.asp?indexpage={i}&orderby=new";
                saveWebFile = $"Lottory{i}.txt";
                DownloadWeb(webLink, lottoryDir, saveWebFile);
            }
            
        }

        private void DownloadWeb(string webLink, string saveDir, string saveFile)
        {
            WebClient client = new WebClient();
            client.DownloadFile(webLink, Path.Combine(saveDir, saveFile));
        }
    }
}
