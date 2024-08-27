using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using PaymentsApi.Models;
using PaymentsApi.Repositories;

namespace PaymentsApi.Services
{
    public class ApiKeyService
    {
        private readonly ApiKeyRepository _apiKeyRepository;

        public ApiKeyService(ApiKeyRepository apiKeyRepository)
        {
            _apiKeyRepository = apiKeyRepository;
        }

        public string GenerateKey()
        {
            var randBytes = new byte[4];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(randBytes);
            }

            byte[] hash;
            hash = SHA512.HashData(randBytes);

            string encodedHash = Convert.ToBase64String(hash);

            return encodedHash;
        }

        public async Task<string> GenerateAndSaveKey()
        {
            string generatedKey = this.GenerateKey();

            var apiKeyToCreate = new ApiKeys
            {
                api_key = generatedKey
            };

            await _apiKeyRepository.Create(apiKeyToCreate);

            return generatedKey;
        }

        public async Task<bool> VerifyApiKey(string apiKey)
        {
            var apikey = await _apiKeyRepository.GetByApiKey(apiKey);
            return apikey != null;
        }
    }
}