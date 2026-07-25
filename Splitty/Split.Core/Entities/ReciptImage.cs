using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Split.Core.Entities
{
    public class ReciptImage
    {
        public int Id { get; set; }

        public string ImagePath { get; set; } = string.Empty;

        public int TripId { get; set; }
        public Trip? Trip { get; set; }
    }
}
