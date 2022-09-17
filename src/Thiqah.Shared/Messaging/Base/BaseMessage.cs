using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Thiqah.Shared.Messaging.Base
{
    public abstract class BaseMessage<T>
    {
        public BaseMessageHeader Header { get; set; }
        public BaseMessage()
        {
            Header = new BaseMessageHeader();
            Header.Topic = this.GetType().Name;
            Header.Queue = Assembly.GetEntryAssembly()?.GetName()?.Name + "." + this.GetType().Name;
        }
        public T? Body
        {
            get
            {
                return (T)Convert.ChangeType(this, typeof(T));
            }
        }
    }
}
