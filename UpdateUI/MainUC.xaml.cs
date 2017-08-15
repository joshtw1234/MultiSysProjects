using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UpdateUI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MainControl : UserControl, UtilityUILib.IModuleInterface
    {
        const string regUpdatePath = "SOFTWARE\\HP\\OMEN Ally\\Settings\\Updates";
        const string regSettingsPath = "SOFTWARE\\HP\\OMEN Ally\\Settings";
        const string PIPENAME = "\\\\.\\pipe\\MyUpdate";
        public static PipeClientUpdate pipeClient;
        public static CultureInfo inSideculInfo = new CultureInfo("en-US");

        public MainControl()
        {
            InitializeComponent();
            this.Loaded += UserControl1_Loaded;
        }

        private void UserControl1_Loaded(object sender, RoutedEventArgs e)
        {
            string ErrMsg = "HPSA no Install\n";
            if (CheckUpdatReg(regUpdatePath, "HPSAInstalled"))
            {
                ErrMsg = "HPSA installed\n";
                txtUIMessage.Text = "Congrats!You Already have the latest version.";
                SetBtnDownloadVisibility(false);
               
            }
            else
            {
                txtUIMessage.Text = "HPSA not installed";
                SetBtnDownloadVisibility(true);
            }
            txtMsg.AppendText(ErrMsg);
            ErrMsg = "HPSA service is not runing\n";
            if (CheckUpdatReg(regUpdatePath, "HPSARunning"))
            {
                ErrMsg = "HPSA service runing\n";
            }
            txtMsg.AppendText(ErrMsg);
            //ErrMsg = "Network connected\n";
            //if (!CheckUpdatReg(regSettingsPath, "NetworkConnection"))
            //{
            //    ErrMsg = "Network not connected\n";
            //    txtUIMessage.Text = "You have no Network connected. Please connected and try again.";
            //    SetBtnDownloadVisibility(false);
            //}
            //txtMsg.AppendText(ErrMsg);
           
            pipeClient = new PipeClientUpdate(PIPENAME);
            pipeClient.SetBufferSize(512);
            ErrMsg = "Pipe not connect\n";
            if (pipeClient.Connect())
            {
                ErrMsg = "Pipe connected\n";
                pipeClient.DataReceived += PipeClient_DataReceived;
                pipeClient.GetIsHPSARunning();
            }
            txtMsg.AppendText(ErrMsg);
        }

        private void PipeClient_DataReceived(object sender, PipeClient.PipeData rc)
        {
            string revStr = string.Empty;
            string[] revAry = null;
            revStr = Encoding.ASCII.GetString(rc.Data).Replace("\0", string.Empty);
            revAry = revStr.Split(',');
            btnChkForUpdate.Content = "Check for Update";
            switch ((CommandID)rc.commandId)
            {
                case CommandID.COMMAND_X:
                    break;
                case CommandID.COMMAND_GetOMENUpdate:
                    
                    switch (Convert.ToInt32(revAry[0]))
                    {
                        case 0:
                            //HPSA is running
                            txtUIMessage.Text = string.Format("HPSA is running");
                            if (btnChkForUpdate.IsEnabled)
                            {
                                btnChkForUpdate.IsEnabled = false;
                            }
                            break;
                        case 1:
                            //OMEN has update
                            txtUIMessage.Text = string.Format("Looks like new Update available. Priority [{0}]", revAry[1]);
                            if (!btnChkForUpdate.IsEnabled)
                            {
                                btnChkForUpdate.Content = "Update";
                                btnChkForUpdate.Foreground = Brushes.Red;
                                btnChkForUpdate.IsEnabled = true;
                            }
                            break;
                        case 2:
                            //OMEN no Update
                            txtUIMessage.Text = "Congrats!You Already have the latest version.";
                            SetBtnDownloadVisibility(false);
                            if (!btnChkForUpdate.IsEnabled)
                            {
                                btnChkForUpdate.IsEnabled = true;
                            }
                            break;
                    }
                    break;
                case CommandID.COMMAND_IsHPSARunning:
                    switch (Convert.ToInt32(revAry[0]))
                    {
                        case 0:
                            //HPSA is not running
                            txtUIMessage.Text = "Congrats!You Already have the latest version.";
                            if (!btnChkForUpdate.IsEnabled)
                            {
                                btnChkForUpdate.IsEnabled = true;
                            }
                            break;
                        case 1:
                            //HPSA is running
                            txtUIMessage.Text = string.Format("Checking for updates...This could take a couple of minutes. You will be notified when it's ready");
                            if (btnChkForUpdate.IsEnabled)
                            {
                                btnChkForUpdate.IsEnabled = false;
                            }
                            break;
                    }
                    break;
                case CommandID.COMMAND_GetNetWork:
                    switch (Convert.ToInt32(revAry[0]))
                    {
                        case 1:
                            txtUIMessage.Foreground = Brushes.White;
                            txtUIMessage.Text = string.Format("Ready for check update"); ;
                            break;
                        default:
                            txtUIMessage.Foreground = Brushes.Red;
                            txtUIMessage.Text = string.Format("Looks like you are not connected to the internet. Please connect and try again.");
                            break;
                    }
                    break;
                //case CommandID.COMMAND_GetWMIValue:
                //    revStr = Encoding.ASCII.GetString(rc.Data).Replace("\0", string.Empty);
                //    //MainControl.Logger(string.Format("0 Get WMI {0}", revStr));
                //    ShowLog(string.Format("0 Get WMI {0}", revStr));
                //    revAry = revStr.Split('|');
                //    break;
            }
            ShowLog(string.Format("{0}::{1}", (CommandID)rc.commandId, revStr));
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch(btn.Name)
            {
                case "btnDownload":
                    Process.Start("http://www.hp.com/go/hpsupportassistant");
                    break;
                case "btnChkForUpdate":
                    if (!CheckUpdatReg(regSettingsPath, "NetworkConnection"))
                    {
                        txtUIMessage.Text = "You have no Network connected. Please connected and try again.";
                        SetBtnDownloadVisibility(false);
                    }
                    else
                    {
                        pipeClient.GetOMENUpdate();
                    }
                    break;
            }
        }
        private void btnGetDev_Click(object sender, RoutedEventArgs e)
        {
#if true
            // Get a toast XML template
            //XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);

            //// Fill in the text elements
            //XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
            //for (int i = 0; i < stringElements.Length; i++)
            //{
            //    stringElements[i].AppendChild(toastXml.CreateTextNode("Line " + i));
            //}

            //// Specify the absolute path to an image
            //String imagePath = "file:///" + Path.GetFullPath("toastImageAndText.png");
            //XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
            //imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

            ////// Create the toast and attach event listeners
            ////ToastNotification toast = new ToastNotification(toastXml);
            ////toast.Activated += ToastActivated;
            ////toast.Dismissed += ToastDismissed;
            ////toast.Failed += ToastFailed;

            //// Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
            //ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);
#else
            HP.SupportAssistant.HPSAUpdate ss = new HPSAUpdate();
            ss.

            //txtMsg.AppendText("New UPdate Class.\n");
            //hpUPdate = new AppUpdates();
            //txtMsg.AppendText("New UPdate Class Done.\n");
            //List<HPSAMessage> msgList = hpUPdate.GetMessagesList(null, null);
            //txtMsg.AppendText(string.Format("Get Message List Cnt {0}\n", msgList.Count));
            //List<HPSADevice> deviceList = hpUPdate.GetDevicesList();
            //txtMsg.AppendText(string.Format("Get Devise List Cnt {0}\n", deviceList.Count));

            txtMsg.AppendText("Get Device List Done.");
            foreach(HPSADevice hpD in deviceList)
            {
                txtMsg.AppendText(string.Format("\n {0} {1} {2}", hpD.DeviceID, hpD.DeviceName, hpD.ProductName));
            }
#endif
        }

        void SetBtnDownloadVisibility(bool isVisiable)
        {
            if (isVisiable)
            {
                btnDownload.Visibility = Visibility.Visible;
                gdBtn.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                gdBtn.ColumnDefinitions[1].Width = new GridLength(10, GridUnitType.Pixel);
            }
            else
            {
                btnDownload.Visibility = Visibility.Collapsed;
                gdBtn.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Auto);
                gdBtn.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Auto);
            }
        }
        bool CheckUpdatReg(string keyLocation, string keyName)
        {
            bool rev = false;
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(keyLocation);
            if (null != regKey)
            {
                if (Convert.ToBoolean(regKey.GetValue(keyName, 0)))
                {
                    rev = true;
                }
                regKey.Close();
            }
            return rev;
        }

        private void ShowLog(string log)
        {
            this.Dispatcher.BeginInvoke(new Action(()=> { txtMsg.AppendText(string.Format("{0}\n",log)); txtMsg.ScrollToEnd(); }));
        }


        #region Module Interface
        public string interfaceVersion
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string moduleName
        {
            get
            {
                return "Update";
            }
        }

        public BitmapImage getModuleLogoImage()
        {
            throw new NotImplementedException();
        }

        public BitmapImage getModuleLogoImage2()
        {
            throw new NotImplementedException();
        }

        public int hide()
        {
            throw new NotImplementedException();
        }

        public void initialize()
        {
            throw new NotImplementedException();
        }

        public bool isPlatformSupported()
        {
            return true;
        }

        public int show()
        {
            throw new NotImplementedException();
        }

        public void uninitialize()
        {
            throw new NotImplementedException();
        }
        #endregion

        
    }
}
