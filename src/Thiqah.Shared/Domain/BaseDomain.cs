using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thiqah.Shared.Domain
{
    public abstract class BaseDomain
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public int? UserCreated { get; set; }
        public int? UserUpdated { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
