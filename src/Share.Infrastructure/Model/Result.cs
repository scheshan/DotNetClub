using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Share.Infrastructure.Model
{
    public class Result
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        public static Result SuccessResult()
        {
            return new Result
            {
                Success = true
            };
        }

        public static Result ErrorResult(string errorMessage)
        {
            return new Result
            {
                ErrorMessage = errorMessage
            };
        }

        public static Result<T> SuccessResult<T>(T data)
        {
            return new Result<T>
            {
                Success = true,
                Data = data
            };
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }

        public static new Result<T> ErrorResult(string errorMessage)
        {
            return new Result<T>
            {
                ErrorMessage = errorMessage
            };
        }
    }
}
