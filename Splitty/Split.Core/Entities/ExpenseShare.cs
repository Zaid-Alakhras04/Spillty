using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Split.Core.Entities
{
    public class ExpenseShare
    {
        public int Id { get; set; }

        public int ExpenseId { get; set; }
        public Expenses? Expense { get; set; }

        public int MemberId { get; set; }
        public Member? Member { get; set; }

        public decimal OwnedAmount { get; set; }
    }
}
