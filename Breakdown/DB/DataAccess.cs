using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakdown.DB
{
    public class DataAccess
    {
        public delegate void AddNewItem();
        public static event AddNewItem AddNewItemEvent;

        private static DbSet<Client> _clients => BreakdownEntities.GetContext().Clients;
        private static DbSet<ClientService> _clientServices => BreakdownEntities.GetContext().ClientServices;

        public static List<Client> GetClients() => BreakdownEntities.GetContext().Clients.ToList();
        public static List<Gender> GetGenders() => BreakdownEntities.GetContext().Genders.ToList();
        public static List<Service> GetServices() => BreakdownEntities.GetContext().Services.ToList();

        public static void SaveClient(Client client)
        {
            if (client.ID == 0)
                _clients.Add(client);

            BreakdownEntities.GetContext().SaveChanges();
            AddNewItemEvent?.Invoke();
        }

        public static void DeleteClient(Client client)
        {
            _clients.Remove(client);
            BreakdownEntities.GetContext().SaveChanges();
            AddNewItemEvent?.Invoke();
        }

        public static void DeleteClientService(ClientService clientService)
        {
            _clientServices.Remove(clientService);
            BreakdownEntities.GetContext().SaveChanges();
            AddNewItemEvent?.Invoke();
        }
    }
}
