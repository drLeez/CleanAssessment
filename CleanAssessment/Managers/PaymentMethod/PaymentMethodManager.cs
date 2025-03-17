using CleanAssessment.Domain.Features.Customer;
using CleanAssessment.Domain.Features.PaymentMethod;
using CleanAssessment.Shared.Bases;
using Microsoft.JSInterop;
using System.Net.Http;
using CleanAssessment.Domain.Features.Customer.Queries;
using CleanAssessment.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CleanAssessment.Managers.PaymentMethod
{
    public class PaymentMethodManager : IPaymentMethodManager
    {
        private readonly HttpClient _httpClient;
        public PaymentMethodManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<List<PaymentMethodResponse>>> GetAllAsync(CustomerResponse customer)
        {
            if (customer == null)
            {
                return new Result<List<PaymentMethodResponse>>() { Succeeded = false, Messages = new() { "Please select a Customer first" } };
            }
            var url = $"{Routes.PaymentMethodEndpoints.GetAll}?customerId={customer.CustomerId}";
            var response = await _httpClient.GetAsync(url);
            return await response.ToResult<List<PaymentMethodResponse>>();
        }
    }
}
