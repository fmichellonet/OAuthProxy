using System.Collections.Generic;
using System.Linq;
using FryProxy;

namespace OAuthProxy
{
    public class Proxy
    {
        private readonly HttpProxyServer _proxyServer;
        private readonly List<ITransformationRule> _rules;

        public Proxy(string hostName = "localhost", int port = 8050)
        {
            _rules = new List<ITransformationRule>();

            var proxy = new HttpProxy
            {
                OnRequestReceived = OnRequestReceived
            };

            _proxyServer = new HttpProxyServer(hostName, port, proxy);
        }

        public void AddRule(ITransformationRule rule)
        {
            _rules.Add(rule);
        }

        private void OnRequestReceived(ProcessingContext context)
        {
            _rules.Where(x => context.RequestHeader != null && x.Match(context.RequestHeader))
                  .ToList()
                  .ForEach(x => x.Transform(context.RequestHeader));
        }

        public void Start()
        {
            _proxyServer.Start()
                        .WaitOne();
        }

        public void Stop()
        {
            _proxyServer.Stop();
        }
    }
}