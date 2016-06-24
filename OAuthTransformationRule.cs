using System;
using System.Net.Http.Headers;
using FryProxy.Headers;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace OAuthProxy
{
    public class OAuthTransformationRule : TransformationRuleBase
    {
        private readonly OAuthSecurityInfo _securityInfo;

        public OAuthTransformationRule(Func<HttpRequestHeader, bool> comparer, 
                                       Func<HttpRequestHeader, HttpRequestHeader> transformer, 
                                       OAuthSecurityInfo securityInfo)
            : base(comparer, transformer)
        {
            _securityInfo = securityInfo;
        }

        public override HttpRequestHeader Transform(HttpRequestHeader requestHeader)
        {
            requestHeader = base.Transform(requestHeader);

            AuthenticationContext authContext = new AuthenticationContext(_securityInfo.Authority);

            // Authenticate the user and get a token from Azure AD
            AuthenticationResult authResult = authContext.AcquireTokenAsync(_securityInfo.Resource, _securityInfo.ClientId, _securityInfo.RedirectUri, new PlatformParameters(PromptBehavior.Auto)).Result;
            // Add Bearer in the header
            requestHeader.Authorization = new AuthenticationHeaderValue(authResult.AccessTokenType, authResult.AccessToken).ToString();
            return requestHeader;
        }
    }
}