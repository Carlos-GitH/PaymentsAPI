using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentsApi.DTOs;
using PaymentsApi.Repositories;

namespace PaymentsApi.Services
{
    public class CreditCardService
    {
        private readonly CreditCardRepository _creditCardRepository;
        
        public CreditCardService(CreditCardRepository creditCardRepository)
        {
            _creditCardRepository = creditCardRepository;
        }

        public async Task<bool> ValidateCard(CardInfoDTO cardInfo, decimal value)
        {
            var card = await _creditCardRepository.GetById(cardInfo.id);
            if (card is null) return false;
            if (!(card.card_number     == cardInfo.card_number     && 
                  card.cvv             == cardInfo.cvv             &&
                  card.expiration_date == cardInfo.expiration_date &&
                  card.holder_name     == cardInfo.holder_name     &&
                  card.holder_document == cardInfo.holder_document)) return false;
            if (!HasCredit(card.card_limit, value)) return false;
            return true;
        }

        public bool HasCredit(decimal card_limit, decimal value)
        {
            if (card_limit < value) return false;
            return true;
        }

        public async Task<CreditCardWithTokenDTO> AdjustLimit(int id, decimal value)
        {
            return await _creditCardRepository.AdjustLimit(id, value);
        }
    }
}