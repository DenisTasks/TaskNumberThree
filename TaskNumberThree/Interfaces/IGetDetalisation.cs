using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.VirtualModel;
using TaskNumberThree.VirtualModel.Auxiliary;

namespace TaskNumberThree.Interfaces
{
    public interface IGetDetalisation
    {
        ICollection<CallString> GetDetalisation(int mobileNumber);
    }
}
