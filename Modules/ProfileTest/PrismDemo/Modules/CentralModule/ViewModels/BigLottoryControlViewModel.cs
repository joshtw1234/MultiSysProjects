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
using CentralModule.Interface;
using CommonUILib.Interfaces;
using CommonUILib.Models;

namespace CentralModule.ViewModels
{
    class BigLottoryControlViewModel : IDebugOutPutControlViewModel, IProgressBarControlViewModel
    {
        private IBigLottoryControlModel _model;

        List<LottoryInfo> LottoryHistory;
        Dictionary<int, List<int>> ResultNumbers;
        public IViewItem DebugMessage { get; set; }
        public IViewItem TextProgress { get; set; }
        public IViewItem ViewProgressBar { get; set; }

        public BigLottoryControlViewModel(IBigLottoryControlModel model)
        {
            _model = model;
            DebugMessage = new DebugViewItem()
            {
                MenuName = "Hello Word\n",
                MenuStyle = Application.Current.Resources["BaseTextBoxStyle"] as Style,
            };
            TextProgress = new DebugViewItem()
            {
                MenuName = "Please wait processing....",
                MenuStyle = Application.Current.Resources["BaseTextBoxStyle"] as Style,
                MenuVisibility = false
            };

            ViewProgressBar = new ProgressBarViewItem()
            {
                MenuName="50",
                MenuMinValue="0",
                MenuMaxValue="100",
                MenuStyle = Application.Current.Resources["CustomProgressBar"] as Style,
            };
            ResultNumbers = new Dictionary<int, List<int>>();
            LottoryDataProcess();
        }



        private void LottoryDataProcess()
        {
            const string infoFile = "info.txt";
            string webLink = "https://www.pilio.idv.tw/ltobig/ServerC/list.asp?indexpage=1&orderby=new";
            string currentPath = Directory.GetCurrentDirectory();
            string lottoryDir = $"{currentPath}\\LottoryData";
            string saveWebFile = "Lottory";
            if (!Directory.Exists(lottoryDir))
            {
                Directory.CreateDirectory(lottoryDir);
            }
            if (!DownloadWeb(webLink, currentPath, infoFile)) return;
            int pageCount = GetWebPageCount(Path.Combine(currentPath, infoFile));

            var dataFiles = Directory.GetFiles(lottoryDir);

            if (dataFiles.Count() != pageCount)
            {
                TextProgress.MenuVisibility = true;
                Task.Factory.StartNew(() =>
                {
                    while (TextProgress.MenuVisibility)
                    {
                        for (int i = 0; i <= 100; i += 10)
                        {
                            Thread.Sleep(100);
                            ViewProgressBar.MenuName = i.ToString();
                        }
                    }
                });
                Task.Factory.StartNew(() =>
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    /*
                     * 03/27/2019 has 71 page on website
                     * use this number prevent re download files
                     * TODO: find a way to get page numbers.
                     */
                    for (int i = 1; i < pageCount + 1; i++)
                    {
                        webLink = $"https://www.pilio.idv.tw/ltobig/ServerC/list.asp?indexpage={i}&orderby=new";
                        saveWebFile = $"Lottory{i}.txt";
                        if (!DownloadWeb(webLink, lottoryDir, saveWebFile)) return;
                    }
                    sw.Stop();
                    Console.WriteLine($"DownloadWeb done {sw.Elapsed.TotalSeconds}");
                    LoadLottoryHistory(lottoryDir);
                    TextProgress.MenuVisibility = false;
                });
            }
            else
            {
                LoadLottoryHistory(lottoryDir);
            }

        }
        int hisCount = 0;
        void LoadLottoryHistory(string lottoryDir)
        {
            GetLottoryHistory(lottoryDir);
#if true
            hisCount = LottoryHistory.Count;
            GetRecentNewNumbers(LottoryHistory);
            var newHis = GetNewHistory(LottoryHistory.Count);
            GetRecentNewNumbers(newHis);
            while(newHis.Count > 50)
            {
                newHis = GetNewHistory(newHis.Count);
                GetRecentNewNumbers(newHis);
            }
            List<int> bigList = new List<int>();
            foreach(var rall in ResultNumbers)
            {
                DebugMessage.MenuName += $"Count {rall.Key} || ";
                bigList.AddRange(rall.Value);
                foreach (var rnum in rall.Value)
                {
                    DebugMessage.MenuName += $"{rnum.ToString()} ";
                }
                DebugMessage.MenuName += "\n";
            }
           var gg = bigList.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();
            foreach (var vv in gg)
            {
                var cc = bigList.Where(x => x.Equals(vv));
                DebugMessage.MenuName += $"[{vv.ToString("00")}] ";
            }
            DebugMessage.MenuName += "\n";
            foreach (var vv in gg)
            {
                var cc = bigList.Where(x => x.Equals(vv));
                DebugMessage.MenuName += $"[{cc.Count().ToString("00")}] ";
            }
            DebugMessage.MenuName += "\n";

            var ddd = bigList.Where(x => !gg.Contains(x));
            foreach (var vv in ddd)
            {
                var cc = bigList.Where(x => x.Equals(vv));
                DebugMessage.MenuName += $"[{vv.ToString("00")}] ";
            }
            DebugMessage.MenuName += "\n";
            foreach (var vv in ddd)
            {
                var cc = bigList.Where(x => x.Equals(vv));
                DebugMessage.MenuName += $"[{cc.Count().ToString("00")}] ";
            }
            DebugMessage.MenuName += "\n";
            File.WriteAllText($"{Directory.GetCurrentDirectory()}\\Lottory{DateTime.Now.ToString("yyyyMMddHHmm")}.txt", DebugMessage.MenuName);
            //newHis = GetNewHistory(newHis.Count);
            //GetRecentNewNumbers(newHis);
            //newHis = GetNewHistory(newHis.Count);
            //GetRecentNewNumbers(newHis);
            //newHis = GetNewHistory(newHis.Count);
            //GetRecentNewNumbers(newHis);
#else
            
            //Console.WriteLine($"GetLottoryHistory done {sw.Elapsed.TotalSeconds}");
            LottoryHistory.RemoveAt(0);
            DebugMessage.MenuName += $"\nRemove 3/26 numbers Total {LottoryHistory.Count}";
            GetPacent(LottoryHistory);
            LottoryHistory.RemoveAt(0);
            DebugMessage.MenuName += $"\nRemove 3/22 numbers Total {LottoryHistory.Count}";
            GetPacent(LottoryHistory);
            LottoryHistory.RemoveAt(0);
            DebugMessage.MenuName += $"\nRemove 3/19 numbers Total {LottoryHistory.Count}";
            GetPacent(LottoryHistory);
#endif
        }

