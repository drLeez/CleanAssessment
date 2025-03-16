using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Shared.Bases
{
    public class Result : IResult
    {
        public Result() { }

        public List<string> Messages { get; set; } = [];

        public bool Succeeded { get; set; } = false;

        public static IResult Fail()
        {
            return new Result()
            {
                Succeeded = false,
            };
        }
        public static IResult Fail(IEnumerable<string> messages)
        {
            return new Result()
            {
                Messages = messages.ToList(),
                Succeeded = false,
            };
        }
        public static IResult Fail(string message) => Fail([message]);

        public static Task<IResult> FailAsync() => Task.FromResult(Fail());
        public static Task<IResult> FailAsync(IEnumerable<string> messages) => Task.FromResult(Fail(messages));
        public static Task<IResult> FailAsync(string message) => Task.FromResult(Fail(message));

        public static IResult Success(string? message = null)
        {
            var ret = new Result()
            {
                Succeeded = false,
            };
            if (message != null) ret.Messages = [message];
            return ret;
        }
        public static Task<IResult> SuccessAsync(string? message = null) => Task.FromResult(Fail(message));
    }
    public class Result<T> : Result, IResult<T>
    {
        public T Data { get; set; }

        public static new Result<T> Fail()
        {
            return new Result<T>()
            {
                Succeeded = false,
            };
        }
        public static new Result<T> Fail(IEnumerable<string> messages)
        {
            return new Result<T>()
            {
                Messages = messages.ToList(),
                Succeeded = false,
            };
        }
        public static new Result<T> Fail(string message) => Fail([message]);

        public static new Task<Result<T>> FailAsync() => Task.FromResult(Fail());
        public static new Task<Result<T>> FailAsync(IEnumerable<string> messages) => Task.FromResult(Fail(messages));
        public static new Task<Result<T>> FailAsync(string message) => Task.FromResult(Fail(message));

        public static new Result<T> Success(string? message = null)
        {
            var ret = new Result<T>()
            {
                Succeeded = false,
            };
            if (message != null) ret.Messages = [message];
            return ret;
        }
        public static new Task<Result<T>> SuccessAsync(string? message = null) => Task.FromResult(Fail(message));

        public static Result<T> Success(T data)
        {
            return new Result<T>()
            {
                Data = data,
                Succeeded = true,
            };
        }
        public static Task<Result<T>> SuccessAsync(T data) => Task.FromResult(Success(data));
    }
}
