using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Split.Core.DTOs
{
    public class DebtSettelment
    {
        public string? FromName { get; set; }
        public string? ToName { get; set; }
        public decimal Amount { get; set; }

    }
}
