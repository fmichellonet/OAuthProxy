using System;
using FryProxy.Headers;

namespace OAuthProxy
{
    public class TransformationRuleBase : ITransformationRule
    {

        private readonly Func<HttpRequestHeader, bool> _comparer;
        private readonly Func<HttpRequestHeader, HttpRequestHeader> _tranformer;

        public TransformationRuleBase(Func<HttpRequestHeader, bool> comparer,
            Func<HttpRequestHeader, HttpRequestHeader> tranformer)
        {
            _comparer = comparer;
            _tranformer = tranformer;
        }

        public virtual bool Match(HttpRequestHeader requestHeader)
        {
            return _comparer(requestHeader);
        }

        public virtual HttpRequestHeader Transform(HttpRequestHeader requestHeader)
        {
            return _tranformer(requestHeader);
        }
    }
}