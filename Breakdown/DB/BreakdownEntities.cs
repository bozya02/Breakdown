using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakdown.DB
{
    public partial class BreakdownEntities
    {
        private static BreakdownEntities _context;

        public static BreakdownEntities GetContext()
        {
            if (_context == null)
                _context = new BreakdownEntities();

            return _context;
        }
    }
}
