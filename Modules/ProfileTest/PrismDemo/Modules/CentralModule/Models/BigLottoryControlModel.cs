using CentralModule.Interface;
using CommonUILib.Interfaces;
using CommonUILib.Models;
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

namespace CentralModule.Models
{
    public class BigLottoryControlModel : IBigLottoryControlModel
    {
        int hisCount = 0;
        List<LottoryInfo> lottoryHistory;
        Dictionary<int, List<int>> resultNumbers;

        private DebugControlModel _lottoryNumMessage;
        private DebugControlModel _lottoryOpenMessage;
        public IViewItem GetDebugMessage()
        {
            return new DebugViewItem()
            {
                MenuName = "Hello Word\n",
                MenuStyle = Application.Current.Resources["BaseTextBoxStyle"] as Style,
            };
        }
        public IViewItem GetTextProgress()
        {
            return new DebugViewItem()
            {
                MenuName = "Please wait processing....",
                MenuStyle = Application.Current.Resources["BaseTextBoxStyle"] as Style,
                MenuVisibility = false
            };
        }
        public IViewItem GetViewProgressBar()
        {
           return new ProgressBarViewItem()
            {
                MenuName = "50",
                MenuMinValue = "0",
                MenuMaxValue = "100",
                MenuStyle = Application.Current.Resources["CustomProgressBar"] as Style,
            };
        }
        public void LottoryDataProcess()
        {
            lottoryHistory = new List<LottoryInfo>();
            resultNumbers = new Dictionary<int, List<int>>();
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
                _lottoryNumMessage.TextProgress.MenuVisibility = true;
                Task.Factory.StartNew(() =>
                {
                    while (_lottoryNumMessage.TextProgress.MenuVisibility)
                    {
                        for (int i = 0; i <= 100; i += 10)
                        {
                            Thread.Sleep(100);
                            _lottoryNumMessage.ViewProgressBar.MenuName = i.ToString();
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
                    _lottoryNumMessage.TextProgress.MenuVisibility = false;
                });
            }
            else
            {
                LoadLottoryHistory(lottoryDir);
            }
        }

        public void LottoryDataByOpen()
        {
#if false
            _lottoryOpenMessage.DebugMessage.MenuName = "Josh\n";
            string openLott = File.ReadAllText(@"D:\z-JoshCodeWork\TestLottary\大樂透落球順序開獎號碼查詢 - 樂透堂.html");
            var newLott = new List<LottoryInfo>();
            const string ppt = @"<td align[^\r\n]+>([\d]{6})<[^\r\n]+[\s]+<[^\r\n]+([\d-]{10})[^\r\n]+[\s]+[^\r\n]+>[\s]+([^\r\n]+)</[^\r\n]+([\d]{2})";
            const string dirtyw = "&nbsp;";
            MatchCollection mt = Regex.Matches(openLott, ppt);
            foreach (Match mm in mt)
            {
                var uuW = Array.ConvertAll(mm.Groups[3].Value.Replace(dirtyw, ",").Split(','), s => int.Parse(s)).ToList();

                newLott.Add(new LottoryInfo()
                {
                    Date = Convert.ToDateTime(mm.Groups[2].Value),
                    LottoryNumbers = uuW,
                    SpecialNumber = Convert.ToInt32(mm.Groups[4].Value)
                });
            }
            var openDicOrder = GetOpenPercent(newLott);
            GetNewOpenNumber(openDicOrder);
            int totalcc = newLott.Count;
            while(totalcc > 12)
            {
                var newcc = GetHistoryCount(_lottoryOpenMessage, totalcc);
                var startIdx = newLott.Count - newcc;
                var eeLott = newLott.GetRange(startIdx, newcc);
                openDicOrder = GetOpenPercent(eeLott);
                GetNewOpenNumber(openDicOrder);
                totalcc = newcc;
            }
#endif
        }
        
        public DebugControlModel GetDebugMessageModel()
        {
            return _lottoryNumMessage = new DebugControlModel()
            {
                DebugMessage = GetDebugMessage(),
                TextProgress = GetTextProgress(),
                ViewProgressBar = GetViewProgressBar()
            };
        }

        public DebugControlModel GetOpenMessage()
        {
            return _lottoryOpenMessage = new DebugControlModel()
            {
                DebugMessage = GetDebugMessage(),
                TextProgress = GetTextProgress(),
                ViewProgressBar = GetViewProgressBar()
            };
        }

        #region Private
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

        private bool DownloadWeb(string webLink, string saveDir, string saveFile)
        {
            bool rev = true;
            try
            {
                WebClient client = new WebClient();
                client.DownloadFile(webLink, Path.Combine(saveDir, saveFile));
            }
            catch (Exception ex)
            {
                _lottoryNumMessage.DebugMessage.MenuName += $"\nDownloadWeb exception [{ex.Message}]";
                rev = false;
            }
            return rev;
        }

        private void LoadLottoryHistory(string lottoryDir)
        {
            GetLottoryHistory(lottoryDir);

#if false
            hisCount = lottoryHistory.Count;
            GetRecentNewNumbers(lottoryHistory);
            var newHis = GetNewHistory(lottoryHistory.Count);
            GetRecentNewNumbers(newHis);
            while (newHis.Count > 12)
            {
                newHis = GetNewHistory(newHis.Count);
                GetRecentNewNumbers(newHis);
            }
            List<int> bigList = new List<int>();
            foreach (var rall in resultNumbers)
            {
                _lottoryNumMessage.DebugMessage.MenuName += $"Count {rall.Key} || ";
                bigList.AddRange(rall.Value);
                foreach (var rnum in rall.Value)
                {
                    _lottoryNumMessage.DebugMessage.MenuName += $"{rnum.ToString()} ";
                }
                _lottoryNumMessage.DebugMessage.MenuName += "\n";
            }
            var gg = bigList.GroupBy(x => x)
               .Where(g => g.Count() > 1)
               .Select(y => y.Key)
               .ToList();
            foreach (var vv in gg)
            {
                var cc = bigList.Where(x => x.Equals(vv));
                _lottoryNumMessage.DebugMessage.MenuName += $"[{vv.ToString("00")}] ";
            }
            _lottoryNumMessage.DebugMessage.MenuName += "\n";
            foreach (var vv in gg)
            {
                var cc = bigList.Where(x => x.Equals(vv));
                _lottoryNumMessage.DebugMessage.MenuName += $"[{cc.Count().ToString("00")}] ";
            }
            _lottoryNumMessage.DebugMessage.MenuName += "\n";

            var ddd = bigList.Where(x => !gg.Contains(x));
            foreach (var vv in ddd)
            {
                var cc = bigList.Where(x => x.Equals(vv));
                _lottoryNumMessage.DebugMessage.MenuName += $"[{vv.ToString("00")}] ";
            }
            _lottoryNumMessage.DebugMessage.MenuName += "\n";
            foreach (var vv in ddd)
            {
                var cc = bigList.Where(x => x.Equals(vv));
                _lottoryNumMessage.DebugMessage.MenuName += $"[{cc.Count().ToString("00")}] ";
            }
            _lottoryNumMessage.DebugMessage.MenuName += "\n";
            string infoPath = $"{Directory.GetCurrentDirectory()}\\LottoryInfo";
            if (!Directory.Exists(infoPath))
            {
                Directory.CreateDirectory(infoPath);
            }
            File.WriteAllText($"{infoPath}\\Lottory{DateTime.Now.ToString("yyyyMMddHHmm")}.txt", _lottoryNumMessage.DebugMessage.MenuName);
            //newHis = GetNewHistory(newHis.Count);
            //GetRecentNewNumbers(newHis);
            //newHis = GetNewHistory(newHis.Count);
            //GetRecentNewNumbers(newHis);
            //newHis = GetNewHistory(newHis.Count);
            //GetRecentNewNumbers(newHis);

            
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

        private void GetLottoryHistory(string dataDir)
        {
            List<LottoryInfo> rawData = new List<LottoryInfo>();
            var dataFiles = Directory.GetFiles(dataDir);

            foreach (var dataPath in dataFiles)
            {
                rawData.AddRange(GetLottoryData(dataPath));
            }

            lottoryHistory.AddRange(rawData.OrderByDescending(x => x.Date).ToList());
            _lottoryNumMessage.DebugMessage.MenuName += $"Files {dataFiles.Length} History Length {lottoryHistory.Count}\n";
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
        private void GetNewNumber(Dictionary<int, double> allTable)
        {
            var ddd = allTable.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value).Keys.ToList().GetRange(43, 6);
            resultNumbers.Add(hisCount, ddd);
            _lottoryNumMessage.DebugMessage.MenuName += "New Number \n";
            foreach (var vv in ddd)
            {
                _lottoryNumMessage.DebugMessage.MenuName += $"{vv.ToString()} ";
            }
            _lottoryNumMessage.DebugMessage.MenuName += "\n";
        }
        private List<LottoryInfo> GetNewHistory(int count)
        {
            int cnt = GetHistoryCount(_lottoryNumMessage, count);
            hisCount = cnt;
            return lottoryHistory.GetRange(0, cnt);
        }

     

        private int GetHistoryCount(DebugControlModel _Message, int count)
        {
            if (count % 2 != 0)
            {
                count -= 1;
            }
            int rev = count / 2;
            _Message.DebugMessage.MenuName += $"Get New History {rev}\n";
            return rev;
        }
#if false
        private void GetRecentNewNumbers(List<LottoryInfo> lottoryHistory)
        {
            var allTable = GetPacent(lottoryHistory);
            GetNewNumber(allTable);
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
                //_lottoryNumMessage.DebugMessage.MenuName += $"\n number {i} count {num1Cnt.Count} pacent {pacent.ToString("0.00")}%";
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
            _lottoryNumMessage.DebugMessage.MenuName += $"\n {tt}\n";
        }

        private Dictionary<int, Dictionary<int, double>> GetOpenPercent(List<LottoryInfo> newLott)
        {
            Dictionary<int, Dictionary<int, double>> openDic = new Dictionary<int, Dictionary<int, double>>();
            double pt = 0;
            for (int i = 0; i < 7; i++)
            {
                int allb = 0;
                openDic.Add(i, new Dictionary<int, double>());
                for (int j = 1; j < 50; j++)
                {
                    List<LottoryInfo> oo1 = new List<LottoryInfo>();
                    if (i < 6)
                    {
                        oo1.AddRange(newLott.Where(x => x.LottoryNumbers[i].Equals(j)));
                    }
                    else
                    {
                        oo1.AddRange(newLott.Where(x => x.SpecialNumber.Equals(j)));
                    }
                    pt = (double)oo1.Count() / (double)newLott.Count * 100;
                    openDic[i].Add(j, pt);
                    allb += oo1.Count();
                }
                //_lottoryOpenMessage.DebugMessage.MenuName += $"{i + 1}, Total {allb}\n";
            }
            Dictionary<int, Dictionary<int, double>> openDicOrder = new Dictionary<int, Dictionary<int, double>>();
            foreach (var ee in openDic)
            {
                var gg = ee.Value.OrderByDescending(x => x.Value).ToDictionary(y => y.Key, y => y.Value);
                openDicOrder.Add(ee.Key, gg);
            }
            return openDicOrder;
        }

        private void GetNewOpenNumber(Dictionary<int, Dictionary<int, double>> openDicOrder)
        {
            _lottoryOpenMessage.DebugMessage.MenuName += $"New Open Numbers \n";
            GetFinalNewOpenNum(true, openDicOrder);
            //GetFinalNewOpenNum(false, openDicOrder);

        }

        private void GetFinalNewOpenNum(bool v, Dictionary<int, Dictionary<int, double>> openDicOrder)
        {
            List<int> newOpenList = new List<int>();
            string numPrefix = $"Hight ";
            int newNN = 0;
            for (int i = 0; i < openDicOrder.Count; i++)
            {
                if (i > 0)
                {
                    foreach (var nn in newOpenList)
                    {
                        openDicOrder[i].Remove(nn);
                    }
                }
                if (v)
                {
                    newNN = openDicOrder[i].First().Key;
                }
                else
                {
                    newNN = openDicOrder[i].Last().Key;
                }
                newOpenList.Add(newNN);
            }
            if (!v) numPrefix = $"Lowest ";

            _lottoryOpenMessage.DebugMessage.MenuName += numPrefix;
            foreach (var ii in newOpenList)
            {
                _lottoryOpenMessage.DebugMessage.MenuName += $"{ii},";
            }
            _lottoryOpenMessage.DebugMessage.MenuName += $"\n";
        }
#endif
        #endregion
    }

}
