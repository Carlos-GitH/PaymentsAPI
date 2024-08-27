using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsApi.DTOs
{
    public class CreditCardDTO
    {
        public int id { get; set; }
        public string holder_name { get; set; }
        public string holder_document { get; set; }
        public string card_number { get; set; }
        public string cvv { get; set; }
        public string expiration_date { get; set; }
        public decimal card_limit { get; set; }
    }

    public class CreditCardWithTokenDTO
    {
        public int id { get; set; }
        public string holder_name { get; set; }
        public string holder_document { get; set; }
        public string card_number { get; set; }
        public string cvv { get; set; }
        public string expiration_date { get; set; }
        public decimal card_limit { get; set; }
        public string? token { get; set; }
    }

    public class CreditCardsDTO
    {
        public List<CreditCardDTO> credit_cards { get; set; }
    }

    public class CreditCardsWithTokenDTO
    {
        public List<CreditCardDTO> credit_cards { get; set; }
        public string? token { get; set; }
    }

    public class CreateCardDTO
    {
        public string holder_name { get; set; }
        public string holder_document { get; set; }
        public string card_number { get; set; }
        public string cvv { get; set; }
        public string expiration_date { get; set; }
        public decimal? card_limit { get; set; }
    }

    public class CreatedCardDTO
    {
        public int id { get; set; }
        public string holder_name { get; set; }
        public string holder_document { get; set; }
        public string card_number { get; set; }
        public string cvv { get; set; }
        public string expiration_date { get; set; }
        public decimal? card_limit { get; set; }
        public string? api_key { get; set; }
    }

    public class CreatedCardWithTokenDTO
    {
        public int id { get; set; }
        public string holder_name { get; set; }
        public string holder_document { get; set; }
        public string card_number { get; set; }
        public string cvv { get; set; }
        public string expiration_date { get; set; }
        public decimal? card_limit { get; set; }
        public string? api_key { get; set; }
        public string? token { get; set; }
    }

    public class CreditCardAdjustLimitDTO
    {
        public int id { get; set; }
        public decimal card_limit { get; set; }
    }
}