        private List<LottoryInfo> GetNewHistory(int count)
        {
            int cnt = GetHistoryCount(count);
            hisCount = cnt;
            return LottoryHistory.GetRange(0, cnt);
        }

        private void GetRecentNewNumbers(List<LottoryInfo> lottoryHistory)
        {
            var allTable = GetPacent(lottoryHistory);
            GetNewNumber(allTable);
        }

        private int GetHistoryCount(int count)
        {
            if (count % 2 != 0)
            {
                count -= 1;
            }
            int rev = count / 2;
            DebugMessage.MenuName += $"New History {rev}\n";
            return rev;
        }

        private void GetNewNumber(Dictionary<int, double> allTable)
        {
            var ddd = allTable.OrderByDescending(x => x.Value).ToDictionary(x=>x.Key, x=>x.Value).Keys.ToList().GetRange(43, 6);
            ResultNumbers.Add(hisCount, ddd);
            DebugMessage.MenuName += "New Number \n";
            foreach (var vv in ddd)
            {
                DebugMessage.MenuName += $"{vv.ToString()} ";
            }
            DebugMessage.MenuName += "\n";
        }

        private Dictionary<int, double> GetPacent(List<LottoryInfo> lottoryHistory)
        {
            Dictionary<int, double> dicTable = new Dictionary<int, double>();
            Dictionary<int, double> dic11Table = new Dictionary<int, double>();
            Dictionary<int, double> dic10Table = new Dictionary<int, double>();
            Dictionary<int, double> dicAllTable = new Dictionary<int, double>();
            for (int i = 1; i < 50; i++)
            {
                var num1Cnt = lottoryHistory.Where(x => x.LottoryNumbers.Contains(i)).ToList();
                double pacent = (double)num1Cnt.Count / (double)lottoryHistory.Count * 100;
                dicAllTable.Add(i, pacent);
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
            //PrintDicTable(11, dic11Table);
            //PrintDicTable(10, dic10Table);
            //PrintDicTable(12, dicTable);
            PrintDicTable(99, dicAllTable);
            return dicAllTable;
        }

        private void PrintDicTable(int tableName, Dictionary<int, double> dicTable)
        {
            string tt = $"{tableName}% Cnt {dicTable.Count} \n";
            var ddd = dicTable.OrderByDescending(x => x.Value);
            foreach (var dd in ddd)
            {
                tt += $" [{dd.Key}] <{dd.Value.ToString("0.000")}>";
            }
            DebugMessage.MenuName += $"\n {tt}\n";
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
            DebugMessage.MenuName += $"Files {dataFiles.Length} History Length {LottoryHistory.Count}\n";
        }

        private int GetWebPageCount(string samplePage)
        {
            int rev = -1;
            const string pageCountPat = "<option[^\\r\\n]+\">([^\\s]+)</option>";
            string rawFile = File.ReadAllText(samplePage);
            var mcNumbers = Regex.Matches(rawFile, pageCountPat);
            var pageList = mcNumbers.Cast<Match>().Select(m => m.Groups[1].Value).ToList();
            rev = int.Parse(pageList.Last());
            return rev;
        }

        private List<LottoryInfo> GetLottoryData(string dataFile)
        {
            const string getNumberPat = "\">[\\s]+([^\\s]*)[\\s]+</td>";
            const string numberKey = ",&nbsp;";
            const string getSpecialNum = @">([\d]{2})</td>";
            const string spcialNumberKey = "&nbsp;";
            const string getFieldPat = "\">([\\d/]*)<br>";
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
#if false
                if (!strDate.Contains(dateKey)) continue;
                int dateIdx = strDate.LastIndexOf('>');
                strDate = strDate.Remove(dateIdx + 1);
                strDate = strDate.Replace(dateKey, ":");
                var dd = strDate.Split(':');
                strDate = $"{dd[1]}/{dd[0]}";
#endif
                tmpList[listCnt].Date = Convert.ToDateTime(strDate);
                listCnt++;
            }
            return tmpList;
        }

        private bool DownloadWeb(string webLink, string saveDir, string saveFile)
        {
            bool rev = true;
            try
            {
                WebClient client = new WebClient();
                client.DownloadFile(webLink, Path.Combine(saveDir, saveFile));
            }catch(Exception ex)
            {
                DebugMessage.MenuName += $"\nDownloadWeb exception [{ex.Message}]";
                rev = false;
            }
            return rev;
        }
    }
    class LottoryInfo
    {
        public DateTime Date { get; set; }
        public List<int> LottoryNumbers { get; set; }
        public int SpecialNumber { get; set; }
    }
}
