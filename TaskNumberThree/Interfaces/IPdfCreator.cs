using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskNumberThree.VirtualModel.Auxiliary;

namespace TaskNumberThree.Interfaces
{
    public interface IPdfCreator
    {
        void PdfCreate(ICollection<CallString> callStrings, string fileName);
    }
}
