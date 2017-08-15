using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileTest
{
    public class ProgramsData : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(object sender, string propertyName)
        {
            if (this.PropertyChanged != null)
            { PropertyChanged(sender, new PropertyChangedEventArgs(propertyName)); }
        }
        #endregion

        //DisplayName ==> ProductName property
        //DisplayVersion ==> Derived from ProductVersion property
        //Publisher ==> Manufacturer property
        //VersionMinor ==> Derived from ProductVersion property
        //VersionMajor ==> Derived from ProductVersion property
        //Version ==> Derived from ProductVersion property
        //HelpLink ==> ARPHELPLINK property
        //HelpTelephone ==> ARPHELPTELEPHONE property
        //InstallDate ==> The last time this product received service.
        //The value of this property is replaced each time a patch is applied or removed from
        //the product or the /v Command-Line Option is used to repair the product.
        //If the product has received no repairs or patches this property contains
        //the time this product was installed on this computer.
        //InstallLocation ==> ARPINSTALLLOCATION property
        //InstallSource ==> SourceDir property
        //URLInfoAbout ==> ARPURLINFOABOUT property
        //URLUpdateInfo ==> ARPURLUPDATEINFO property
        //AuthorizedCDFPrefix ==> ARPAUTHORIZEDCDFPREFIX property
        //Comments ==> Comments provided to the Add or Remove Programs control panel.
        //Contact ==> Contact provided to the Add or Remove Programs control panel.
        //EstimatedSize ==> Determined and set by the Windows Installer.
        //Language ==> ProductLanguage property
        //ModifyPath ==> Determined and set by the Windows Installer.
        //Readme ==> Readme provided to the Add or Remove Programs control panel.
        //UninstallString ==> Determined and set by Windows Installer.
        //SettingsIdentifier ==> MSIARPSETTINGSIDENTIFIER property

        string displayName = string.Empty;
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; OnPropertyChanged(this, "DisplayName"); }
        }
        System.Windows.Media.ImageSource displayIcon = null;
        public System.Windows.Media.ImageSource DisplayIcon
        {
            get { return displayIcon; }
            set { displayIcon = value; OnPropertyChanged(this, "DisplayIcon"); }
        }
        string displayVersion = string.Empty;
        public string DisplayVersion
        {
            get { return displayVersion; }
            set { displayVersion = value; OnPropertyChanged(this, "DisplayVersion"); }
        }
        string publisher = string.Empty;
        public string Publisher
        {
            get { return publisher; }
            set { publisher = value; OnPropertyChanged(this, "Publisher"); }
        }
        string versionMinor = string.Empty;
        public string VersionMinor
        {
            get { return versionMinor; }
            set { VersionMinor = value; OnPropertyChanged(this, "VersionMinor"); }
        }
        string versionMajor = string.Empty;
        public string VersionMajor
        {
            get { return versionMajor; }
            set { versionMajor = value; OnPropertyChanged(this, "VersionMajor"); }
        }
        string version = string.Empty;
        public string Version
        {
            get { return version; }
            set { version = value; OnPropertyChanged(this, "Version"); }
        }
        string helpLink = string.Empty;
        public string HelpLink
        {
            get { return helpLink; }
            set { helpLink = value; OnPropertyChanged(this, "HelpLink"); }
        }
        string helpTelephone = string.Empty;
        public string HelpTelephone
        {
            get { return helpTelephone; }
            set { helpTelephone = value; OnPropertyChanged(this, "HelpTelephone"); }
        }
        string installDate = string.Empty;
        public string InstallDate
        {
            get { return installDate; }
            set { installDate = value; OnPropertyChanged(this, "InstallDate"); }
        }
        string installLocation = string.Empty;
        public string InstallLocation
        {
            get { return installLocation; }
            set { installLocation = value; OnPropertyChanged(this, "InstallLocation"); }
        }
        string installSource = string.Empty;
        public string InstallSource
        {
            get { return installSource; }
            set { installSource = value; OnPropertyChanged(this, "InstallSource"); }
        }
        string uRLInfoAbout = string.Empty;
        public string URLInfoAbout
        {
            get { return uRLInfoAbout; }
            set { uRLInfoAbout = value; OnPropertyChanged(this, "URLInfoAbout"); }
        }
        string uRLUpdateInfo = string.Empty;
        public string URLUpdateInfo
        {
            get { return uRLUpdateInfo; }
            set { uRLUpdateInfo = value; OnPropertyChanged(this, "URLUpdateInfo"); }
        }
    }
}
