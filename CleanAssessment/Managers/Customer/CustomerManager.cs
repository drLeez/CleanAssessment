﻿using CleanAssessment.Domain.Features.Customer;
using CleanAssessment.Domain.Features.Customer.Queries;
using CleanAssessment.Shared.Bases;
using CleanAssessment.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;

namespace CleanAssessment.Managers.Customer
{
    public class CustomerManager : ICustomerManager
    {
        private readonly HttpClient _httpClient;
        public CustomerManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> AddAsync(CustomerResponse customer)
        {
            var url = $"{Routes.CustomerEndpoints.Save}";
            var response = await _httpClient.PostAsJsonAsync(url, customer);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(CustomerResponse customer)
        {
            var url = $"{Routes.CustomerEndpoints.Delete}";
            var response = await _httpClient.PostAsJsonAsync(url, customer);
            return await response.ToResult<int>();
        }

        public async Task<IResult<CustomerResponse>> EditAsync(CustomerResponse customer)
        {
            var url = $"{Routes.CustomerEndpoints.Update}";
            var response = await _httpClient.PostAsJsonAsync(url, customer);
            return await response.ToResult<CustomerResponse>();
        }

        public async Task<IResult<List<CustomerResponse>>> GetAllAsync(DateTime? startDate, DateTime? endDate, string? firstName, string? lastName)
        {
            var url = $"{Routes.CustomerEndpoints.GetAll}?startDate={startDate?.ToDateId() ?? 0}&endDate={endDate?.ToDateId() ?? 0}&firstName={firstName}&lastName={lastName}";
            var response = await _httpClient.GetAsync(url);
            return await response.ToResult<List<CustomerResponse>>();
        }
    }
}
