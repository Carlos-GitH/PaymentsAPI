using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using PaymentsApi.Repositories;
using static PaymentsApi.Repositories.TokenRepository;

namespace PaymentsApi.Services
{
    public class TokenService
    {
        private readonly TokenRepository _tokenRepository;

        public TokenService(TokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public string GenerateToken()
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

        public async Task<string> GenerateAndSaveToken()
        {
            string generatedToken = this.GenerateToken();

            var tokenToCreate = new Models.Tokens
            {
                token = generatedToken
            };

            await _tokenRepository.Create(tokenToCreate);
        
            return tokenToCreate.token;
        }

        public async Task<bool> VerifyToken(string token)
        {
            var savedToken = await _tokenRepository.GetByTokenString(token);
            if (savedToken != null) return true;
            return false;
        }

        public async Task DeleteToken(string token)
        {
            await _tokenRepository.DeleteByTokenString(token);
        }
    }
}