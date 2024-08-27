using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentsApi.DTOs;
using PaymentsApi.Repositories;

namespace PaymentsApi.Services
{
    public class PaymentService
    {
        private readonly PaymentRepository _paymentRepository;

        public PaymentService(PaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<PaymentsWithTokenDTO> GetCardInvoiceOrStatement(int id, string type)
        {
            var payments = await _paymentRepository.GetCardInvoiceOrStatement(id, type);

            return payments;
        }
    }
}