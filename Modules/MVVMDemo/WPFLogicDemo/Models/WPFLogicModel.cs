using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UtilityUILib;

namespace WPFLogicDemo.Models
{
    class WPFLogicModel : IWPFLogicModel
    {
        public ResourceDictionary GetLocalStyle()
        {
            return new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/WPFLogicDemo;component/Styles/WPFLogicStyle.xaml", UriKind.RelativeOrAbsolute)
            };

        }

        public void SetAsyncAwaitCooRun(IMenuItem messageText)
        {
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Coo start Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
            StartTask3(messageText);
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Coo end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
        }

        public void SetAsyncAwaitBooRun(IMenuItem messageText)
        {
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Boo start Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
            var result = StartTask2(messageText);
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Boo end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
        }

        public void SetAsyncAwaitAooRun(IMenuItem messageText)
        {
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Aoo start Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
            var result = StartTask1(messageText);
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Aoo end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
        }

        private async Task StartTask1(IMenuItem messageText)
        {
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Aoo StartTask1 start Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
            await Task.Run(async () => 
            {
                messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Aoo StartTask1 Delay GO Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
                await Task.Delay(10000);
                messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Aoo StartTask1 Delay Done Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
            });
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Aoo StartTask1 end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
        }

        private async Task StartTask2(IMenuItem messageText)
        {
#if false
            //This will A => B => C
            await StartForLoop(messageText, "Boo A");
            await StartForLoop(messageText, "Boo B");
            await StartForLoop(messageText, "Boo C");
#else
            //This is same as StartTask3
            var resu = StartForLoop(messageText, "Boo A");
            resu = StartForLoop(messageText, "Boo B");
            await StartForLoop(messageText, "Boo C");
#endif
        }

        private async Task StartForLoop(IMenuItem messageText, string funName)
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < 30; i++)
                {
                    Thread.Sleep(500);
                    messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] {funName} StartTask2 {i} end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
                }
            });
        }

        private void StartTask3(IMenuItem messageText)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 30; i++)
                {
                    Thread.Sleep(500);
                    messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Coo A StartTask3 {i} end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
                }
            });
            Task.Run(() =>
            {
                for (int i = 0; i < 30; i++)
                {
                    Thread.Sleep(500);
                    messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Coo B StartTask3 {i} end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
                }
            });
            Task.Run(() =>
            {
                for (int i = 0; i < 30; i++)
                {
                    Thread.Sleep(500);
                    messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Coo C StartTask3 {i} end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
                }
            });
        }
    }
}
