using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider
{
    public interface IContextInspector<T> where T : class
    {
        void Inspect(T context);
    }
}
