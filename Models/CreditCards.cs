using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsApi.Models
{
    public class CreditCards
    {
        public int id { get; set; }
        public string holder_name { get; set; }
        public string holder_document { get; set; }
        public string card_number { get; set; }
        public string cvv { get; set; }
        public string expiration_date { get; set; }
        public decimal card_limit { get; set; } = 0;
    }
}