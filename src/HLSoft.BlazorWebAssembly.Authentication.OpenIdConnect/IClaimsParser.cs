using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect
{
    public interface IClaimsParser<T>
    {
        IList<Claim> ParseClaims(T claims);
    }
}
