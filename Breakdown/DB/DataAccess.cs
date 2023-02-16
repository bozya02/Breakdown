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
        public event AddNewItem AddNewItemEvent;

        private static DbSet<Client> _clients => BreakdownEntities.GetContext().Clients;
    }
}
