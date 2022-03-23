using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Consumer.Abstractions
{
    public interface IRabbitConsumer
    {
        public void Consumer();
    }
}
