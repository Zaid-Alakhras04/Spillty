using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Split.Core.Entities
{
    public class Trip
    {
        public int Id {  get; set; }
        public string? Name { get; set; }
        public List<Member> Members { get; set; } = new List<Member>();
        public List<Expenses> Expense { get; set; } = new List<Expenses>();
        public ICollection<ReciptImage> Images { get; set; } = new List<ReciptImage>();


    }
}
