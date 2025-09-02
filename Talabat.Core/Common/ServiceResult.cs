using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Common
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; }
        public T Data { get; private set; }

        private ServiceResult(bool isSuccess, T data, string errorMessage)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
        }

        public static ServiceResult<T> Success(T data) => new ServiceResult<T>(true, data, null);
        public static ServiceResult<T> Failure(string message) => new ServiceResult<T>(false, default, message);
        
    }
}
