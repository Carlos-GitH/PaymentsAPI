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
    public class PaymentController : ControllerBase
    {
        private readonly PaymentRepository _paymentRepository;
        private readonly CreditCardService _creditCardService;
        private readonly DebitCardService _debitCardService;

        public PaymentController(PaymentRepository paymentRepository, CreditCardService creditCardService, DebitCardService debitCardService)
        {
            _paymentRepository = paymentRepository;
            _creditCardService = creditCardService;
            _debitCardService = debitCardService;
        }

        [HttpGet]
        public async Task<ActionResult<PaymentsDTO>> GetAll()
        {
            var payments = await _paymentRepository.GetAll();
            return Ok(payments);
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<PaymentsDTO>> GetById(int id)
        {
            var payment = await _paymentRepository.GetById(id);
            return Ok(payment);
        }

        [HttpPost("pay")]
        [Consumes("application/json")]
        public async Task<ActionResult<PaymentDTO>> Pay([FromBody] PayDTO payDTO)
        {
            if (payDTO.card_type == "credit")
            {
                var validateCard = await _creditCardService.ValidateCard(payDTO.card_info, payDTO.value);
                if (!validateCard) return BadRequest("Invalid credit card info or not enough credit");
            } else if (payDTO.card_type == "debit") {
                var validateCard = await _debitCardService.ValidateCard(payDTO.card_info, payDTO.value);
                if (!validateCard) return BadRequest("Invalid debit card info or insuficient funds");
            } else {
                return BadRequest("Invalid payment type");
            }

            var payment = await _paymentRepository.Pay(payDTO);

            if (payment is null) return NotFound();

            if (payDTO.card_type == "credit")
            {
                var value = payDTO.value * -1;
                await _creditCardService.AdjustLimit(payDTO.card_info.id, value);
            } else {
                var value = payDTO.value * -1;
                await _debitCardService.AdjustBalance(payDTO.card_info.id, value);
            }

            return Ok(payment);
        }

        [HttpPatch("cancel/{id}")]
        public async Task<ActionResult<PaymentDTO>> Cancel(int id)
        {
            var payment = await _paymentRepository.Cancel(id);
            if (payment is null) return NotFound("Payment doesn't exist or already canceled");

            if (payment.card_type == "credit")
            {
                var value = payment.value;
                var creditCard = await _creditCardService.AdjustLimit(int.Parse(payment.card_id), value);
            } else {
                var value = payment.value;
                var debitCard  = await _debitCardService.AdjustBalance(int.Parse(payment.card_id), value);
            }
            return Ok(payment);
        }
    }
}