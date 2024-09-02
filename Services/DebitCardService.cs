using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentsApi.DTOs;
using PaymentsApi.Repositories;

namespace PaymentsApi.Services
{
    public class DebitCardService
    {
        private readonly DebitCardRepository _debitCardRepository;

        public DebitCardService(DebitCardRepository debitCardRepository)
        {
            _debitCardRepository = debitCardRepository;
        }

        public async Task<bool> ValidateCard(CardInfoDTO cardInfo, decimal value)
        {
            var card = await _debitCardRepository.GetById(cardInfo.id);
            if(card is null) return false;
            if(!(card.card_number == cardInfo.card_number &&
                 card.cvv == cardInfo.cvv &&
                 card.expiration_date == cardInfo.expiration_date &&
                 card.holder_name == cardInfo.holder_name &&
                 card.holder_document == cardInfo.holder_document)) return false;
            if(!HasBalance(card.card_balance, value)) return false;
            return true;
        }

        public bool HasBalance(decimal card_balance, decimal value)
        {
            if (card_balance < value) return false;
            return true;
        }

        public async Task<DebitCardWithTokenDTO> AdjustBalance(int id, decimal value)
        {
            var card = await _debitCardRepository.AdjustBalance(id, value);
            return card;
        }
    }
}