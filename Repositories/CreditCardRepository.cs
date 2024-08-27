using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentsApi.Data;
using PaymentsApi.DTOs;
using PaymentsApi.Models;

namespace PaymentsApi.Repositories
{
    public class CreditCardRepository
    {
        private readonly AppDbContext _dbContext;

        public CreditCardRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreditCardsWithTokenDTO> GetAll()
        {
            var cards = await _dbContext.Set<CreditCardDTO>().FromSqlRaw("SELECT * FROM credit_cards").ToListAsync();
            return new CreditCardsWithTokenDTO
            {
                credit_cards = cards
            };
        }

        public async Task<CreditCardWithTokenDTO> GetById(int id)
        {
            var card = await _dbContext.CreditCards.FirstOrDefaultAsync(c => c.id == id);
            if (card is null) return null;

            var cardReturn = new CreditCardWithTokenDTO
            {
                id              = card.id,
                holder_name     = card.holder_name,
                holder_document = card.holder_document,
                card_number     = card.card_number,
                cvv             = card.cvv,
                expiration_date = card.expiration_date,
                card_limit      = card.card_limit
            };
            return cardReturn;
        }

        public async Task<CreditCardDTO> GetByNumber(string number)
        {
            var card = await _dbContext.CreditCards.FirstOrDefaultAsync(c => c.card_number == number);
            if (card is null) return null;
            return new CreditCardDTO
            {
                id              = card.id,
                holder_name     = card.holder_name,
                holder_document = card.holder_document,
                card_number     = card.card_number,
                cvv             = card.cvv,
                expiration_date = card.expiration_date,
                card_limit      = card.card_limit
            };
        }

        public async Task<CreatedCardWithTokenDTO> Create(CreateCardDTO card)
        {
            var sql = """
                INSERT INTO credit_cards(holder_name
                                       , holder_document
                                       , card_number
                                       , cvv
                                       , expiration_date
                                       , card_limit)
                     VALUES(@holder_name
                          , @holder_document
                          , @card_number
                          , @cvv
                          , @expiration_date
                          , @card_limit)
                RETURNING *;
            """;
            var parameters = new []
            {
                new Npgsql.NpgsqlParameter("holder_name", card.holder_name),
                new Npgsql.NpgsqlParameter("holder_document", card.holder_document),
                new Npgsql.NpgsqlParameter("card_number", card.card_number),
                new Npgsql.NpgsqlParameter("cvv", card.cvv),
                new Npgsql.NpgsqlParameter("expiration_date", card.expiration_date),
                new Npgsql.NpgsqlParameter("card_limit", card.card_limit)
            };

            var createdCard = await _dbContext.Set<CreditCardDTO>().FromSqlRaw(sql, parameters).ToListAsync();
            return new CreatedCardWithTokenDTO
            {
                id              = createdCard[0].id,
                holder_name     = createdCard[0].holder_name,
                holder_document = createdCard[0].holder_document,
                card_number     = createdCard[0].card_number,
                cvv             = createdCard[0].cvv,
                expiration_date = createdCard[0].expiration_date,
                card_limit      = createdCard[0].card_limit
            };
        }

        public async Task<CreditCardWithTokenDTO> AdjustLimit(int id, decimal value)
        {
            var sql = """
                UPDATE credit_cards
                   SET card_limit = card_limit + @value
                 WHERE id = @id
                RETURNING *;
            """;
            var parameters = new []
            {
                new Npgsql.NpgsqlParameter("id", id),
                new Npgsql.NpgsqlParameter("value", value)
            };

            var updatedCard = await _dbContext.Set<CreditCardDTO>().FromSqlRaw(sql, parameters).ToListAsync();
            var updatedWithToken = new CreditCardWithTokenDTO
            {
                id = updatedCard[0].id,
                holder_name = updatedCard[0].holder_name,
                holder_document = updatedCard[0].holder_document,
                card_number = updatedCard[0].card_number,
                cvv = updatedCard[0].cvv,
                expiration_date = updatedCard[0].expiration_date,
                card_limit = updatedCard[0].card_limit
            };
            return updatedWithToken;
        }

        public async Task<CreditCardWithTokenDTO> Delete(int id)
        {
            var cardToDelete = await _dbContext.CreditCards.FirstOrDefaultAsync(c => c.id == id);
            if (cardToDelete is null) return null;
            _dbContext.Remove(cardToDelete);
            return new CreditCardWithTokenDTO {
                id = cardToDelete.id,
                holder_name = cardToDelete.holder_name,
                holder_document = cardToDelete.holder_document,
                card_number = cardToDelete.card_number,
                cvv = cardToDelete.cvv,
                expiration_date = cardToDelete.expiration_date,
                card_limit = cardToDelete.card_limit
            };
        }
    }
}