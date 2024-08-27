using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsApi.DTOs
{
    public class PaymentDTO
    {
        public int id { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public decimal value { get; set; }
        public string status { get; set; }
        public string card_id { get; set; }
        public string card_type { get; set; }
    }

    public class PaymentWithTokenDTO
    {
        public int id { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public decimal value { get; set; }
        public string status { get; set; }
        public string card_id { get; set; }
        public string card_type { get; set; }
        public string? token { get; set; }
    }

    public class PaymentsDTO
    {
        public List<PaymentDTO> payments { get; set; }
    }

    public class PaymentsWithTokenDTO
    {
        public List<PaymentDTO> payments { get; set; }
        public string token { get; set; }
    }

    public class PayDTO
    {
        public string description { get; set; }
        public decimal value { get; set; }
        public string? status { get; set; }
        public string card_id { get; set; }
        public string card_type { get; set; }
        public CardInfoDTO card_info { get; set; }
    }
}