using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentsApi.DTOs;
using PaymentsApi.Repositories;
using PaymentsApi.Services;

namespace PaymentsApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AutenticateController : ControllerBase
    {
        private readonly ApiKeyRepository _apiKeyRepository;
        private readonly TokenService _tokenService;
        private readonly ApiKeyService _apiKeyService;

        public AutenticateController(ApiKeyRepository apiKeyRepository, TokenService tokenService, ApiKeyService apiKeyService)
        {
            _apiKeyRepository = apiKeyRepository;
            _tokenService = tokenService;
            _apiKeyService = apiKeyService;
        }

        [HttpPost]
        public async Task<IActionResult> Autenticate()
        {
            var apiKey = HttpContext.Request.Headers["api_key"];
            if (apiKey == "") return Unauthorized();
            var validKey = await _apiKeyRepository.Autenticate(apiKey);
            if (!validKey) return Unauthorized();
            var token = await _tokenService.GenerateAndSaveToken();
            return Ok(token);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var apiKey = await _apiKeyService.GenerateAndSaveKey();
            return Ok(apiKey);
        }
    }
}