using Breakdown.DB;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Breakdown.Windows
{
    /// <summary>
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        public Client Client { get; set; }
        public List<Gender> Genders { get; set; }
        public List<Service> Services { get; set; }

        public ClientWindow(Client client, bool isNew = false)
        {
            InitializeComponent();

            Client = client;
            Genders = DataAccess.GetGenders();
            Services = DataAccess.GetServices();

            if (client.ClientServices.Count() > 0)
                btnDelete.Visibility = Visibility.Collapsed;

            if (isNew)
            {
                btnDelete.Visibility = Visibility.Collapsed;
                client.RegistrationDate = DateTime.Now;
            }

            this.DataContext = this;
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "*.png|*.png|*.jpg|*.jpg|*.jpeg|*.jpeg"
            };

            if (fileDialog.ShowDialog().Value)
            {
                Client.Photo = File.ReadAllBytes(fileDialog.FileName);
                imgPhoto.Source = new BitmapImage(new Uri(fileDialog.FileName));
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить данного клиента?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DataAccess.DeleteClient(Client);
                this.Close();
            }
        }

        private void cbServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Client.ClientServices.Add(new ClientService
            {
                Service = cbService.SelectedItem as Service,
                Client = Client,
                StartTime = DateTime.Now
            });

            lvServices.ItemsSource = Client.ClientServices;
            lvServices.Items.Refresh();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Client.LastName))
                stringBuilder.AppendLine("Фамилия обязательна для заполнения!");
            if (string.IsNullOrWhiteSpace(Client.FirstName))
                stringBuilder.AppendLine("Имя обязательно для заполнения!");
            if (string.IsNullOrWhiteSpace(Client.Phone))
                stringBuilder.AppendLine("Телефон обязателен для заполнения!");
            if (Client.Gender == null)
                stringBuilder.AppendLine("Пол обязателен для заполнения!");

            if (stringBuilder.Length > 0)
            {
                MessageBox.Show(stringBuilder.ToString(), "Ошибка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                return;
            }

            DataAccess.SaveClient(Client);
            this.Close();

        }
    }
}
