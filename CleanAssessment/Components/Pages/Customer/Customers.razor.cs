using CleanAssessment.Components.Layout;
using CleanAssessment.Domain.Features.Customer;
using MudBlazor;
using static MudBlazor.CategoryTypes;

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

        protected override async Task OnAfterRenderAsync(bool first)
        {
            if (!first) return;
            await Refresh();
        }

        private bool NoItemSelected()
        {
            return _grid.SelectedItem == null;
        }
        private string RowClassFunc(CustomerResponse item, int rowIndex)
        {
            return item == _grid.SelectedItem ? $"selected" : string.Empty;
        }
        private async Task OnRowClick(DataGridRowClickEventArgs<CustomerResponse> args)
        {
            if (args.MouseEventArgs.Detail == 2)
            {
                await _grid.SetSelectedItemAsync(args.Item);
                await _grid.SetEditingItemAsync(args.Item);
            }
            StateHasChanged();
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
            await _grid.SetSelectedItemAsync(null);
            _loading = false;
            StateHasChanged();
        }
        private async Task InvokeAddModal()
        {
            var options = new DialogOptions()
            {
                CloseButton = false,
                MaxWidth = MaxWidth.Large,
                BackdropClick = false,
            };
            var dialog = await _dialogService.ShowAsync<AddCustomerModal>("Add Customer", options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                var data = result.Data as CustomerResponse;
                if (data != null)
                {
                    var response = await _customerManager.AddAsync(data);
                    if (response.Succeeded)
                    {
                        _snackBarHelper.Add($"{data.FullName} successfully added!", Severity.Success);
                    }
                    else
                    {
                        _snackBarHelper.Add(response.Messages, Severity.Error);
                    }
                }
                await Refresh();
            }
        }
        private async Task InvokeEditModal()
        {
            if (_grid.SelectedItem == null)
            {
                _snackBarHelper.Add("Select a customer before attempting to edit", Severity.Warning, false);
            }
            await _grid.SetEditingItemAsync(_grid.SelectedItem);
        }
        private async Task CommitEdit(CustomerResponse customer)
        {
            _loading = true;
            StateHasChanged();
            
            var response = await _customerManager.EditAsync(customer);
            if (response.Succeeded)
            {
                _snackBarHelper.Add($"Changes to customer successfully saved", Severity.Success);
            }
            else
            {
                _snackBarHelper.Add(response.Messages, Severity.Error);
            }
            
            _loading = false;
            StateHasChanged();
        }
        private async Task InvokeDeleteModal()
        {
            var current = _grid.SelectedItem;
            var dialog = await ConfirmModal.Dialog(_dialogService, "Delete Customer", $"Are you sure you want to delete {current.FullName} ?", "Delete"
                , icon: Icons.Material.Filled.Delete
                , buttonColor: Color.Error
                );
            _loading = true;
            StateHasChanged();
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                var response = await _customerManager.DeleteAsync(current);
                if (response.Succeeded)
                {
                    _snackBarHelper.Add($"Customer deleted", Severity.Success);
                }
                else
                {
                    _snackBarHelper.Add(response.Messages, Severity.Error);
                }
                await Refresh();
            }
            _loading = false;
            StateHasChanged();
        }
    }
}
