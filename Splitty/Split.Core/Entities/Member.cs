using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Split.Core.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TripID { get; set; }
        public Trip Trip { get; set; }
        public List<Expenses> Expense { get; set; } = new List<Expenses>();

    }
}
