using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsApi.Models
{
    public class Tokens
    {
        public int id { get; set; }
        public string token { get; set; }
        public DateTime? created_at { get; set; } = DateTime.UtcNow.AddHours(-3);
        public DateTime? expiration_date { get; set; } =  DateTime.UtcNow.AddHours(-3).AddMinutes(30);
    }
}