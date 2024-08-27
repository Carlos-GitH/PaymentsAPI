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
    public class TokenRepository
    {
        private readonly AppDbContext _dbContext;

        public TokenRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tokens> Create(Tokens token)
        {
            _dbContext.Tokens.Add(token);
            await _dbContext.SaveChangesAsync();

            return token;
        }

        public async Task<Tokens?> GetByTokenString(string token)
        {
            return await _dbContext.Tokens.FirstOrDefaultAsync(t => t.token == token);
        }

        public async Task DeleteByTokenString(string token)
        {
            var tokenToDelete = await _dbContext.Tokens.FirstAsync(t => t.token == token);
            _dbContext.Tokens.Remove(tokenToDelete);
            await _dbContext.SaveChangesAsync();
        }
    }
}
