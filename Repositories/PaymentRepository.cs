using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentsApi.Data;
using PaymentsApi.DTOs;
using PaymentsApi.Models;

namespace PaymentsApi.Repositories
{
    public class PaymentRepository
    {
        private readonly AppDbContext _dbContext;

        public PaymentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaymentsWithTokenDTO> GetAll()
        {
            var payments = await _dbContext.Payments.ToListAsync();

            var paymentsWithToken = new PaymentsWithTokenDTO();
            foreach(var payment in payments)
            {
                var paymentDTO = new PaymentDTO
                {
                    id          = payment.id,
                    description = payment.description,
                    date        = payment.date,
                    value       = payment.value,
                    status      = payment.status,
                    card_id     = payment.card_id
                };
                paymentsWithToken.payments.Add(paymentDTO);
            }
            return paymentsWithToken;
            
        }

        public async Task<PaymentWithTokenDTO> GetById(int id)
        {
            var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.id == id);

            if (payment is null) return null;

            var paymentReturn = new PaymentWithTokenDTO
            {
                id          = payment.id,
                description = payment.description,
                date        = payment.date,
                value       = payment.value,
                status      = payment.status,
                card_id     = payment.card_id
            };
            return paymentReturn;
        }

        public async Task<PaymentsWithTokenDTO> GetCardInvoiceOrStatement(int id, string type)
        {
            var sql = """
                SELECT *
                  FROM payments
                 WHERE card_id   = @id
                   AND card_type = @type
                   AND status    = @status
            """;
            var parameters = new []
            {
                new Npgsql.NpgsqlParameter("id", id),
                new Npgsql.NpgsqlParameter("type", type),
                new Npgsql.NpgsqlParameter("status", "confirmed")
            };
            var payments = await _dbContext.Set<PaymentDTO>().FromSqlRaw(sql, parameters).ToListAsync();
            var paymentsDTO = new PaymentsWithTokenDTO {
                payments = payments
            };

            return paymentsDTO;
        }

        public async Task<PaymentWithTokenDTO> Pay(PayDTO payment)
        {
            var sql = """
                INSERT INTO payments(description
                                   , date
                                   , value
                                   , status
                                   , card_id
                                   , card_type)
                     VALUES(@description
                          , CURRENT_TIMESTAMP
                          , @value
                          , @status
                          , CAST(@card_id AS text)
                          , @type)
                RETURNING *;
            """;
            var parameters = new []
            {
                new Npgsql.NpgsqlParameter("description", payment.description),
                new Npgsql.NpgsqlParameter("value", payment.value),
                new Npgsql.NpgsqlParameter("status", "confirmed"),
                new Npgsql.NpgsqlParameter("card_id", payment.card_id.ToString()),
                new Npgsql.NpgsqlParameter("type", payment.card_type)
            };

            var newPayment = await _dbContext.Set<PaymentDTO>().FromSqlRaw(sql, parameters).ToListAsync();

            var paymentWithToken = new PaymentWithTokenDTO
            {
                id          = newPayment[0].id,
                description = newPayment[0].description,
                date        = newPayment[0].date,
                value       = newPayment[0].value,
                status      = newPayment[0].status,
                card_id     = newPayment[0].card_id,
                card_type   = newPayment[0].card_type
            };
            return paymentWithToken;
        }

        public async Task<PaymentWithTokenDTO> Cancel(int id)
        {
            var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.id == id);

            if (payment is null) return null;
            
            payment.status = "canceled";

            await _dbContext.SaveChangesAsync();
            
            return new PaymentWithTokenDTO
            {
                id          = payment.id,
                description = payment.description,
                date        = payment.date,
                value       = payment.value,
                status      = payment.status,
                card_id     = payment.card_id
            };
        }
    }
}