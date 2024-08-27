using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentsApi.DTOs;
using PaymentsApi.Filters;
using PaymentsApi.Repositories;
using PaymentsApi.Services;

namespace PaymentsApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [TypeFilter(typeof(AsyncAuthorizeActionFilter))]
    public class DebitCardController : ControllerBase
    {
        private readonly DebitCardRepository _debitCardRepository;
        private readonly ApiKeyService _apiKeyService;
        private readonly PaymentService _paymentService;

        public DebitCardController(DebitCardRepository debitCardRepository, ApiKeyService apiKeyService, PaymentService paymentService)
        {
            _debitCardRepository = debitCardRepository;
            _apiKeyService       = apiKeyService;
            _paymentService      = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult<DebitCardsWithTokenDTO>> GetAll()
        {
            var debitCards = await _debitCardRepository.GetAll();
            return Ok(debitCards);
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<DebitCardWithTokenDTO>> GetById(int id)
        {
            var debitCard = await _debitCardRepository.GetById(id);
            if (debitCard is null) return NotFound();
            return Ok(debitCard);
        }

        [HttpGet("number/{number}")]
        public async Task<ActionResult<DebitCardWithTokenDTO>> GetByNumber(string number)
        {
            var debitCard = await _debitCardRepository.GetByNumber(number);
            if (debitCard is null) return NotFound();
            return Ok(debitCard);
        }

        [HttpGet("bank_statement/{id}")]
        public async Task<ActionResult<PaymentsWithTokenDTO>> GetBankStatement(int id)
        {
            var payments = await _paymentService.GetCardInvoiceOrStatement(id, "debit");
            return Ok(payments);
        }

        [HttpPost("create")]
        [Consumes("application/json")]
        public async Task<ActionResult<CreatedDebitCardWithTokenDTO>> Create([FromBody] CreateDebitCardDTO createCardDTO)
        {
            var debitCard = await _debitCardRepository.Create(createCardDTO);

            var apiKey = await _apiKeyService.GenerateAndSaveKey();

            var cardWithKey = new CreatedDebitCardWithTokenDTO
            {
                id              = debitCard.id,
                card_number     = debitCard.card_number,
                cvv             = debitCard.cvv,
                expiration_date = debitCard.expiration_date,
                holder_name     = debitCard.holder_name,
                holder_document = debitCard.holder_document,
                card_balance    = debitCard.card_balance,
                api_key         = apiKey
            };

            return Ok(cardWithKey);
        }

        [HttpPatch("adjust_balance")]
        [Consumes("application/json")]
        public async Task<ActionResult<DebitCardWithTokenDTO>> AdjustBalance([FromBody] DebitCardAdjustBalanceDTO newBalance)
        {
            var id    = newBalance.id;
            var value = newBalance.value;
            var debitCard = await _debitCardRepository.GetById(id);
            if (debitCard is null) return NotFound();
            var debitCardUpdated = await _debitCardRepository.AdjustBalance(id, value);
            return Ok(debitCardUpdated);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var cardDeleted = await _debitCardRepository.Delete(id);
            return Ok(cardDeleted);
        }
    }
}