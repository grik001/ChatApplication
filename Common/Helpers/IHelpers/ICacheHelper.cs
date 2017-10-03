using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers.IHelpers
{
    public interface ICacheHelper
    {
        T GetValue<T>(string key);

        bool SetValue<T>(string key, T value);
    }
}
