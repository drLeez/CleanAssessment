using CleanAssessment.Shared.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanAssessment.Shared.Extensions
{
    public static class ResultExtensions
    {
        public static async Task<IResult<T>> ToResult<T>(this HttpResponseMessage response)
        {
            var str = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<Result<T>>(str, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return obj;
        }
        public static async Task<IResult> ToResult(this HttpResponseMessage response)
        {
            var str = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<Result>(str, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return obj;
        }
    }
}
