using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.Services;

namespace SharpOAuth2.Provider
{
    public abstract class ContextProcessor<T> where T : class
    {
        protected readonly IServiceFactory ServiceFactory;


        protected ContextProcessor(IServiceFactory serviceFactory)
        {
            ServiceFactory = serviceFactory;
        }


        public abstract bool IsSatisfiedBy(T context);

        public abstract void Process(T context);
    }
}
