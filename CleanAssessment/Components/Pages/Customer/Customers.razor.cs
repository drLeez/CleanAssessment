using MudBlazor;

namespace CleanAssessment.Components.Pages.Customer
{
    public partial class Customers
    {
        
        private string? _firstNameFilter { get; set; }
        private string? _lastNameFilter { get; set; }
        private bool _useDateFilter { get; set; } = false;
        private DateRange? _dateRangeFilter { get; set; }
        protected override void OnInitialized()
        {
            
        }

        private async Task Refresh()
        {
            var response = await _customerManager.GetAllAsync(null, null, "bob", null);
            if (response.Succeeded)
            {
                _snackBarHelper.Add($"{response.Data.Count} item(s) successfully loaded", Severity.Success);
            }
            else
            {
                _snackBarHelper.Add(response.Messages, Severity.Error);
            }
        }
    }
}
