namespace CleanAssessment.Helpers
{
    public interface ICookieHelper
    {
        public Task<string> GetCookie(string key);
        public Task SetCookie(string key, string value);
        public Task ClearCookie(string key);
    }
}
