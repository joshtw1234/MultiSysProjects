using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using BigLottoryModule.Interface;
using CommonUILib.Interfaces;
using CommonUILib.Models;

namespace BigLottoryModule.ViewModels
{
    class BigLottoryControlViewModel : IDebugOutPutControlViewModel
    {
        private IBigLottoryControlModel _model;

        List<LottoryInfo> LottoryHistory;
        public IViewItem DebugMessage { get; set; }
        public IViewItem TextProgress { get; set; }
        public IViewItem ViewProgressBar { get; set; }

        public BigLottoryControlViewModel(IBigLottoryControlModel model)
        {
            _model = model;
            DebugMessage = new DebugViewItem()
            {
                MenuName = "Hello Word",
                MenuStyle = Application.Current.Resources["BaseTextBoxStyle"] as Style,
            };
            TextProgress = new DebugViewItem()
            {
                MenuName = "Please wait processing....",
                MenuStyle = Application.Current.Resources["BaseTextBoxStyle"] as Style,
                MenuVisibility = true
            };

            ViewProgressBar = new ProgressBarViewItem()
            {
                MenuName="50",
                MenuMinValue="0",
                MenuMaxValue="100",
                MenuStyle = Application.Current.Resources["CustomProgressBar"] as Style,
            };
            Task.Factory.StartNew(() => 
            {
                while(true)
                {
                    for(int i = 0; i <= 100; i+=10)
                    {
                        Thread.Sleep(100);
                        ViewProgressBar.MenuName = i.ToString();
                    }
                }
            });
        }

        private void LottoryDataProcess()
        {
            string webLink = "https://www.pilio.idv.tw/ltobig/ServerC/list.asp?indexpage=1&orderby=new";
            string currentPath = Directory.GetCurrentDirectory();
            string lottoryDir = $"{currentPath}\\LottoryData";
            string saveWebFile = "Lottory";
            if (!Directory.Exists(lottoryDir))
            {
                Directory.CreateDirectory(lottoryDir);
            }
            Stopwatch sw = new Stopwatch();
            sw.Start();

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
            sw.Stop();
            Console.WriteLine($"DownloadWeb done {sw.Elapsed.TotalSeconds}");
            sw.Restart();
            GetLottoryHistory(lottoryDir);
            GetPacent(LottoryHistory);
            sw.Stop();
            Console.WriteLine($"GetLottoryHistory done {sw.Elapsed.TotalSeconds}");
            LottoryHistory.RemoveAt(0);
            DebugMessage.MenuName += $"\nRemove 3/26 numbers Total {LottoryHistory.Count}";
            GetPacent(LottoryHistory);
            LottoryHistory.RemoveAt(0);
            DebugMessage.MenuName += $"\nRemove 3/22 numbers Total {LottoryHistory.Count}";
            GetPacent(LottoryHistory);
            LottoryHistory.RemoveAt(0);
            DebugMessage.MenuName += $"\nRemove 3/19 numbers Total {LottoryHistory.Count}";
            GetPacent(LottoryHistory);
        }

        private void GetPacent(List<LottoryInfo> lottoryHistory)
        {
            Dictionary<int, double> dicTable = new Dictionary<int, double>();
            Dictionary<int, double> dic11Table = new Dictionary<int, double>();
            Dictionary<int, double> dic10Table = new Dictionary<int, double>();
            for (int i = 1; i < 50; i++)
            {
                var num1Cnt = lottoryHistory.Where(x => x.LottoryNumbers.Contains(i)).ToList();
                double pacent = (double)num1Cnt.Count / (double)lottoryHistory.Count * 100;
                //DebugMessage.MenuName += $"\n number {i} count {num1Cnt.Count} pacent {pacent.ToString("0.00")}%";
                if (pacent < 12 && pacent > 11)
                {
                    dic11Table.Add(i, pacent);
                }
                else if (pacent < 11 && pacent > 10)
                {
                    dic10Table.Add(i, pacent);
                }
                else
                {
                    dicTable.Add(i, pacent);
                }
            }
            PrintDicTable(11, dic11Table);
            PrintDicTable(10, dic10Table);
            PrintDicTable(12, dicTable);
        }

        private void PrintDicTable(int tableName, Dictionary<int, double> dicTable)
        {
            string tt = $"{tableName}% {dicTable.Count}";
            foreach (var dd in dicTable)
            {
                tt += $" [{dd.Key}] [{dd.Value.ToString("0.000")}]";
            }
            DebugMessage.MenuName += $"\n {tt}";
        }

       

        private void GetLottoryHistory(string dataDir)
        {
            LottoryHistory = new List<LottoryInfo>();
            List<LottoryInfo> rawData = new List<LottoryInfo>();
            var dataFiles = Directory.GetFiles(dataDir);
           
            foreach (var dataPath in dataFiles)
            {
                rawData.AddRange(GetLottoryData(dataPath));
            }
            
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
