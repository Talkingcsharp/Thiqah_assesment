using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thiqah.Shared.Exceptions;

namespace Thiqah.Shared.Validation
{
    public class FluentValidator
    {
        public List<string>? Validate<T>(IValidationModel<T> model,bool throwException = true)
        {
            if (model.IsValid)
            {
                return null;
            }

            if (throwException)
            {
                throw new DataNotValidException(model.Errors!);
            }

            return model.Errors;
        }
    }
}
