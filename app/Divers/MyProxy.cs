using System;
using System.Net;

namespace app.Divers
{
    public class MyProxy : IWebProxy
    {
        public MyProxy(string proxyUri)
            : this(new Uri(proxyUri))
        {
        }

        private MyProxy(Uri proxyUri)
        {
            ProxyUri = proxyUri;
        }

        public Uri ProxyUri { get; }

        public ICredentials Credentials { get; set; }

        public Uri GetProxy(Uri destination)
        {
            return ProxyUri;
        }

        public bool IsBypassed(Uri host)
        {
            return false; /* Proxy all requests */
        }

        public override int GetHashCode()
        {
            if (ProxyUri == null)
                return -1;

            return ProxyUri.GetHashCode();
        }
    }
}