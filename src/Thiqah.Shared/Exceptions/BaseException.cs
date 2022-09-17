using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thiqah.Shared.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException(string message, int code = 500, List<string>? validationErrors = null) : base(message)
        {
            Code = code;
            ValidationErrors = validationErrors;
        }
        public int Code { get; set; }
        public List<string>? ValidationErrors { get; set; }
    }
}
