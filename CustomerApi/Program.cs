using CustomerApi.Repositories;
using CustomerApi.Repositories.Interfaces;
using Refit;
using System.Net;
using System.Net.Http.Headers;
using CustomerApi.Dtos;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAuthTokenProvider, AuthTokenProvider>();

builder.Services
    .AddRefitClient<ICustomerAdditionalInfoApiClient>()
    .ConfigureHttpClient((serviceProvider, c) =>
    {
        var tokenProvider = serviceProvider.GetRequiredService<IAuthTokenProvider>();
        var token = tokenProvider.GetToken();

        c.BaseAddress = new Uri("http://localhost:5080");
        c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    });

var app = builder.Build();

app.MapGet("/customers/additionalInfo/{id}", async (string id, ICustomerAdditionalInfoApiClient additionalInfoApiClient) =>
{
    try
    {
        var additionalInfo = await additionalInfoApiClient.GetCustomerAdditionalInfo(id);

        return Results.Ok(additionalInfo);
    }
    catch (ApiException ex) 
    when (ex.StatusCode == HttpStatusCode.NotFound)
    {
        return Results.NotFound($"Additional information not found for the customer id: {id}");
    }
    catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
    {
        return Results.Unauthorized();
    }
});


app.MapPost("/soma", async ([FromBody] CustomerAdditionalInfoSomaNumerosDto numeros, ICustomerAdditionalInfoApiClient additionalInfoApiClient) =>
{
    try
    {
        var additionalInfo = await additionalInfoApiClient.PostSomaNumeros(numeros);

        return Results.Ok(additionalInfo);
    }
    catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
    {
        return Results.Unauthorized();
    }
});



app.Run();
