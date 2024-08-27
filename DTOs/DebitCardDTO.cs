using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsApi.DTOs
{
    public class DebitCardDTO
    {
        public int id { get; set; }
        public string holder_name { get; set; }
        public string holder_document { get; set; }
        public string card_number { get; set; }
        public string cvv { get; set; }
        public string expiration_date { get; set; }
        public decimal card_balance { get; set; }
    }

    public class DebitCardWithTokenDTO
    {
        public int id { get; set; }
        public string holder_name { get; set; }
        public string holder_document { get; set; }
        public string card_number { get; set; }
        public string cvv { get; set; }
        public string expiration_date { get; set; }
        public decimal card_balance { get; set; }
        public string? token { get; set; }
    }

    public class DebitCardsDTO
    {
        public List<DebitCardDTO> cards { get; set; }
    }

    public class DebitCardsWithTokenDTO
    {
        public List<DebitCardDTO> cards { get; set; }
        public string? token { get; set; }
    }

    public class CreateDebitCardDTO
    {
        public string holder_name { get; set; }
        public string holder_document { get; set; }
        public string card_number { get; set; }
        public string cvv { get; set; }
        public string expiration_date { get; set; }
    }

    public class CreatedDebitCardDTO
    {
        public int id { get; set; }
        public string holder_name { get; set; }
        public string holder_document { get; set; }
        public string card_number { get; set; }
        public string cvv { get; set; }
        public string expiration_date { get; set; }
        public decimal card_balance { get; set; }
        public string api_key { get; set; }
    }

    public class CreatedDebitCardWithTokenDTO
    {
        public int id { get; set; }
        public string holder_name { get; set; }
        public string holder_document { get; set; }
        public string card_number { get; set; }
        public string cvv { get; set; }
        public string expiration_date { get; set; }
        public decimal card_balance { get; set; }
        public string api_key { get; set; }
        public string? token { get; set; }
    }
    
    public class DebitCardAdjustBalanceDTO
    {
        public int id { get; set; }
        public decimal value { get; set; }
    }
}