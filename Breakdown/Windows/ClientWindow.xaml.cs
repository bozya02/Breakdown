using Breakdown.DB;
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

        public ClientWindow(Client client)
        {
            InitializeComponent();

            Client = client;
            Genders = DataAccess.GetGenders();
            Services = DataAccess.GetServices();

            this.DataContext = this;
        }
    }
}
