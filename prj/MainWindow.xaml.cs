using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace fruits
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FruitsDataBase FruitsDB;
        public MainWindow()
        {
            InitializeComponent();
            FruitsDB = new FruitsDataBase();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var RD = new ReceiveDeliveryWindow();
            RD.Show();
        }

        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<ProductType> ProdTypes = null;
            FruitsDB.GetProductTypes(ref ProdTypes);
            IEnumerable<Provider> providers = null;
            FruitsDB.GetProviders(ref providers);

            DateTime BeginDateDT = (DateTime)BeginDateDP.SelectedDate;
            var BeginDate = BeginDateDT.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime EndDateDT = (DateTime)EndDateDP.SelectedDate;
            var EndDate = EndDateDT.ToString("yyyy-MM-dd HH:mm:ss");

            using (var rep = new StreamWriter("Отчет.txt"))
                foreach (var ProdType in ProdTypes)
                {
                    rep.WriteLine(ProdType.prt_name);
                    foreach (var prov in providers)
                    {
                        
                        IEnumerable<WeightCost> total = null;
                        FruitsDB.GetTotalWeightAndTotalCost(ProdType.prt_id, prov.prv_id, BeginDate, EndDate, ref total);
                        var total2 = total.First();
                        rep.WriteLine($"{prov.prv_name}. Вес: {total2.weight}. Стоимость: {total2.cost}.");
                    }
                }
            Process.Start("Отчет.txt");
        }
    }
}
