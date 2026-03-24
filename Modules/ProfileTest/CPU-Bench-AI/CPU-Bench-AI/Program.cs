using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CPU_Bench_AI
{
    internal class Program
    {
        // 基準機分數 (假設一台參考機在指定秒數內跑出的迴圈數，實測後再調整)
        private const double BaselineSingleOpsPerSec        = 40_000_0000;   // 單執行緒 int
        private const double BaselineMultiOpsPerSec         = 120_000_0000;  // 多執行緒 int
        private const double BaselineFloatOpsPerSec         = 8_000_0000;    // 單執行緒 float
        private const double BaselineMultiFloatOpsPerSec    = 24_000_0000;   // 多執行緒 float
        private const double BaselineMemoryReadMBPerSec     = 5000.0;        // 記憶體讀取 MB/s (示意)
        private const double BaselineMemoryWriteMBPerSec    = 4000.0;        // 記憶體寫入 MB/s (示意)
        private const double BaselineMemoryMTPerSec         = 2000.0;        // 記憶體隨機 MT/s (示意)
        private const double BaselineMemorySeqMTPerSec      = 8000.0;        // 記憶體循序 MT/s (示意)

        // 測試時間配置
        private const int SingleThreadIntSeconds   = 5;    // 單執行緒整數測試
        private const int MultiThreadIntSeconds    = 5;    // 多執行緒整數測試
        private const int SingleThreadFloatSeconds = 5;    // 單執行緒浮點測試
        private const int MultiThreadFloatSeconds  = 5;    // 多執行緒浮點測試
        private const int MemoryReadSeconds        = 3;    // 記憶體讀取測試
        private const int MemoryWriteSeconds       = 3;    // 記憶體寫入測試
        private const int MemoryMTSeconds          = 4;    // 記憶體 MT/s 測試
        private const int MemorySeqMTSeconds       = 3;    // 記憶體循序 MT/s 測試
        // 總計: 5 + 5 + 5 + 5 + 3 + 3 + 4 + 3 = 30 秒

        // 記憶體測試參數
        private const int MemoryTestArraySizeMB = 128;     // 測試用陣列大小 (MB)

        // ★ P/Invoke 宣告，用來設定執行緒 CPU affinity
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentThread();

        [DllImport("kernel32.dll")]
        private static extern UIntPtr SetThreadAffinityMask(IntPtr hThread, UIntPtr dwThreadAffinityMask);

        static void Main(string[] args)
        {
            Console.WriteLine("=== CPU & Memory Benchmark (30-Second Total) ===");
            Console.WriteLine(".NET Framework 4.7.2  |  C# 7.3");
            Console.WriteLine("Total benchmark time: ~30 seconds");
            Console.WriteLine();

            var totalSw = Stopwatch.StartNew();
            RunCpuBenchmarks();
            totalSw.Stop();

            Console.WriteLine();
            Console.WriteLine($"Total elapsed time: {totalSw.Elapsed.TotalSeconds:F1} seconds");
            Console.WriteLine("Benchmark finished. Press any key to exit.");
            Console.ReadKey();
        }

        /// <summary>
        /// 每一項測試都固定跑指定秒數，
        /// 測量「每秒迴圈數」，再轉換成分數。
        /// </summary>
        private static void RunCpuBenchmarks()
        {
            Console.WriteLine($"1) Integer tight loop - single thread ({SingleThreadIntSeconds}s, pinned to CPU core 0)");
            long singleOps;
            var singleTime = RunSingleThreadIntLoopTimed(SingleThreadIntSeconds, out singleOps);
            double singleOpsPerSec = singleOps / singleTime.TotalSeconds;
            Console.WriteLine($"   Time: {singleTime.TotalSeconds:F1} s, Ops: {singleOps:N0}, Ops/s: {singleOpsPerSec:N0}");

            Console.WriteLine();
            Console.WriteLine($"2) Integer tight loop - multi thread ({MultiThreadIntSeconds}s, all cores)");
            long multiOps;
            var multiTime = RunMultiThreadIntLoopTimed(MultiThreadIntSeconds, out multiOps);
            double multiOpsPerSec = multiOps / multiTime.TotalSeconds;
            Console.WriteLine($"   Time: {multiTime.TotalSeconds:F1} s, Ops: {multiOps:N0}, Ops/s: {multiOpsPerSec:N0}");

            Console.WriteLine();
            Console.WriteLine($"3) Floating-point math - single thread ({SingleThreadFloatSeconds}s, pinned to CPU core 0)");
            long floatOps;
            var floatTime = RunFloatingPointTestTimed(SingleThreadFloatSeconds, out floatOps);
            double floatOpsPerSec = floatOps / floatTime.TotalSeconds;
            Console.WriteLine($"   Time: {floatTime.TotalSeconds:F1} s, Ops: {floatOps:N0}, Ops/s: {floatOpsPerSec:N0}");

            Console.WriteLine();
            Console.WriteLine($"4) Floating-point math - multi thread ({MultiThreadFloatSeconds}s, all cores)");
            long multiFloatOps;
            var multiFloatTime = RunMultiThreadFloatingPointTestTimed(MultiThreadFloatSeconds, out multiFloatOps);
            double multiFloatOpsPerSec = multiFloatOps / multiFloatTime.TotalSeconds;
            Console.WriteLine($"   Time: {multiFloatTime.TotalSeconds:F1} s, Ops: {multiFloatOps:N0}, Ops/s: {multiFloatOpsPerSec:N0}");

            Console.WriteLine();
            Console.WriteLine($"5) Memory sequential read ({MemoryReadSeconds}s, {MemoryTestArraySizeMB} MB buffer)");
            double memReadMBPerSec;
            var memReadTime = RunMemoryReadBenchmark(MemoryReadSeconds, out memReadMBPerSec);
            Console.WriteLine($"   Time: {memReadTime.TotalSeconds:F1} s, Speed: {memReadMBPerSec:F1} MB/s");

            Console.WriteLine();
            Console.WriteLine($"6) Memory sequential write ({MemoryWriteSeconds}s, {MemoryTestArraySizeMB} MB buffer)");
            double memWriteMBPerSec;
            var memWriteTime = RunMemoryWriteBenchmark(MemoryWriteSeconds, out memWriteMBPerSec);
            Console.WriteLine($"   Time: {memWriteTime.TotalSeconds:F1} s, Speed: {memWriteMBPerSec:F1} MB/s");

            Console.WriteLine();
            Console.WriteLine($"7) Memory random access ({MemoryMTSeconds}s, measuring MT/s)");
            double memMTPerSec;
            var memMTTime = RunMemoryMTBenchmark(MemoryMTSeconds, out memMTPerSec);
            Console.WriteLine($"   Time: {memMTTime.TotalSeconds:F1} s, Speed: {memMTPerSec:F1} MT/s");

            Console.WriteLine();
            Console.WriteLine($"8) Memory sequential access MT/s ({MemorySeqMTSeconds}s, measuring MT/s)");
            double memSeqMTPerSec;
            var memSeqMTTime = RunMemorySequentialMTBenchmark(MemorySeqMTSeconds, out memSeqMTPerSec);
            Console.WriteLine($"   Time: {memSeqMTTime.TotalSeconds:F1} s, Speed: {memSeqMTPerSec:F1} MT/s");

            // ===== 分數計算區 =====
            Console.WriteLine();
            Console.WriteLine("=== Scores (higher is better, 1000 = baseline) ===");

            double singleScore     = CalcScoreFromThroughput(singleOpsPerSec,     BaselineSingleOpsPerSec);
            double multiScore      = CalcScoreFromThroughput(multiOpsPerSec,      BaselineMultiOpsPerSec);
            double floatScore      = CalcScoreFromThroughput(floatOpsPerSec,      BaselineFloatOpsPerSec);
            double multiFloatScore = CalcScoreFromThroughput(multiFloatOpsPerSec, BaselineMultiFloatOpsPerSec);
            double memReadScore    = CalcScoreFromThroughput(memReadMBPerSec,     BaselineMemoryReadMBPerSec);
            double memWriteScore   = CalcScoreFromThroughput(memWriteMBPerSec,    BaselineMemoryWriteMBPerSec);
            double memMTScore      = CalcScoreFromThroughput(memMTPerSec,         BaselineMemoryMTPerSec);
            double memSeqMTScore   = CalcScoreFromThroughput(memSeqMTPerSec,      BaselineMemorySeqMTPerSec);

            Console.WriteLine($"Single-thread Int Score        : {singleScore:F1}");
            Console.WriteLine($"Multi-thread  Int Score        : {multiScore:F1}");
            Console.WriteLine($"Single-thread Float Score      : {floatScore:F1}");
            Console.WriteLine($"Multi-thread  Float Score      : {multiFloatScore:F1}");
            Console.WriteLine($"Memory Read Score              : {memReadScore:F1}");
            Console.WriteLine($"Memory Write Score             : {memWriteScore:F1}");
            Console.WriteLine($"Memory Random MT/s Score       : {memMTScore:F1}");
            Console.WriteLine($"Memory Sequential MT/s Score   : {memSeqMTScore:F1}");

            // 總分權重可依需求調整
            double totalScore =
                singleScore     * 0.13 +
                multiScore      * 0.22 +
                floatScore      * 0.09 +
                multiFloatScore * 0.18 +
                memReadScore    * 0.13 +
                memWriteScore   * 0.13 +
                memMTScore      * 0.12 +
                memSeqMTScore   * 0.11;

            Console.WriteLine("---------------------------------------");
            Console.WriteLine($"Overall System Score           : {totalScore:F1}");
        }

        /// <summary>
        /// 分數公式 (吞吐量版)：Score = 1000 * (ActualOpsPerSec / BaselineOpsPerSec)
        /// 跑得越多迴圈分數越高；BaselineOpsPerSec 為 1000 分。
        /// </summary>
        private static double CalcScoreFromThroughput(double actualOpsPerSec, double baselineOpsPerSec)
        {
            if (actualOpsPerSec <= 0 || baselineOpsPerSec <= 0)
                return 0;

            return 1000.0 * (actualOpsPerSec / baselineOpsPerSec);
        }

        private static TimeSpan RunSingleThreadIntLoopTimed(int seconds, out long iterationsDone)
        {
            // ★ 單執行緒 int：綁定 core0
            IntPtr threadHandle = GetCurrentThread();
            UIntPtr previousMask = SetThreadAffinityMask(threadHandle, new UIntPtr(0x1));

            var sw = Stopwatch.StartNew();
            long acc = 0;
            long i = 0;

            while (sw.Elapsed.TotalSeconds < seconds)
            {
                acc += (i * 3) ^ (i >> 1);
                i++;
            }

            sw.Stop();
            iterationsDone = i;

            if (previousMask != UIntPtr.Zero)
            {
                SetThreadAffinityMask(threadHandle, previousMask);
            }

            Console.WriteLine($"   Accumulator (single): {acc}");
            return sw.Elapsed;
        }

        private static TimeSpan RunMultiThreadIntLoopTimed(int seconds, out long iterationsDone)
        {
            int logicalCores = Environment.ProcessorCount;
            var sw = Stopwatch.StartNew();

            long[] accs = new long[logicalCores];
            long[] counts = new long[logicalCores];

            Task[] tasks = new Task[logicalCores];
            for (int t = 0; t < logicalCores; t++)
            {
                int localIndex = t;
                tasks[localIndex] = Task.Factory.StartNew(() =>
                {
                    long localAcc = 0;
                    long localCount = 0;

                    while (sw.Elapsed.TotalSeconds < seconds)
                    {
                        long i = localCount;
                        localAcc += (i * 3) ^ (i >> 1);
                        localCount++;
                    }

                    accs[localIndex] = localAcc;
                    counts[localIndex] = localCount;
                }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }

            Task.WaitAll(tasks);
            sw.Stop();

            long totalAcc = 0;
            long totalCount = 0;
            for (int t = 0; t < logicalCores; t++)
            {
                totalAcc ^= accs[t];
                totalCount += counts[t];
            }

            iterationsDone = totalCount;

            Console.WriteLine($"   Accumulator (multi): {totalAcc}");
            Console.WriteLine($"   Logical cores used:  {logicalCores}");
            return sw.Elapsed;
        }

        private static TimeSpan RunFloatingPointTestTimed(int seconds, out long iterationsDone)
        {
            // ★ 單執行緒 float：綁定 core0
            IntPtr threadHandle = GetCurrentThread();
            UIntPtr previousMask = SetThreadAffinityMask(threadHandle, new UIntPtr(0x1));

            var sw = Stopwatch.StartNew();
            double acc = 0.0;
            long i = 1;

            while (sw.Elapsed.TotalSeconds < seconds)
            {
                double x = i * 0.000_001;
                acc += Math.Sin(x) * Math.Cos(x) + Math.Sqrt(x);
                i++;
            }

            sw.Stop();
            iterationsDone = i - 1;

            if (previousMask != UIntPtr.Zero)
            {
                SetThreadAffinityMask(threadHandle, previousMask);
            }

            Console.WriteLine($"   Accumulator (float): {acc:F4}");
            return sw.Elapsed;
        }

        /// <summary>
        /// 多執行緒浮點測試：所有核心一起跑 sin/cos/sqrt。
        /// </summary>
        private static TimeSpan RunMultiThreadFloatingPointTestTimed(int seconds, out long iterationsDone)
        {
            int logicalCores = Environment.ProcessorCount;
            var sw = Stopwatch.StartNew();

            double[] accs = new double[logicalCores];
            long[] counts = new long[logicalCores];

            Task[] tasks = new Task[logicalCores];
            for (int t = 0; t < logicalCores; t++)
            {
                int localIndex = t;
                tasks[localIndex] = Task.Factory.StartNew(() =>
                {
                    double localAcc = 0.0;
                    long i = 1;

                    while (sw.Elapsed.TotalSeconds < seconds)
                    {
                        double x = i * 0.000_001;
                        localAcc += Math.Sin(x) * Math.Cos(x) + Math.Sqrt(x);
                        i++;
                    }

                    accs[localIndex] = localAcc;
                    counts[localIndex] = i - 1;
                }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }

            Task.WaitAll(tasks);
            sw.Stop();

            double totalAcc = 0.0;
            long totalCount = 0;
            for (int t = 0; t < logicalCores; t++)
            {
                totalAcc += accs[t];
                totalCount += counts[t];
            }

            iterationsDone = totalCount;

            Console.WriteLine($"   Accumulator (multi-floatsum): {totalAcc:F4}");
            Console.WriteLine($"   Logical cores used (float):    {logicalCores}");
            return sw.Elapsed;
        }

        /// <summary>
        /// 記憶體循序讀取測試：反覆讀取大陣列，計算 MB/s。
        /// </summary>
        private static TimeSpan RunMemoryReadBenchmark(int seconds, out double mbPerSecond)
        {
            // 建立測試陣列 (long 型別，8 bytes per element)
            int arrayLength = (MemoryTestArraySizeMB * 1024 * 1024) / 8;
            long[] buffer = new long[arrayLength];

            // 初始化陣列
            for (int i = 0; i < arrayLength; i++)
            {
                buffer[i] = i;
            }

            var sw = Stopwatch.StartNew();
            long totalBytesRead = 0;
            long checksum = 0;

            while (sw.Elapsed.TotalSeconds < seconds)
            {
                // 循序讀取整個陣列
                for (int i = 0; i < arrayLength; i++)
                {
                    checksum += buffer[i];
                }
                totalBytesRead += arrayLength * 8;
            }

            sw.Stop();

            double totalMB = totalBytesRead / (1024.0 * 1024.0);
            mbPerSecond = totalMB / sw.Elapsed.TotalSeconds;

            Console.WriteLine($"   Total bytes read: {totalBytesRead:N0}, Checksum: {checksum}");
            return sw.Elapsed;
        }

        /// <summary>
        /// 記憶體循序寫入測試：反覆寫入大陣列，計算 MB/s。
        /// </summary>
        private static TimeSpan RunMemoryWriteBenchmark(int seconds, out double mbPerSecond)
        {
            // 建立測試陣列 (long 型別，8 bytes per element)
            int arrayLength = (MemoryTestArraySizeMB * 1024 * 1024) / 8;
            long[] buffer = new long[arrayLength];
                
            var sw = Stopwatch.StartNew();
            long totalBytesWritten = 0;
            long value = 0;

            while (sw.Elapsed.TotalSeconds < seconds)
            {
                // 循序寫入整個陣列
                for (int i = 0; i < arrayLength; i++)
                {
                    buffer[i] = value++;
                }
                totalBytesWritten += arrayLength * 8;
            }

            sw.Stop();

            double totalMB = totalBytesWritten / (1024.0 * 1024.0);
            mbPerSecond = totalMB / sw.Elapsed.TotalSeconds;

            // 讀取最後一個值避免編譯器優化掉整個迴圈
            long finalValue = buffer[arrayLength - 1];
            Console.WriteLine($"   Total bytes written: {totalBytesWritten:N0}, Final value: {finalValue}");
            return sw.Elapsed;
        }

        /// <summary>
        /// 記憶體隨機存取測試：測量記憶體傳輸速度 (MT/s - Megatransfers per second)。
        /// 使用隨機存取模式來測試記憶體延遲與頻寬的綜合性能。
        /// </summary>
        private static TimeSpan RunMemoryMTBenchmark(int seconds, out double mtPerSecond)
        {
            // 建立測試陣列 (long 型別，8 bytes per element)
            int arrayLength = (MemoryTestArraySizeMB * 1024 * 1024) / 8;
            long[] buffer = new long[arrayLength];
            
            // 建立隨機存取索引陣列（pointer chasing pattern）
            int[] indices = new int[arrayLength];
            Random rnd = new Random(12345); // 固定種子確保可重複性
            
            // 初始化隨機存取模式
            for (int i = 0; i < arrayLength; i++)
            {
                indices[i] = i;
            }
            
            // Fisher-Yates shuffle 打亂索引
            for (int i = arrayLength - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                int temp = indices[i];
                indices[i] = indices[j];
                indices[j] = temp;
            }
            
            // 初始化 buffer
            for (int i = 0; i < arrayLength; i++)
            {
                buffer[i] = i;
            }

            var sw = Stopwatch.StartNew();
            long totalTransfers = 0;
            long checksum = 0;

            while (sw.Elapsed.TotalSeconds < seconds)
            {
                // 隨機存取模式 - 測試記憶體延遲
                for (int i = 0; i < arrayLength; i++)
                {
                    int idx = indices[i];
                    checksum += buffer[idx];
                    totalTransfers++;
                }
            }

            sw.Stop();

            // MT/s = 百萬次傳輸/秒
            double totalMT = totalTransfers / 1_000_000.0;
            mtPerSecond = totalMT / sw.Elapsed.TotalSeconds;

            Console.WriteLine($"   Total transfers: {totalTransfers:N0}, Checksum: {checksum} MT/s");
            return sw.Elapsed;
        }

        /// <summary>
        /// 記憶體循序存取測試：測量記憶體傳輸速度 (MT/s - Megatransfers per second)。
        /// 使用循序存取模式來測試記憶體頻寬的最大性能。
        /// 優化版：展開迴圈、使用 16 個累加器減少依賴鏈、批次計時。
        /// </summary>
        private static TimeSpan RunMemorySequentialMTBenchmark(int seconds, out double mtPerSecond)
        {
            // 建立測試陣列 (long 型別，8 bytes per element)
            int arrayLength = (MemoryTestArraySizeMB * 1024 * 1024) / 8;
            long[] buffer = new long[arrayLength];
            
            // 初始化 buffer
            for (int i = 0; i < arrayLength; i++)
            {
                buffer[i] = i;
            }

            var sw = Stopwatch.StartNew();
            long totalTransfers = 0;
            
            // 使用 16 個累加器打破依賴鏈，提升指令級平行度
            long checksum0  = 0, checksum1  = 0, checksum2  = 0, checksum3  = 0;
            long checksum4  = 0, checksum5  = 0, checksum6  = 0, checksum7  = 0;
            long checksum8  = 0, checksum9  = 0, checksum10 = 0, checksum11 = 0;
            long checksum12 = 0, checksum13 = 0, checksum14 = 0, checksum15 = 0;

            // 減少時間檢查頻率
            int passesPerCheck = 10; // 每跑 10 次陣列才檢查一次時間
            int passCount = 0;

            while (true)
            {
                // 手動展開迴圈，每次處理 16 個元素（128 bytes = 2 cache lines）
                for (int i = 0; i < arrayLength; i += 16)
                {
                    // 使用不同的累加器避免依賴鏈，允許 CPU 亂序執行和平行處理
                    checksum0  += buffer[i];
                    checksum1  += buffer[i + 1];
                    checksum2  += buffer[i + 2];
                    checksum3  += buffer[i + 3];
                    checksum4  += buffer[i + 4];
                    checksum5  += buffer[i + 5];
                    checksum6  += buffer[i + 6];
                    checksum7  += buffer[i + 7];
                    checksum8  += buffer[i + 8];
                    checksum9  += buffer[i + 9];
                    checksum10 += buffer[i + 10];
                    checksum11 += buffer[i + 11];
                    checksum12 += buffer[i + 12];
                    checksum13 += buffer[i + 13];
                    checksum14 += buffer[i + 14];
                    checksum15 += buffer[i + 15];
                }
                
                totalTransfers += arrayLength;
                passCount++;

                // 每跑幾次才檢查時間，減少檢查開銷
                if (passCount >= passesPerCheck)
                {
                    passCount = 0;
                    if (sw.Elapsed.TotalSeconds >= seconds)
                        break;
                }
            }

            sw.Stop();

            // 合併所有 16 個累加器
            long checksum = checksum0  + checksum1  + checksum2  + checksum3  + 
                   checksum4  + checksum5  + checksum6  + checksum7  +
                   checksum8  + checksum9  + checksum10 + checksum11 +
                   checksum12 + checksum13 + checksum14 + checksum15;

            // MT/s = 百萬次傳輸/秒
            double totalMT = totalTransfers / 1_000_000.0;
            mtPerSecond = totalMT / sw.Elapsed.TotalSeconds;

            Console.WriteLine($"   Total transfers: {totalTransfers:N0}, Checksum: {checksum}");
            return sw.Elapsed;
        }
    }
}
