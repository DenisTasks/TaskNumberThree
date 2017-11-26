using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskNumberThree.Interfaces
{
    public interface ISaveInfo<T>
    {
        ICollection<T> GetCallInfo();
    }
}
