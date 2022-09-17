using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thiqah.Shared.Validation;
public interface IValidationModel<T>
{
    AbstractValidator<T> Validator { get; }
    bool IsValid => Validator.Validate((T)this).IsValid;
    List<string>? Errors => Validator.Validate((T)this).Errors?.Select(s => s.ErrorMessage)?.ToList();
}
