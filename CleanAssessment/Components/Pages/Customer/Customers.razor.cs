using CleanAssessment.Domain.Features.Customer;
using MudBlazor;

namespace CleanAssessment.Components.Pages.Customer
{
    public partial class Customers
    {
        private MudDataGrid<CustomerResponse> _grid;
        private List<CustomerResponse> _customers { get; set; } = new();
        private bool _loading = false;

        private string? _firstNameFilter { get; set; }
        private string? _lastNameFilter { get; set; }
        private bool _useDateFilter { get; set; } = false;
        private DateRange? _dateRangeFilter { get; set; }

        protected override void OnInitialized()
        {
            
        }

        private async Task Refresh()
        {
            _loading = true;
            StateHasChanged();
            var response = await _customerManager.GetAllAsync(_dateRangeFilter?.Start, _dateRangeFilter?.End, _firstNameFilter, _lastNameFilter);
            if (response.Succeeded)
            {
                _customers = response.Data.ToList();
                _snackBarHelper.Add($"{response.Data.Count} item(s) successfully loaded", Severity.Success);
            }
            else
            {
                _snackBarHelper.Add(response.Messages, Severity.Error);
            }
            _loading = false;
            StateHasChanged();
        }
    }
}
