using AutoMapper;
using ElectionMaterialManager.CQRS.Responses;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using FluentValidation;
using System;

namespace ElectionMaterialManager.Utilities
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException exception)
            {
                var response = new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "One or more validation errors has occurred"
                };
                if (exception.Errors != null )
                {
                    response.Errors = exception.Errors.Select(x => x.ErrorMessage).AsEnumerable();
                }

                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.Response.WriteAsJsonAsync(response);
            }
        }
       
    }
}
