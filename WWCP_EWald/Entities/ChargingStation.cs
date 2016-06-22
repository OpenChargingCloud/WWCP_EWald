/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP E-Wald <https://github.com/OpenChargingCloud/WWCP_EWald>
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
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.EWald
{

    public class ChargingStation
    {

        public String                  Status               { get; }
        public String                  MarkerColor          { get; }
        public String                  UUID                 { get; }
        public String                  OnlineStatus         { get; }
        public String                  Title                { get; }
        public Operator                Operator             { get; }
        public IEnumerable<Connector>  Connectors           { get; }
        public Byte                    ConnectorsFree       { get; }
        public Address                 Address              { get; }
        public Byte                    ConnectorsFaulted    { get; }
        public String                  StatusIcon           { get; }
        public String                  Model                { get; }
        public String                  Manufacturer         { get; }
        public String                  StatusColor          { get; }
        public Byte                    ConnectorsTotal      { get; }
        public GeoCoordinate           GeoCoordinate        { get; }

        public ChargingStation(String                  Status,
                               String                  MarkerColor,
                               String                  UUID,
                               String                  OnlineStatus,
                               String                  Title,
                               Operator                Operator,
                               IEnumerable<Connector>  Connectors,
                               Byte                    ConnectorsFree,
                               Address                 Address,
                               Byte                    ConnectorsFaulted,
                               String                  StatusIcon,
                               String                  Model,
                               String                  Manufacturer,
                               String                  StatusColor,
                               Byte                    ConnectorsTotal,
                               GeoCoordinate           GeoCoordinate)
        {

            this.Status             = Status;
            this.MarkerColor        = MarkerColor;
            this.UUID               = UUID;
            this.OnlineStatus       = OnlineStatus;
            this.Title              = Title;
            this.Operator           = Operator;
            this.Connectors         = Connectors;
            this.ConnectorsFree     = ConnectorsFree;
            this.Address            = Address;
            this.ConnectorsFaulted  = ConnectorsFaulted;
            this.StatusIcon         = StatusIcon;
            this.Model              = Model;
            this.Manufacturer       = Manufacturer;
            this.StatusColor        = StatusColor;
            this.ConnectorsTotal    = ConnectorsTotal;
            this.GeoCoordinate      = GeoCoordinate;

        }


        public static ChargingStation ParseJSON(JObject JSON)
        {

            // {
            //    "status":                 "NoError",
            //    "marker-color":           "#44B6AE",
            //    "uuid":                   "5e220377-d179-11e4-a755-005056b332c9",
            //    "online_status":          "online",
            //    "title":                  "Tittling",
            //    "operator":               { ... },
            //    "connectors":             [ ... ],
            //    "connectors_free":        4,
            //    "address":                { ... },
            //    "connectors_faulted":     0,
            //    "status_icon":            "fi-electric-station",
            //    "model":                  "LS-Public 12 2x2 (r1)",
            //    "manufacturer":           "Technagon GmbH",
            //    "status_color":           "#44B6AE",
            //    "connectors_total":       4
            //  }
            //},

            var properties = JSON["properties"];

            return new ChargingStation(Status:             properties["status"].            Value<String>(),
                                       MarkerColor:        properties["marker-color"].      Value<String>(),
                                       UUID:               properties["uuid"].              Value<String>(),
                                       OnlineStatus:       properties["online_status"].     Value<String>(),
                                       Title:              properties["title"].             Value<String>(),
                                       Operator:           new Operator(properties["operator"]["name"].Value<String>()),
                                       Connectors:         properties["connectors"].SelectIgnoreErrors(connector => Connector.ParseJSON(connector as JObject)),
                                       ConnectorsFree:     properties["connectors_free"].   Value<Byte>(),
                                       Address:            Address.ParseJSON(properties["address"] as JObject),
                                       ConnectorsFaulted:  properties["connectors_faulted"].Value<Byte>(),
                                       StatusIcon:         properties["status_icon"].       Value<String>(),
                                       Model:              properties["model"].             Value<String>(),
                                       Manufacturer:       properties["manufacturer"].      Value<String>(),
                                       StatusColor:        properties["status_color"].      Value<String>(),
                                       ConnectorsTotal:    properties["connectors_total"].  Value<Byte>(),
                                       GeoCoordinate:      new GeoCoordinate(
                                                               Latitude. Parse(JSON["geometry"]["coordinates"][1].Value<String>()),
                                                               Longitude.Parse(JSON["geometry"]["coordinates"][0].Value<String>())
                                                           )
                                      );

        }

    }

}
