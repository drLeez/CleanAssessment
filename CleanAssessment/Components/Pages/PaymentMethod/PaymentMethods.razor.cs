using CleanAssessment.Components.Layout;
using CleanAssessment.Components.Pages.Customer;
using CleanAssessment.Domain.Features.Customer;
using CleanAssessment.Domain.Features.PaymentMethod;
using MudBlazor;

namespace CleanAssessment.Components.Pages.PaymentMethod
{
    public partial class PaymentMethods
    {
        private MudDataGrid<PaymentMethodResponse> _grid;
        private List<PaymentMethodResponse> _paymentMethods { get; set; } = new();
        private List<CustomerResponse> _customers { get; set; } = new();
        private bool _loading = false;

        private CustomerResponse? _filterCustomer;
        private CustomerResponse? FilterCustomer
        {
            get => _filterCustomer;
            set
            {
                if (value == _filterCustomer) return;
                _filterCustomer = value;
                if (_filterCustomer != null) Refresh();
            }
        }

        protected override async Task OnAfterRenderAsync(bool first)
        {
            if (!first) return;
            await Refresh(false);
        }

        private bool NoItemSelected()
        {
            return _grid.SelectedItem == null;
        }
        private string RowClassFunc(PaymentMethodResponse item, int rowIndex)
        {
            return item == _grid.SelectedItem ? $"selected" : string.Empty;
        }
        private async Task OnRowClick(DataGridRowClickEventArgs<PaymentMethodResponse> args)
        {
            if (args.MouseEventArgs.Detail == 2)
            {
                await _grid.SetSelectedItemAsync(args.Item);
                await _grid.SetEditingItemAsync(args.Item);
            }
            StateHasChanged();
        }

        private async Task Refresh(bool doPay = true)
        {
            _loading = true;
            StateHasChanged();

            if (doPay)
            {
                var response = await _paymentMethodManager.GetAllAsync(_filterCustomer);
                if (response.Succeeded)
                {
                    _paymentMethods = response.Data.ToList();
                    _snackBarHelper.Add($"{response.Data.Count} item(s) successfully loaded", Severity.Success);
                }
                else
                {
                    _snackBarHelper.Add(response.Messages, Severity.Error);
                }
            }

            var customerResponse = await _customerManager.GetAllAsync(null, null, null, null);
            if (customerResponse.Succeeded)
            {
                _customers = customerResponse.Data.ToList();
            }
            else
            {
                _snackBarHelper.Add(customerResponse.Messages, Severity.Error);
            }

            await _grid.SetSelectedItemAsync(null);
            _loading = false;
            StateHasChanged();
        }
        private async Task InvokeAddModal()
        {
            //var options = new DialogOptions()
            //{
            //    CloseButton = false,
            //    MaxWidth = MaxWidth.Large,
            //    BackdropClick = false,
            //};
            //var dialog = await _dialogService.ShowAsync<AddCustomerModal>("Add Customer", options);
            //var result = await dialog.Result;
            //if (!result.Canceled)
            //{
            //    var data = result.Data as CustomerResponse;
            //    if (data != null)
            //    {
            //        var response = await _customerManager.AddAsync(data);
            //        if (response.Succeeded)
            //        {
            //            _snackBarHelper.Add($"{data.FullName} successfully added!", Severity.Success);
            //        }
            //        else
            //        {
            //            _snackBarHelper.Add(response.Messages, Severity.Error);
            //        }
            //    }
            //    await Refresh();
            //}
        }
        private async Task InvokeEditModal()
        {
            if (_grid.SelectedItem == null)
            {
                _snackBarHelper.Add("Select a payment method before attempting to edit", Severity.Warning, false);
            }
            await _grid.SetEditingItemAsync(_grid.SelectedItem);
        }
        private async Task CommitEdit(PaymentMethodResponse customer)
        {
            //_loading = true;
            //StateHasChanged();

            //var response = await _customerManager.EditAsync(customer);
            //if (response.Succeeded)
            //{
            //    _snackBarHelper.Add($"Changes to customer successfully saved", Severity.Success);
            //}
            //else
            //{
            //    _snackBarHelper.Add(response.Messages, Severity.Error);
            //}

            //_loading = false;
            //StateHasChanged();
        }
        private async Task InvokeDeleteModal()
        {
            //var current = _grid.SelectedItem;
            //var dialog = await ConfirmModal.Dialog(_dialogService, "Delete Customer", $"Are you sure you want to delete {current.NickName} ?", "Delete"
            //    , icon: Icons.Material.Filled.Delete
            //    , buttonColor: Color.Error
            //    );
            //_loading = true;
            //StateHasChanged();
            //var result = await dialog.Result;
            //if (!result.Canceled)
            //{
            //    var response = await _customerManager.DeleteAsync(current);
            //    if (response.Succeeded)
            //    {
            //        _snackBarHelper.Add($"Customer deleted", Severity.Success);
            //    }
            //    else
            //    {
            //        _snackBarHelper.Add(response.Messages, Severity.Error);
            //    }
            //    await Refresh();
            //}
            //_loading = false;
            //StateHasChanged();
        }

        private async Task<IEnumerable<CustomerResponse>> SearchCustomers(string s, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(s))
            {
                return _customers;
            }
            return _customers.Where(x => x.FullName.Contains(s, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
