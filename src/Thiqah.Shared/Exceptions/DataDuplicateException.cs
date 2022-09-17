using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thiqah.Shared.Exceptions
{
    public sealed class DataDuplicateException:BaseException
    {
        public DataDuplicateException(string message = "Data is duplicated") : base(message, 409, null)
        {

        }
    }
}
