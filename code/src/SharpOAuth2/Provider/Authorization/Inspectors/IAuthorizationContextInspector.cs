﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.Authorization.Inspectors
{
    public interface IAuthorizationContextInspector
    {
        void Insepct(IAuthorizationContext context);
    }
}
