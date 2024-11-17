using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace AgriBlazorServer
{

    public class CustomHttpClientHandler : HttpClientHandler
    {
        public CustomHttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        }
    }
}
