using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thiqah.Shared.Exceptions;

public sealed class ExceptionViewModel
{
    public string? Message { get; set; }
    public int Code { get; set; }
    public string? Status { get; set; }
    public string? StackTrace { get; set; }
    public string? RequestId { get; set; }
    public List<string>? ValidationResult { get; set; }
    public ExceptionViewModel? InnerException { get; set; }
}

