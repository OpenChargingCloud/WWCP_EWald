﻿/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of the Open Charging Cloud API
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Linq;
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

        #endregion

        #region Constructor(s)

        public EWaldAPI()
        {

            this.Hostname                    = "api.e-wald.eu";
            this.TCPPort                     = IPPort.Parse(80);
            this.VirtualHost                 = "api.e-wald.eu";
            this.RemoteCertificateValidator  = null;
            this.URIPrefix                   = "";
            this.DNSClient                   = new DNSClient(SearchForIPv6DNSServers: false);

        }

        #endregion


        public async Task<HTTPResponse<IEnumerable<ChargingStation>>>

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
                    return new HTTPResponse<IEnumerable<ChargingStation>>(response,
                                                                          new Exception("Could not parse the returned GeoJSON!", e));
                }

                try
                {
                    stations = (JSON["features"] as JArray).SafeSelect(feature => feature as JObject);
                }
                catch (Exception e)
                {
                    return new HTTPResponse<IEnumerable<ChargingStation>>(response,
                                                                          new Exception("Could not extract the features from the returned GeoJSON!", e));
                }

                return new HTTPResponse<IEnumerable<ChargingStation>>(response,
                                                                      stations.SelectIgnoreErrors(ChargingStation.ParseJSON));

            }

            return new HTTPResponse<IEnumerable<ChargingStation>>(response, true);

        }

    }

}
