using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace ProfileTest
{
    /// <summary>
    /// Interaction logic for UCGAMEBASE.xaml
    /// </summary>
    public partial class UCGAMEBASE : UserControl
    {
        string[] pgProperity = { "DisplayName", "DisplayVersion", "Publisher", "Version",
                                 "InstallDate","InstallLocation","InstallSource", "DisplayIcon"};
        const string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        const string swInstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData\S-1-5-18\Products";
        const string pgkey = "InstallProperties";
        string strLinkProcName = string.Empty;
        bool isChecking = false, isCheckLink = true, isUCLoaded = false;
        Thread thCheckProc;
        public ObservableCollection<ProgramsData> pgList { get; set; }
        public UCGAMEBASE()
        {
            InitializeComponent();
            this.Loaded += UCGAMEBASE_Loaded;
            pgList = new ObservableCollection<ProgramsData>();
            DataContext = this;
        }

        private void UCGAMEBASE_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isUCLoaded)
            {
                Window wind = Window.GetWindow(this);
                wind.Closing += Wind_Closing;
                isUCLoaded = true;
            }
        }

        private void Wind_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isCheckLink = false;
        }

        System.Windows.Media.ImageSource IconToImageSource(System.Drawing.Icon icon)
        {
            return Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                new Int32Rect(0, 0, icon.Width, icon.Height),
                BitmapSizeOptions.FromEmptyOptions());
        }

        void GetDataFromReg(string firstKey, string extendKey, int searIdx)
        {
            List<ProgramsData> tmpLst = new List<ProgramsData>();
            ProgramsData pgData = null;
            object regObj;

            RegistryKey rk = null;
            System.Text.RegularExpressions.Match mt;
            switch (searIdx)
            {
                case 0:
                    rk = Registry.LocalMachine.OpenSubKey(firstKey);
                    break;
                case 1:
                    rk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(firstKey);
                    break;
                default:
                    return;
            }

            foreach (string skName in rk.GetSubKeyNames())
            {
                using (RegistryKey sk = rk.OpenSubKey(skName))
                {
                    if (sk == null)
                    {
                        continue;
                    }
                    pgData = new ProgramsData();

                    regObj = sk.GetValue(pgProperity[0]);
                    if (regObj != null)
                    {
                        pgData.DisplayName = regObj.ToString();
                    }
                    regObj = sk.GetValue(pgProperity[1]);
                    if (regObj != null)
                    {
                        pgData.DisplayVersion = regObj.ToString();
                    }
                    regObj = sk.GetValue(pgProperity[2]);
                    if (regObj != null)
                    {
                        pgData.Publisher = regObj.ToString();
                    }
                    regObj = sk.GetValue(pgProperity[3]);
                    if (regObj != null)
                    {
                        pgData.Version = regObj.ToString();
                    }
                    regObj = sk.GetValue(pgProperity[4]);
                    if (regObj != null)
                    {
                        pgData.InstallDate = regObj.ToString();
                    }
                    regObj = sk.GetValue(pgProperity[5]);
                    if (regObj != null)
                    {
                        pgData.InstallLocation = regObj.ToString();
                    }
                    regObj = sk.GetValue(pgProperity[6]);
                    if (regObj != null)
                    {
                        pgData.InstallSource = regObj.ToString();
                    }
                    regObj = sk.GetValue(pgProperity[7]);
                    if (regObj != null)
                    {
                        string iconPath = regObj.ToString(), strPat = @"([C|c][ -\{\}\w\\:]+.exe)([\W\w]*)";

                        if (!string.IsNullOrEmpty(iconPath))
                        {
                            mt = System.Text.RegularExpressions.Regex.Match(iconPath, strPat);
                            if (mt.Success && System.IO.File.Exists(mt.Groups[1].Value))
                            {

                                pgData.DisplayIcon = IconToImageSource(System.Drawing.Icon.ExtractAssociatedIcon(mt.Groups[1].Value));
                                if (string.IsNullOrEmpty(pgData.InstallLocation))
                                {
                                    FileInfo fi = new FileInfo(mt.Groups[1].Value);
                                    pgData.InstallLocation = fi.Directory.FullName;
                                }
                                //pgList.Add(pgData);
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(pgData.DisplayName))
                {
                    if (null == tmpLst.Find(x => x.DisplayName.Equals(pgData.DisplayName) && x.DisplayVersion.Equals(pgData.DisplayVersion)))
                    {
                        tmpLst.Add(pgData);
                    }
                    //pgList.Add(pgData);
                }
            }

            foreach (ProgramsData sPgData in tmpLst)
            {
                pgList.Add(sPgData);
            }

        }

        void GetProgramsList()
        {
            pgList.Clear();
            GetDataFromReg(uninstallKey, string.Empty, 0);
            GetDataFromReg(uninstallKey, string.Empty, 1);
            //Search in OS version.

        }


        void CheckLinkProcess()
        {
            bool isShowGotMsg = false, isShowNoMsg = false, isGotProc = false;
            string strGot = string.Empty;
            while (isCheckLink)
            {
                Thread.Sleep(1000);
                Process[] processlist = Process.GetProcesses();

                foreach (Process theprocess in processlist)
                {
                    if (!isGotProc)
                    {
                        if (strLinkProcName.Equals(theprocess.ProcessName))
                        {
                            if (!isShowGotMsg)
                            {
                                //MessageBox.Show(string.Format("{0} is runing", strLinkProcName));
                                this.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    txtMsg.Text = string.Format("{0} is runing", strLinkProcName);
                                    MaskofMonitor.Visibility = Visibility.Visible;

                                }));
                                isGotProc = !isGotProc;
                                isShowGotMsg = !isShowGotMsg;
                                if (isShowNoMsg)
                                {
                                    isShowNoMsg = !isShowNoMsg;
                                }
                                strGot = strLinkProcName;
                                break;
                            }
                        }
                    }
                    else
                    {

                        strGot = string.Empty;
                        if (strLinkProcName.Equals(theprocess.ProcessName))
                        {
                            strGot = strLinkProcName;
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(strGot))
                {
                    if (isShowGotMsg)
                    {
                        isShowGotMsg = !isShowGotMsg;
                    }
                    if (isGotProc)
                    {
                        isGotProc = !isGotProc;
                    }
                    if (!isShowNoMsg)
                    {
                        //MessageBox.Show(string.Format("{0} is not runing", strLinkProcName));
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            MaskofMonitor.Visibility = Visibility.Visible;
                            txtMsg.Text = string.Format("{0} is not runing", strLinkProcName);
                        }));
                        isShowNoMsg = !isShowNoMsg;

                    }
                }
            }
        }
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "btnGetList":
                    GetProgramsList();
                    break;
                case "btnSetLink":
                    ProgramsData pgData = dgPrograms.SelectedItem as ProgramsData;
                    if (pgData != null && !string.IsNullOrEmpty(pgData.InstallLocation))
                    {
                        // Create OpenFileDialog 
                        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                        dlg.InitialDirectory = pgData.InstallLocation;
                        // Set filter for file extension and default file extension 
                        dlg.DefaultExt = ".exe";
                        //dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
                        dlg.Filter = "Exe File (*.exe)|*.exe|All Files (*.*)|*.*";


                        // Display OpenFileDialog by calling ShowDialog method 
                        Nullable<bool> result = dlg.ShowDialog();


                        // Get the selected file name and display in a TextBox 
                        if (result == true)
                        {
                            // Open document 
                            FileInfo fi = new FileInfo(dlg.FileName);
                            strLinkProcName = Path.GetFileNameWithoutExtension(fi.Name);
                            //textBox1.Text = filename;
                        }
                    }
                    break;
                case "btnStartMonitor":
                    if (!isChecking)
                    {
                        if (string.IsNullOrEmpty(strLinkProcName))
                        {
                            MessageBox.Show("Plsase Setup Link first!!");
                        }
                        else
                        {
                            isCheckLink = true;
                            btnStartMonitor.Content = "STOP Monitor";
                            isChecking = !isChecking;
                            thCheckProc = new Thread(CheckLinkProcess);
                            thCheckProc.Priority = ThreadPriority.Normal;
                            thCheckProc.Start();
                        }
                    }
                    else
                    {
                        isCheckLink = false;
                        btnStartMonitor.Content = "Start Monitor";
                        isChecking = !isChecking;

                        //thCheckProc.Abort();
                    }
                    break;
                case "btnDismiss":
                    MaskofMonitor.Visibility = Visibility.Collapsed;
                    break;
            }
        }

    }
}
