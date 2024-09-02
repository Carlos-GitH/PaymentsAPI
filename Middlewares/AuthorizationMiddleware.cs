using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PaymentsApi.DTOs;
using PaymentsApi.Models;
using PaymentsApi.Services;

namespace PaymentsApi.Middlewares
{
    public class AuthorizationMiddleware
    {
        // 1 - Injetar o método next: serve para dar prosseguimento à requisição
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        // 2 - Criar um método Invoke ou InvokeAsync
        public async Task InvokeAsync(HttpContext context, TokenService tokenService, ApiKeyService apiKeyService)
        {
            string? token   = context.Request.Headers["auth"];
            string? api_key = context.Request.Headers["api_key"];
            string? path    = context.Request.Path;
            string? method  = context.Request.Method;

            Console.WriteLine($"Caminho: {path}");
            Console.WriteLine($"Método: {method}");
            if (token is not null) Console.WriteLine($"Token: {token}");
            Console.WriteLine($"Api Key: {api_key}");

            if (path.Contains("Autenticate"))
            {
                await _next(context);
                return;
            };
            
            if (!PublicRoutes.IsPublicRoute(path))
            {

                if (token == null || api_key == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Acesso não autorizado. Erro: Faltando parâmetro token ou api key.");
                    return;
                }
                bool savedToken = await tokenService.VerifyToken(token);
                bool VerifyDate = await tokenService.VerifyTokenByDate(token);
                bool apiKey     = await apiKeyService.VerifyApiKey(api_key);
                if (!savedToken || !apiKey || !VerifyDate)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Acesso não autorizado. Erro: Token inválido ou inexistente");
                    return;
                } else
                {
                    await tokenService.UpdateTokenExpirationDate(token);
                }
            }

            await _next(context);
        }
    }
}