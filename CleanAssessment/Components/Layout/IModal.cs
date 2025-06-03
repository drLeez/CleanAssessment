using MudBlazor;

namespace CleanAssessment.Components.Layout
{
    public interface IModal
    {
        static abstract Task<IDialogReference> GetDialog(IDialogService service, params object[] args);
    }
}
