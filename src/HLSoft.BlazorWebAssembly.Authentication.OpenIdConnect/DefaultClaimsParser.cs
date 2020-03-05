using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect
{
    internal class DefaultClaimsParser : IClaimsParser<object>
    {
		public IList<Claim> ParseClaims(object claims)
		{
			//var result = new List<Claim>();
			//if (claims == null)
			//	return result;
			//var claimsObj = (JsonElement)claims;
			//if (claimsObj.ValueKind != JsonValueKind.Object)
			//	return result;

			//ParseClaims(claimsObj, result, 1);

			//return result;

			return null;
		}

		//private void ParseClaims(JsonElement jsonElem, IList<Claim> claims, int level)
		//{
		//	foreach (var item in jsonElem.EnumerateObject())
		//	{
		//		switch (item.Value.ValueKind)
		//		{
		//			case JsonValueKind.Null:
		//			case JsonValueKind.Undefined:
		//				break;
		//			case JsonValueKind.Array:
		//				break;
		//			case JsonValueKind.Object:
		//				if (level < 3)
		//				{
		//					ParseClaims(item.Value, claims, level + 1);
		//				}
		//				break;
		//			default:
		//				claims.Add(new Claim(item.Name, item.Value.ToString()));
		//				break;
		//		}
		//	}
		//}
	}
}
