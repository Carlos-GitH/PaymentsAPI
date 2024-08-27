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
    public class DebitCardRepository
    {
        private readonly AppDbContext _dbContext;

        public DebitCardRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DebitCardsWithTokenDTO> GetAll()
        {
            var cards = await _dbContext.Set<DebitCardDTO>().FromSqlRaw("SELECT * FROM debit_cards").ToListAsync();
            return new DebitCardsWithTokenDTO {
                cards = cards
            };
            
        }

        public async Task<DebitCardWithTokenDTO> GetById(int id)
        {
            var card = await _dbContext.DebitCards.Where(x => x.id == id).FirstOrDefaultAsync();
            return new DebitCardWithTokenDTO
            {
                id = card.id,
                holder_name = card.holder_name,
                holder_document = card.holder_document,
                card_number = card.card_number,
                cvv = card.cvv,
                expiration_date = card.expiration_date,
                card_balance = card.card_balance
            };
        }

        public async Task<DebitCardWithTokenDTO> GetByNumber(string number)
        {
            var card = await _dbContext.DebitCards.Where(x => x.card_number == number).FirstOrDefaultAsync();
            return new DebitCardWithTokenDTO
            {
                id = card.id,
                holder_name = card.holder_name,
                holder_document = card.holder_document,
                card_number = card.card_number,
                cvv = card.cvv,
                expiration_date = card.expiration_date,
                card_balance = card.card_balance
            };
        }

        public async Task<DebitCardDTO> Create(CreateDebitCardDTO card)
        {
            var sql = """
                INSERT INTO debit_cards(holder_name
                                     , holder_document
                                     , card_number
                                     , cvv
                                     , expiration_date
                                     , card_balance)
                     VALUES(@holder_name
                          , @holder_document
                          , @card_number
                          , @cvv
                          , @expiration_date
                          , @card_balance)
                RETURNING *;
            """;
            var parameters = new []
            {
                new Npgsql.NpgsqlParameter("holder_name", card.holder_name),
                new Npgsql.NpgsqlParameter("holder_document", card.holder_document),
                new Npgsql.NpgsqlParameter("card_number", card.card_number),
                new Npgsql.NpgsqlParameter("cvv", card.cvv),
                new Npgsql.NpgsqlParameter("expiration_date", card.expiration_date),
                new Npgsql.NpgsqlParameter("card_balance", NpgsqlTypes.NpgsqlDbType.Numeric) {Value = 0.0}
            };

            var createdCard = await _dbContext.Set<DebitCardDTO>().FromSqlRaw(sql, parameters).ToListAsync();

            return new DebitCardDTO
            {
                id = createdCard[0].id,
                holder_name = createdCard[0].holder_name,
                holder_document = createdCard[0].holder_document,
                card_number = createdCard[0].card_number,
                cvv = createdCard[0].cvv,
                expiration_date = createdCard[0].expiration_date,
                card_balance = createdCard[0].card_balance
            };
        }

        public async Task<DebitCardWithTokenDTO> AdjustBalance(int id, decimal value)
        {
            var sql = """
                UPDATE debit_cards
                   SET card_balance = card_balance + @value
                 WHERE id = @id
                RETURNING *;
            """;
            var parameters = new []
            {
                new Npgsql.NpgsqlParameter("id", id),
                new Npgsql.NpgsqlParameter("value", value)
            };
            var updatedCard = await _dbContext.Set<DebitCardDTO>().FromSqlRaw(sql, parameters).ToListAsync();
            return new DebitCardWithTokenDTO{
                id = updatedCard[0].id,
                holder_name = updatedCard[0].holder_name,
                holder_document = updatedCard[0].holder_document,
                card_number = updatedCard[0].card_number,
                cvv = updatedCard[0].cvv,
                expiration_date = updatedCard[0].expiration_date,
                card_balance = updatedCard[0].card_balance
            };
        }

        public async Task<DebitCardWithTokenDTO> Delete(int id)
        {
            var cardToDelete = await _dbContext.DebitCards.FirstOrDefaultAsync(c => c.id == id);
            if (cardToDelete is null) return null;
            _dbContext.Remove(cardToDelete);
            return new DebitCardWithTokenDTO
            {
                id = cardToDelete.id,
                holder_name = cardToDelete.holder_name,
                holder_document = cardToDelete.holder_document,
                card_number = cardToDelete.card_number,
                cvv = cardToDelete.cvv,
                expiration_date = cardToDelete.expiration_date,
                card_balance = cardToDelete.card_balance
            };
        }
    }
}