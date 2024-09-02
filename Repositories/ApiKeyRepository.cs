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
    public class ApiKeyRepository
    {
        private readonly AppDbContext _dbContext;

        public ApiKeyRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Autenticate(string apiKey)
        {
            var api_key = await _dbContext.ApiKeys.FirstOrDefaultAsync(k => k.api_key == apiKey);
            System.Console.WriteLine($"AAAAAAAAAAAAAAAAAAAAA{api_key}AAAAAAAAAAAAAAAAAAA");
            if (api_key == null) return false;

            return true;
        }

        public async Task<ApiKeys> Create(ApiKeys apiKey)
        {
            _dbContext.ApiKeys.Add(apiKey);
            await _dbContext.SaveChangesAsync();
            return apiKey;
        }

        public async Task<ApiKeyDTO> GetByApiKey(string apiKey)
        {
            // System.Console.WriteLine($"AAAAAAAAAAAAAAAAAAAAA{apiKey}AAAAAAAAAAAAAAAAAAA");
            var apikey = await _dbContext.ApiKeys.FirstOrDefaultAsync(k => k.api_key == apiKey);
            if (apikey == null) return null;
            return new ApiKeyDTO
            {
                api_key = apikey.api_key
            };
        }
    }
}