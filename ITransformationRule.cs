using FryProxy.Headers;

namespace OAuthProxy
{
    public interface ITransformationRule
    {
        bool Match(HttpRequestHeader requestHeader);
        HttpRequestHeader Transform(HttpRequestHeader requestHeader);
    }
}