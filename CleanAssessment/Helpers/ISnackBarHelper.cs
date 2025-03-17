using MudBlazor;

namespace CleanAssessment.Helpers
{
    public interface ISnackBarHelper
    {
        public void Add(string message, Severity severity = Severity.Info, bool keepInHistory = true);
        public void Add(IEnumerable<string> messages, Severity severity = Severity.Info, bool keepInHistory = true);
    }
}
