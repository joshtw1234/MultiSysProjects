using CentralModule.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CentralModule.Models
{
    class LottoryArgsClass
    {
        private static LottoryArgsClass _instance;
        public static LottoryArgsClass Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new LottoryArgsClass();
                }
                return _instance;
            }
        }
        const string PPFORMAT = "00.000000000";
        private LottoryArgsClass() { }
        private DebugControlModel _logMsgModel;
        private DebugControlModel _logOutMsgModel;
        Dictionary<int, double> dicAllTable = new Dictionary<int, double>();
        internal void LottoryArgsMethods(DebugControlModel logMsg, DebugControlModel logOutMsg, List<LottoryInfo> lottoryHistory, LottoryArgs args)
        {
            _logMsgModel = logMsg;
            _logOutMsgModel = logOutMsg;
            switch (args)
            {
                case LottoryArgs.Args1:
                    _logMsgModel.DebugMessage.MenuName += $"New Args here\n";
                    GetPacent(lottoryHistory);
                    Print5Lottory(lottoryHistory);
                    break;
            }
        }

        private void Print5Lottory(List<LottoryInfo> lottoryHistory)
        {
            const int DisplayCnt = 100;
            var r5Data = lottoryHistory.GetRange(0, DisplayCnt);
            StringBuilder sb = new StringBuilder();
            double min10cnt = 0, maxcnt = 0;
            foreach(var dd in r5Data)
            {
                var newdic = dicAllTable.Where(x => dd.LottoryNumbers.Contains(x.Key));
                var ornewDic = newdic.OrderBy(x => x.Value);
                string min10 = string.Empty;
                string max = string.Empty;
                if (ornewDic.First().Value < 11)
                {
                    min10 = "~";
                }
                if (ornewDic.Last().Value > 13)
                {
                    max = "^";
                }
                var c10 = ornewDic.Where(x => x.Value < 11);
                var c11 = ornewDic.Where(x => x.Value > 11 && x.Value < 12);
                var c12 = ornewDic.Where(x => x.Value > 12 && x.Value < 13);
                var c13 = ornewDic.Where(x => x.Value > 13);
                
                if (c11.Count() == 2 && c12.Count() == 2 && c13.Count() == 2)
                {
                    min10 += "!!!";
                }
               
                sb.AppendLine($"{min10} {dd.Date.ToString()} {max}");
                sb.AppendLine($"10 [{c10.Count()}] 11 [{c11.Count()}] 12 [{c12.Count()}] 13 [{c13.Count()}]");
                //dd.LottoryNumbers.Sort();
                //foreach(var dnum in dd.LottoryNumbers)
                //{
                //    var pp = dicAllTable[dnum];
                //    sb.Append($"[{dnum.ToString(PPFORMAT)}], ");
                //}
                //sb.Append(dd.SpecialNumber.ToString(PPFORMAT));
                //sb.AppendLine();
                //foreach (var dnum in dd.LottoryNumbers)
                //{
                //    var pp = dicAllTable[dnum];
                //    sb.Append($"[{pp.ToString(PPFORMAT)}], ");
                //}
                //sb.Append(dicAllTable[dd.SpecialNumber].ToString(PPFORMAT));
                //sb.AppendLine();
                //sb.AppendLine("====================================");

                foreach (var dnum in ornewDic)
                {
                    sb.Append($"[{dnum.Key.ToString(PPFORMAT)}], ");
                }
                sb.Append(dd.SpecialNumber.ToString(PPFORMAT));
                sb.AppendLine();
                foreach (var dnum in ornewDic)
                {
                    sb.Append($"[{dnum.Value.ToString(PPFORMAT)}], ");
                }
                sb.Append(dicAllTable[dd.SpecialNumber].ToString(PPFORMAT));
                sb.AppendLine();
               
            }
            //min10cnt = min10cnt / DisplayCnt * 100;
            //maxcnt = maxcnt / DisplayCnt * 100;
            //sb.AppendLine($"min {min10cnt.ToString(PPFORMAT)} max {maxcnt.ToString(PPFORMAT)}");
            _logOutMsgModel.DebugMessage.MenuName = sb.ToString();
        }

        private Dictionary<int, double> GetPacent(List<LottoryInfo> lottoryHistory)
        {
            Dictionary<int, double> dicTable = new Dictionary<int, double>();
            Dictionary<int, double> dic11Table = new Dictionary<int, double>();
            Dictionary<int, double> dic10Table = new Dictionary<int, double>();


            for (int i = 1; i < 50; i++)
            {
                var num1Cnt = lottoryHistory.Where(x => x.LottoryNumbers.Contains(i)).ToList();
                double pacent = (double)num1Cnt.Count / (double)lottoryHistory.Count * 100;
                dicAllTable.Add(i, pacent);
                ////_lottoryNumMessage.DebugMessage.MenuName += $"\n number {i} count {num1Cnt.Count} pacent {pacent.ToString("0.00")}%";
                //if (pacent < 12 && pacent > 11)
                //{
                //    dic11Table.Add(i, pacent);
                //}
                //else if (pacent < 11 && pacent > 10)
                //{
                //    dic10Table.Add(i, pacent);
                //}
                //else
                //{
                //    dicTable.Add(i, pacent);
                //}
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
            StringBuilder sb = new StringBuilder();
            foreach (var dd in ddd)
            {
                sb.AppendLine($" [{dd.Key.ToString("00")}] <{dd.Value.ToString(PPFORMAT)}>");
            }
            _logMsgModel.DebugMessage.MenuName += $"\n {tt}\n {sb.ToString()}";
        }
    }
}
