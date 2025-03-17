using CleanAssessment.Components.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace CleanAssessment.Helpers
{
    public class SnackBarHelper : ISnackBarHelper
    {
        private readonly ISnackbar _snackbar;
        private readonly NavigationManager _navigationManager;
        public SnackBarHelper(ISnackbar snackBar, NavigationManager navigationManager)
        {
            _snackbar = snackBar;
            _navigationManager = navigationManager;
        }

        public record struct HistoricalSnack(Snackbar Snack, DateTime TimeStamp, string? Page) { }
        public static readonly List<HistoricalSnack> History = new();

        public void Add(string message, Severity severity = Severity.Info, bool keepInHistory = true)
        {
            _snackbar.Add(message, severity);
            if (keepInHistory)
            {
                var snack = _snackbar.ShownSnackbars.FirstOrDefault();
                if (snack == null) return;
                var path = _navigationManager.Uri.Split('/').LastOrDefault();
                FavNavLink.LinkToTitles.TryGetValue(path, out var page);
                History.Add(new(snack, DateTime.Now, page));
            }
        }
        public void Add(IEnumerable<string> messages, Severity severity = Severity.Info, bool keepInHistory = true)
        {
            foreach (var message in messages) Add(message, severity, keepInHistory);
        }
    }
}
