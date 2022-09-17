using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thiqah.Shared.Exceptions
{
    public sealed class DataNotValidException : BaseException
    {
        public DataNotValidException(List<string> validationErrors, string message = "Object is not valid", int code = 400) : base(message, code, validationErrors)
        {

        }
    }
}
