using System;
using System.Collections.Generic;
using System.Text;

namespace SharedEntities
{
    public class ServiceResponse<T>
    {
        public ServiceResponse(T Data)
        {
            this.Data = Data;
            this.Success = true;
            this.Message = null;
        }

        public T Data { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
