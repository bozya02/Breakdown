using Breakdown.DB;
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
    /// Interaction logic for ClientsListWindow.xaml
    /// </summary>
    public partial class ClientsListWindow : Window
    {
        private const int ITEMSONPAGE = 10;
        private int _page = 0;
        private int _pagesCount => Clients.Count / ITEMSONPAGE + (Clients.Count % ITEMSONPAGE != 0 ? 1 : 0);

        private List<Client> _clients;
        public List<Client> Clients { get; set; }
        public Dictionary<string, Func<Client, object>> Sortings { get; set; }
        public List<Gender> Genders { get; set; }

        public ClientsListWindow()
        {
            InitializeComponent();

            _clients = DataAccess.GetClients();
            Sortings = new Dictionary<string, Func<Client, object>>
            {
                { "Без сортировки", x => x.ID },
                { "Дата рождения по возрастанию", x => x.Birthday },
                { "Дата рождения по убыванию", x => x.Birthday },       //reverse
                { "Дата регистрации по возрастанию", x => x.RegistrationDate }, 
                { "Дата регистрации по убыванию", x => x.RegistrationDate },    //reverse
            };
            Genders = DataAccess.GetGenders();
            Genders.Insert(0, new Gender
            {
                Name = "Все",
                Clients = _clients
            });

            DataAccess.AddNewItemEvent += DataAccess_AddNewItemEvent;
            this.DataContext = this;

            ApplyFilters();
        }

        private void DataAccess_AddNewItemEvent()
        {
            _clients = DataAccess.GetClients();
            ApplyFilters();
        }

        private void lvClients_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var client = lvClients.SelectedItem as Client;
            if (client != null)
                new ClientWindow(client).ShowDialog();

            lvClients.SelectedIndex = -1;
        }

        private void ApplyFilters(bool isStartFilter = true)
        {
            var search = tbSearch.Text.ToLower().Trim();
            var sorting = cbSorting.SelectedItem as string;
            var gender = cbGender.SelectedItem as Gender;

            if (string.IsNullOrWhiteSpace(sorting) || gender == null)
                return;

            if (isStartFilter)
                _page = 0;

            Clients = gender.Clients.Where(x => x.FirstName.ToLower().Contains(search) ||
                                                x.LastName.ToLower().Contains(search) ||
                                                x.Patronymic.ToLower().Contains(search) ||
                                                x.Email.ToLower().Contains(search)).ToList();

            Clients = sorting.Contains("убыванию") ?
                      Clients.OrderBy(Sortings[sorting]).ToList() :
                      Clients.OrderByDescending(Sortings[sorting]).ToList();

            lvClients.ItemsSource = Clients.Skip(_page * ITEMSONPAGE).Take(ITEMSONPAGE);
            lvClients.Items.Refresh();

            GeneratePages();
        }

        private void cbGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void Paginator(object sender, MouseButtonEventArgs e)
        {
            var content = (sender as TextBlock).Text;

            if (content.Contains("<") && _page > 0)
                _page--;
            else if (content.Contains(">") && _page < _pagesCount - 1)
                _page++;
            else if (int.TryParse(content, out int pageNumber))
                _page = pageNumber - 1;

            ApplyFilters(false);
        }

        public void GeneratePages()
        {
            spPages.Children.Clear();

            for (int i = 0; i < _pagesCount; i++)
            {
                spPages.Children.Add(new TextBlock
                {
                    Text = (i + 1).ToString(),
                    Style = Application.Current.FindResource("Paginator") as Style
                });

                spPages.Children[i].PreviewMouseDown += Paginator;
            }

            if (spPages.Children.Count != 0)
                (spPages.Children[_page] as TextBlock).TextDecorations = TextDecorations.Underline;
        }
    }
}
