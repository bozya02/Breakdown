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

        public static List<Client> GetClients() => BreakdownEntities.GetContext().Clients.ToList();
        public static List<Gender> GetGenders() => BreakdownEntities.GetContext().Genders.ToList();

        public static void SaveClient(Client client)
        {
            if (client.ID == 0)
                _clients.Add(client);

            BreakdownEntities.GetContext().SaveChanges();
            AddNewItemEvent?.Invoke();
        }
    }
}
