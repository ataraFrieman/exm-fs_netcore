using System;
using System.Collections.Generic;
using System.Text;

namespace SendingFilesToDB
{
    public interface IConvertToVM<T>
    {
       T Convert();

    }
}
