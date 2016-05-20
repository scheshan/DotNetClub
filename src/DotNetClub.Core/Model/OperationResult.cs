using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Model
{
    public class OperationResult
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
        
        public OperationResult()
        {
            this.Success = true;
        }

        public static OperationResult Failure(string errorMessage)
        {
            return new OperationResult
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Data { get; set; }

        public OperationResult(T data)
        {
            this.Success = true;
            this.Data = data;
        }

        public static new OperationResult<T> Failure(string errorMessage)
        {
            return new OperationResult<T>(default(T))
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
