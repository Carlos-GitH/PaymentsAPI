using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsApi.Models
{
    public class Payments
    {
        public int id { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public decimal value { get; set; }
        public string status { get; set; }
        public string card_id { get; set; }
        public string card_type { get; set; }
    }
}