
using Microsoft.JSInterop;
using System.Reflection.Metadata.Ecma335;

namespace CleanAssessment.Helpers
{
    public class CookieHelper : ICookieHelper
    {
        private readonly IJSRuntime _jsRuntime;
        public CookieHelper(IJSRuntime jSRuntime)
        {
            _jsRuntime = jSRuntime;
        }
        public async Task ClearCookie(string key)
        {
            await _jsRuntime.InvokeVoidAsync("ClearCookie", key);
        }

        public async Task<string> GetCookie(string key)
        {
            string cookies = await _jsRuntime.InvokeAsync<string>("GetCookies");
            if (string.IsNullOrEmpty(cookies)) return string.Empty;
            var list = cookies.Split(';');
            foreach (var cookie in list)
            {
                var split = cookie.Split('=', 2);
                if (split.Length != 2) continue;
                if (split[0] == key)
                {
                    return split[1];
                }
            }
            return string.Empty;
        }

        public async Task SetCookie(string key, string value)
        {
            await _jsRuntime.InvokeVoidAsync("SetCookie", key, value);
        }
    }
}
