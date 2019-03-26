using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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
            var dataFiles = Directory.GetFiles(lottoryDir);
            if (dataFiles.Count() != 71)
            {
                /*
                 * 03/27/2019 has 71 page on website
                 * use this number prevent re download files
                 * TODO: find a way to get page numbers.
                 */
                for (int i = 1; i < 72; i++)
                {
                    webLink = $"https://www.pilio.idv.tw/ltobig/ServerC/list.asp?indexpage={i}&orderby=new";
                    saveWebFile = $"Lottory{i}.txt";
                    DownloadWeb(webLink, lottoryDir, saveWebFile);
                }
            }

            GetLottoryHistory(lottoryDir);
        }
        
        List<LottoryInfo> LottoryHistory;
        private void GetLottoryHistory(string dataDir)
        {
            LottoryHistory = new List<LottoryInfo>();
            List<LottoryInfo> rawData = new List<LottoryInfo>();
            var dataFiles = Directory.GetFiles(dataDir);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (var dataPath in dataFiles)
            {
                rawData.AddRange(GetLottoryData(dataPath));
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalSeconds);
            LottoryHistory.AddRange(rawData.OrderByDescending(x => x.Date).ToList());
        }

        private List<LottoryInfo> GetLottoryData(string dataFile)
        {
            const string getNumberPat = "\">[\\s]+([^\\s]*)[\\s]+</td>";
            const string numberKey = ",&nbsp;";
            const string getSpecialNum = @">(&[^\r\n]*)</td>";
            const string spcialNumberKey = "&nbsp;";
            const string getFieldPat = "\">([^\\r\\n&]*)</td>";
            const string dateKey = "<br />";

            List<LottoryInfo> tmpList = new List<LottoryInfo>();
            string rawFile = File.ReadAllText(dataFile);
            var mcNumbers = Regex.Matches(rawFile, getNumberPat);
            
            foreach (Match mt in mcNumbers)
            {
                var nums = mt.Groups[1].Value;
                if (!nums.Contains(numberKey)) continue;
                tmpList.Add(new LottoryInfo()
                {
                    LottoryNumbers = Array.ConvertAll(nums.Split(new string[] { numberKey }, StringSplitOptions.None), s => int.Parse(s)).ToList()
                });
            }
            var mcSpecial = Regex.Matches(rawFile, getSpecialNum);
            for (int i = 0; i < mcSpecial.Count; i++)
            {
                var nums = mcSpecial[i].Groups[1].Value;
                nums = nums.Replace(spcialNumberKey, string.Empty);
                tmpList[i].SpecialNumber = int.Parse(nums);
            }
            var mcDate = Regex.Matches(rawFile, getFieldPat);
            int listCnt = 0;
            foreach (Match mt in mcDate)
            {
                var strDate = mt.Groups[1].Value;
                if (!strDate.Contains(dateKey)) continue;
                int dateIdx = strDate.LastIndexOf('>');
                strDate = strDate.Remove(dateIdx + 1);
                strDate = strDate.Replace(dateKey, ":");
                var dd = strDate.Split(':');
                strDate = $"{dd[1]}/{dd[0]}";
                tmpList[listCnt].Date = Convert.ToDateTime(strDate);
                listCnt++;
            }
            return tmpList;
        }

        private void DownloadWeb(string webLink, string saveDir, string saveFile)
        {
            WebClient client = new WebClient();
            client.DownloadFile(webLink, Path.Combine(saveDir, saveFile));
        }
    }
    class LottoryInfo
    {
        public DateTime Date { get; set; }
        public List<int> LottoryNumbers { get; set; }
        public int SpecialNumber { get; set; }
    }
}
