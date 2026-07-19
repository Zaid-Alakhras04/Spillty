using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Split.Core.Entities
{
    public class Expenses
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Amount { get; set; }
        public int TripID { get; set; }
        public Trip? Trip { get; set; }
        public int MemberId { get; set; }
        public Member? Member { get; set; }  

        public List<ExpenseShare> Shares { get; set; } = new List<ExpenseShare>();
    }
}
