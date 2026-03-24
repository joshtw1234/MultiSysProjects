using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;   // ★ 已新增

namespace CPU_Bench_AI
{
    internal class Program
    {
        // 基準機分數 (假設一台參考機在 30 秒內跑出的迴圈數，實測後再調整)
        private const double BaselineSingleOpsPerSec        = 40_000_0000;   // 單執行緒 int
        private const double BaselineMultiOpsPerSec         = 120_000_0000;  // 多執行緒 int
        private const double BaselineFloatOpsPerSec         = 8_000_0000;    // 單執行緒 float
        private const double BaselineMultiFloatOpsPerSec    = 24_000_0000;   // 多執行緒 float (示意)

        // 測試時間 (秒)
        private const int BenchmarkSeconds = 30;

        // ★ P/Invoke 宣告，用來設定執行緒 CPU affinity
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentThread();

        [DllImport("kernel32.dll")]
        private static extern UIntPtr SetThreadAffinityMask(IntPtr hThread, UIntPtr dwThreadAffinityMask);

        static void Main(string[] args)
        {
            Console.WriteLine("=== CPU Benchmark (Timed) ===");
            Console.WriteLine(".NET Framework 4.7.2  |  C# 7.3");
            Console.WriteLine($"Each test runs {BenchmarkSeconds} seconds");
            Console.WriteLine();

            RunCpuBenchmarks();

            Console.WriteLine();
            Console.WriteLine("Benchmark finished. Press any key to exit.");
            Console.ReadKey();
        }

        /// <summary>
        /// 每一項測試都固定跑 BenchmarkSeconds 秒，
        /// 測量「每秒迴圈數」，再轉換成分數。
        /// </summary>
        private static void RunCpuBenchmarks()
        {
            Console.WriteLine("1) Integer tight loop - single thread (pinned to CPU core 0)");
            long singleOps;
            var singleTime = RunSingleThreadIntLoopTimed(BenchmarkSeconds, out singleOps);
            double singleOpsPerSec = singleOps / singleTime.TotalSeconds;
            Console.WriteLine($"   Time: {singleTime.TotalSeconds:F1} s, Ops: {singleOps}, Ops/s: {singleOpsPerSec:N0}");

            Console.WriteLine();
            Console.WriteLine("2) Integer tight loop - multi thread (Environment.ProcessorCount)");
            long multiOps;
            var multiTime = RunMultiThreadIntLoopTimed(BenchmarkSeconds, out multiOps);
            double multiOpsPerSec = multiOps / multiTime.TotalSeconds;
            Console.WriteLine($"   Time: {multiTime.TotalSeconds:F1} s, Ops: {multiOps}, Ops/s: {multiOpsPerSec:N0}");

            Console.WriteLine();
            Console.WriteLine("3) Floating-point math (sin/cos/sqrt) - single thread (pinned to CPU core 0)");
            long floatOps;
            var floatTime = RunFloatingPointTestTimed(BenchmarkSeconds, out floatOps);
            double floatOpsPerSec = floatOps / floatTime.TotalSeconds;
            Console.WriteLine($"   Time: {floatTime.TotalSeconds:F1} s, Ops: {floatOps}, Ops/s: {floatOpsPerSec:N0}");

            Console.WriteLine();
            Console.WriteLine("4) Floating-point math (sin/cos/sqrt) - multi thread (all cores)");
            long multiFloatOps;
            var multiFloatTime = RunMultiThreadFloatingPointTestTimed(BenchmarkSeconds, out multiFloatOps);
            double multiFloatOpsPerSec = multiFloatOps / multiFloatTime.TotalSeconds;
            Console.WriteLine($"   Time: {multiFloatTime.TotalSeconds:F1} s, Ops: {multiFloatOps}, Ops/s: {multiFloatOpsPerSec:N0}");

            // ===== 分數計算區 =====
            Console.WriteLine();
            Console.WriteLine("=== Scores (higher is better, 1000 = baseline) ===");

            double singleScore     = CalcScoreFromThroughput(singleOpsPerSec,     BaselineSingleOpsPerSec);
            double multiScore      = CalcScoreFromThroughput(multiOpsPerSec,      BaselineMultiOpsPerSec);
            double floatScore      = CalcScoreFromThroughput(floatOpsPerSec,      BaselineFloatOpsPerSec);
            double multiFloatScore = CalcScoreFromThroughput(multiFloatOpsPerSec, BaselineMultiFloatOpsPerSec);

            Console.WriteLine($"Single-thread Int Score        : {singleScore:F1}");
            Console.WriteLine($"Multi-thread  Int Score        : {multiScore:F1}");
            Console.WriteLine($"Single-thread Float Score      : {floatScore:F1}");
            Console.WriteLine($"Multi-thread  Float Score      : {multiFloatScore:F1}");

            // 總分權重可依需求調整
            double totalScore =
                singleScore     * 0.20 +
                multiScore      * 0.35 +
                floatScore      * 0.15 +
                multiFloatScore * 0.30;

            Console.WriteLine("---------------------------------------");
            Console.WriteLine($"Overall CPU Score              : {totalScore:F1}");
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

                    // 如需每 core pin，可在這裡加 SetThreadAffinityMask(1UL << localIndex)

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
    }
}
