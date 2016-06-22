
#region Usings

using System;
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.EWald
{

    /// <summary>
    /// http://api.e-wald.eu/chargers/
    /// </summary>
    public class EWaldAPI
    {

        #region Data

        public static readonly TimeSpan DefaultQueryTimeout = TimeSpan.FromSeconds(30);

        #endregion

        #region Properties

        public String                               Hostname                    { get; }
        public IPPort                               TCPPort                     { get; }
        public String                               VirtualHost                 { get; }
        public RemoteCertificateValidationCallback  RemoteCertificateValidator  { get; }
        public String                               URIPrefix                   { get; }
        public DNSClient                            DNSClient                   { get; }
//        public String                               APIKey                      { get; }

        #endregion

        #region Constructor(s)

        public EWaldAPI()
        {

            this.Hostname                    = "api.e-wald.eu";
            this.TCPPort                     = IPPort.Parse(80);
            this.VirtualHost                 = "api.e-wald.eu";
            this.RemoteCertificateValidator  = (a, b, c, d) => true;
            this.URIPrefix                   = "";
            this.DNSClient                   = new DNSClient(SearchForIPv6DNSServers: false);
//            this.APIKey                      = "28d9139000abf9a49d6c6e4ece1e3b45";

        }

        #endregion


        public async Task<HTTPResponse<IEnumerable<JObject>>>

            GetChargingStations(CancellationToken?  CancellationToken = null,
                                TimeSpan?           QueryTimeout      = null)

        {

            var response = await new HTTPClient(Hostname,
                                                TCPPort,
                                                RemoteCertificateValidator,
                                                DNSClient).

                                 Execute(client => client.POST(URIPrefix + "/chargers/",
                                                               requestbuilder => {
                                                                   requestbuilder.Host         = VirtualHost;
                                                                   requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                               }),

                                         Timeout:           QueryTimeout.HasValue ? QueryTimeout : DefaultQueryTimeout,
                                         CancellationToken: CancellationToken);

            if (response.HTTPStatusCode == HTTPStatusCode.OK)
            {

                JObject              JSON      = null;
                IEnumerable<JObject> stations  = null;

                try
                {
                    JSON = JObject.Parse(response.HTTPBody.ToUTF8String());
                }
                catch (Exception e)
                {
                    return new HTTPResponse<IEnumerable<JObject>>(response, e);
                }

                try
                {
                    stations = (JSON["features"] as JArray).SafeSelect(aa => aa as JObject);
                }
                catch (Exception e)
                {
                    return new HTTPResponse<IEnumerable<JObject>>(response, e);
                }

                return new HTTPResponse<IEnumerable<JObject>>(response, stations);

            }

            return new HTTPResponse<IEnumerable<JObject>>(response, true);

        }

    }

}
