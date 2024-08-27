using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PaymentsApi.Services;

namespace PaymentsApi.Filters
{
    public class AsyncAuthorizeActionFilter : IAsyncActionFilter
    {
        private readonly TokenService _tokenService;

        public AsyncAuthorizeActionFilter(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();

            if (executedContext.Result is ObjectResult result && result.Value is not null)
            {
                var token = await _tokenService.GenerateAndSaveToken();
                var currentToken = result.Value.GetType().GetProperty("token");
                currentToken.SetValue(result.Value, token);
                System.Console.WriteLine("PASSOU AQUI");
            }
        }
    }
}