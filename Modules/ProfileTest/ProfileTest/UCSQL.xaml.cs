using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProfileTest
{
    /// <summary>
    /// Interaction logic for UCSQL.xaml
    /// </summary>
    public partial class UCSQL : UserControl
    {
        const string OMENDBName = "OMEN_DB.db";
        UtilityUILib.Db_API dbSql;
        public UCSQL()
        {
            InitializeComponent();
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            dbSql = new UtilityUILib.Db_API(System.IO.Path.Combine(appPath, OMENDBName));
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch(btn.Name)
            {
                case "btnCSQL":
                    dbSql.CreatTableByName("OMENTest");
                    dbSql.InsertToTable("OMENTest", string.Empty, string.Empty);
                    break;
            }

        }
    }
}
