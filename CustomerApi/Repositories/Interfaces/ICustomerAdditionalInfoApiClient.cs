using CustomerApi.Dtos;
using Refit;

namespace CustomerApi.Repositories.Interfaces;
public interface ICustomerAdditionalInfoApiClient
{
    [Get("/customerAdditionalInfos/{customerId}")]
    Task<CustomerAdditionalInfoDto> GetCustomerAdditionalInfo(string customerId);

    [Post("/somaNumeros")]
    Task<int> PostSomaNumeros(CustomerAdditionalInfoSomaNumerosDto numeros);
}
