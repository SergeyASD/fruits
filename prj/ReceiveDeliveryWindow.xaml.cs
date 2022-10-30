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
using System.Windows.Shapes;

namespace fruits
{
    /// <summary>
    /// Interaction logic for ReceiveDelivery.xaml
    /// </summary>
    public partial class ReceiveDeliveryWindow : Window
    {
        FruitsDataBase FruitsDB;
        List<DeliveryPosition> positions;
        public ReceiveDeliveryWindow()
        {
            InitializeComponent();
            FruitsDB = new FruitsDataBase();
            positions = new List<DeliveryPosition>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ;
        }

        private void CalcPositionBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                float price = 0;
                var prov = (Provider)ProviderComboBox.SelectedItem;

                var ProdType = (ProductType)ProductTypeComboBox.SelectedItem;
                if (!FruitsDB.GetPrice(ref price, prov.prv_id, ProdType.prt_id))
                {
                    StatusTextBlock.Text = "Не удалось получить цену товара.";
                    return;
                }

                var Total = Convert.ToSingle(PosWeightTextBox.Text) * price;
                PosPriceLabel.Content = Total;
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = "Ошибка в приложении.";
            }
        }

        private void AddPositionBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var prov = (Provider)ProviderComboBox.SelectedItem;
                var ProdType = (ProductType)ProductTypeComboBox.SelectedItem;

                var pos = new DeliveryPosition();
                pos.ProviderId = prov.prv_id;
                pos.ProductTypeId = ProdType.prt_id;
                pos.Weight = Convert.ToSingle(PosWeightTextBox.Text);
                pos.Price = Convert.ToSingle(PosPriceLabel.Content);

                positions.Add(pos);
                
                PositionsTextBox.AppendText($"{prov.prv_name } {ProdType.prt_name} Вес: {pos.Weight} Стоимость: {pos.Price}\r\n");
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = "Ошибка в приложении.";
            }
        }

        private void AcceptDeliveryBtn_Click(object sender, RoutedEventArgs e)
        {
            FruitsDB.AddDelivery(positions);
            this.Close();
        }
    }
}
