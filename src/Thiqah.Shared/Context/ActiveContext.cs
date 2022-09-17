using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thiqah.Shared.Context
{
    public sealed class ActiveContext
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? RequestId { get; set; }

        public void FromActiveContext(ActiveContext input)
        {
            Id = input.Id;
            UserId = input.UserId;
            RequestDate = input.RequestDate;
            RequestId = input.RequestId;
        }
    }
}
