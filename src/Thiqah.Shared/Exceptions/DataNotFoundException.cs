using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thiqah.Shared.Exceptions
{
    public sealed class DataNotFoundException : BaseException
    {
        public DataNotFoundException(string message = "Object is not found") : base(message, 404, null)
        {

        }
    }
}
