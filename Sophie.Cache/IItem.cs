using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sophie.Cache
{
    public interface IItem<out T>
    {
        Guid Id { get; }

        String Name { get; }

        DateTime Stamp { get; }

        T Value { get; }
    }
}