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
    public class CreditCardController : ControllerBase
    {
        private readonly CreditCardRepository _creditCardRepository;
        private readonly ApiKeyService _apiKeyService;
        private readonly PaymentService _paymentService;
        public CreditCardController(CreditCardRepository creditCardRepository, ApiKeyService apiKeyService, PaymentService paymentService)
        {
            _creditCardRepository = creditCardRepository;
            _apiKeyService = apiKeyService;
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult<CreditCardsWithTokenDTO>> Get()
        {
            var creditCards = await _creditCardRepository.GetAll();
            return Ok(creditCards);
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<CreditCardWithTokenDTO>> GetById(int id)
        {
            var creditCard = await _creditCardRepository.GetById(id);
            if (creditCard is null) return NotFound();
            return Ok(creditCard);
        }

        [HttpGet("number/{number}")]
        public async Task<ActionResult<CreditCardWithTokenDTO>> GetByNumber(string number)
        {
            var creditCard = await _creditCardRepository.GetByNumber(number);
            if (creditCard is null) return NotFound();
            return Ok(creditCard);
        }

        [HttpGet("invoice/{id}")]
        public async Task<ActionResult<PaymentsWithTokenDTO>> GetInvoice(int id)
        {
            var payments = await _paymentService.GetCardInvoiceOrStatement(id, "credit");

            return Ok(payments);
        }

        [HttpPost("create")]
        [Consumes("application/json")]
        public async Task<ActionResult<CreatedCardWithTokenDTO>> Create([FromBody] CreateCardDTO createCardDTO)
        {
            var creditCard = await _creditCardRepository.Create(createCardDTO);

            var apiKey = await _apiKeyService.GenerateAndSaveKey();

            creditCard.api_key = apiKey;
            return Ok(creditCard);
        }

        [HttpPatch("adjust_limit")]
        [Consumes("application/json")]
        public async Task<ActionResult<CreditCardWithTokenDTO>> AdjustLimit([FromBody] CreditCardAdjustLimitDTO newCardLimit)
        {
            var id                = newCardLimit.id;
            var value             = newCardLimit.card_limit;
            var creditCard        = await _creditCardRepository.GetById(id);
            if (creditCard is null) return NotFound();
            var creditCardUpdated = await _creditCardRepository.AdjustLimit(id, value);
            return Ok(creditCardUpdated); 
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var cardDeleted = await _creditCardRepository.Delete(id);
            return Ok(cardDeleted);
        }
    }
}