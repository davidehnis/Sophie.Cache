using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baxter.Domain;
using Sophie.Domain.Research;

namespace Sophie.Cache
{
    public class Revision<TItemType>
    {
        public Json Data { get; protected set; }

        public Key Id { get; protected set; }

        public DateTime Stamp { get; protected set; }

        public TItemType Value { get; protected set; }

        public static Revision<TItemType> Create(IContext context, TItemType value)
        {
            var result = new Revision<TItemType>
            {
                Value = value,
                Stamp = DateTime.UtcNow,
                Data = context.ToJson()
            };

            return result;
        }
    }
}