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

        public async Task UpdateTokenExpirationDate(string token)
        {
            await _tokenRepository.UpdateTokenExpirationDate(token);
        }

        // public bool VerifyDate(DateTime createdAt)
        // {
        //     DateTime now = DateTime.UtcNow.AddHours(-3);

        //     System.Console.WriteLine($"createdAt: {createdAt}");
        //     System.Console.WriteLine($"now: {now}");
            
        //     TimeSpan difference = now - createdAt;

        //     bool isValid = difference.TotalMinutes <= 30;

        //     System.Console.WriteLine($"isValid: {isValid}");
        //     return isValid;
        // }

        public bool VerifyDate(DateTime expirationDate)
        {
            DateTime now = DateTime.UtcNow.AddHours(-3);

            System.Console.WriteLine($"expirationDate: {expirationDate}");
            System.Console.WriteLine($"now: {now}");
            
            // TimeSpan difference = now - expirationDate;
            if (now > expirationDate) return false;
            return true;

            // bool isValid = difference.TotalMinutes <= 30;

            // System.Console.WriteLine($"isValid: {isValid}");
            // return isValid;
        }

        public async Task<bool> VerifyTokenByDate(string token)
        {
            System.Console.WriteLine($"Verificando token {token}");
            
            var savedToken = await _tokenRepository.GetByTokenString(token);
            if (savedToken == null || savedToken.created_at == null) return false;
            else {
                System.Console.WriteLine($"O token {token} existe.");
                return VerifyDate((DateTime)savedToken.created_at);
            }
        }
    }
}