using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

namespace TehServies
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TehServiesEntities1 db = new TehServiesEntities1();
        public MainWindow()
        {
            InitializeComponent();
            JSONParse();
            Load();
        }
        void Load()
        {
            try
            {
                lwZayavki.ItemsSource = db.Zayavki.ToList();
                lwZayavki.SelectionMode = SelectionMode.Single;

                cbClient.ItemsSource = db.Accounts.ToList();
                cbClient.SelectedIndex = 0;
                cbClient.DisplayMemberPath = "Login";
                cbClient.SelectedValuePath = "ID";

                cbOtvetstvennii.ItemsSource = db.Accounts.ToList();
                cbOtvetstvennii.SelectedIndex = 0;
                cbOtvetstvennii.DisplayMemberPath = "Login";
                cbOtvetstvennii.SelectedValuePath = "ID";

                cbCrush.ItemsSource = db.Crushes.ToList();
                cbCrush.SelectedIndex = 0;
                cbCrush.DisplayMemberPath = "Name";
                cbCrush.SelectedValuePath = "ID";

                cbObor.ItemsSource = db.Oborudovaniye.ToList();
                cbObor.SelectedIndex = 0;
                cbObor.DisplayMemberPath = "Name";
                cbObor.SelectedValuePath = "ID";

                cbStatus.ItemsSource = db.Statuses.ToList();
                cbStatus.SelectedIndex = 0;
                cbStatus.DisplayMemberPath = "Name";
                cbStatus.SelectedValuePath = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при добавлении!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lwZayavki_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lwZayavki.SelectedItem != null)
                {
                    Zayavki zayavki = (Zayavki)lwZayavki.SelectedItem;

                    dpDate.SelectedDate = zayavki.Date;
                    tbDescr.Text = zayavki.Descr;
                    tbNumber.Text = zayavki.Number.ToString();

                    cbClient.SelectedItem = zayavki.Accounts;
                    cbCrush.SelectedItem = zayavki.Crushes;
                    cbObor.SelectedItem = zayavki.Oborudovaniye1;
                    cbOtvetstvennii.SelectedItem = zayavki.Accounts1;
                    cbStatus.SelectedItem = zayavki.Statuses;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при добавлении!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void tbAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Zayavki zayavki = new Zayavki
                {
                    Number = Convert.ToInt32(tbNumber.Text),
                    Date = dpDate.SelectedDate,
                    Oborudovaniye = (int)cbObor.SelectedValue,
                    Crush = (int)cbCrush.SelectedValue,
                    Descr = tbDescr.Text,
                    Client = (int)cbClient.SelectedValue,
                    Status = (int)cbStatus.SelectedValue,
                    Otvetstvennii = (int)cbOtvetstvennii.SelectedValue,
                };

                db.Zayavki.Add(zayavki);

                db.SaveChanges();
                Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при добавлении!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void tbEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lwZayavki.SelectedItem != null)
                {
                    Zayavki zayavki = (Zayavki)lwZayavki.SelectedItem;

                    zayavki.Number = Convert.ToInt32(tbNumber.Text);
                    zayavki.Date = dpDate.SelectedDate;
                    zayavki.Oborudovaniye = (int)cbObor.SelectedValue;
                    zayavki.Crush = (int)cbCrush.SelectedValue;
                    zayavki.Descr = tbDescr.Text;
                    zayavki.Client = (int)cbClient.SelectedValue;
                    zayavki.Status = (int)cbStatus.SelectedValue;
                    zayavki.Otvetstvennii = (int)cbOtvetstvennii.SelectedValue;

                    db.SaveChanges();
                    Load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при изменении!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void tbDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lwZayavki.SelectedItem != null)
                {
                    Zayavki zayavki = (Zayavki)lwZayavki.SelectedItem;
                    db.Zayavki.Remove(zayavki);

                    db.SaveChanges();
                    Load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при удалении!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int search = int.Parse(tbSearch.Text);

                lwZayavki.ItemsSource = db.Zayavki.Where(z => z.Number == search).ToList();

                if (lwZayavki.Items.Count == 0)
                {
                    Load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при поиске!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void JSONParse()
        {
            StreamReader streamReader = new StreamReader(@"C:\Users\Artem\Downloads\Car_Filling_Station.json");

            string jsonString = streamReader.ReadToEnd();


            JArray jsonArray = JArray.Parse(jsonString);


            List<FuelStation> stList = new List<FuelStation>();

            for (int i = 0; i < jsonArray.Count; i++)
            {
                JObject obj = (JObject)jsonArray[i];
                FuelStation fuelstation = new FuelStation(
                    obj["Address"].ToString(),
                    (int)obj["Station_ID"],
                    
                    obj["data"]
                );

                stList.Add(fuelstation);
            }
            foreach (FuelStation fuelstation in stList)
            {
                MessageBox.Show($"Address: {fuelstation.address}, Station_ID: {fuelstation.stationID}\n");
            }
        }
        class FuelStation
        {
            public string address;
            public int stationID;
            public List<Fuel> fuel;

            public FuelStation(
                string address,
                int stationID,
            List<Fuel> fuel
            )
            {
                this.address = address;
                this.stationID = stationID;
                this.fuel = fuel;
            }
        }
        class Fuel
        {
            public string name;
            public decimal price;
            public int amountOfFuel;

            public Fuel(string name, decimal price, int amountOfFuel)
            {
                this.name = name;
                this.price = price;
                this.amountOfFuel = amountOfFuel;
            }
        }
    }
}
