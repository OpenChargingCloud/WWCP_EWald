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

using Newtonsoft.Json.Linq;

#endregion

namespace org.GraphDefined.WWCP.EWald
{

    public class Connector
    {

        public String         Id                { get; }
        public String         Label             { get; }
        public EVSE_Id        EVSEId            { get; }
        public String         Status            { get; }

        public Byte           MaxPhase          { get; }
        public String         MaxPowerString    { get; }
        public PlugTypes      Socket            { get; }
        public UInt16         MaxVolt           { get; }
        public ChargingModes  ChargeMode        { get; }
        public String         SocketLabel       { get; }
        public UInt32         MaxPower          { get; }
        public UInt16         MaxAmpere         { get; }


        public Connector(String         Id,
                         String         Label,
                         EVSE_Id        EVSEId,
                         String         Status,

                         Byte           MaxPhase,
                         String         MaxPowerString,
                         PlugTypes      Socket,
                         UInt16         MaxVolt,
                         ChargingModes  ChargeMode,
                         String         SocketLabel,
                         UInt32         MaxPower,
                         UInt16         MaxAmpere)

        {

            this.EVSEId          = EVSEId;
            this.Status          = Status;
            this.Id              = Id;
            this.Label           = Label;

            this.MaxPhase        = MaxPhase;
            this.MaxPowerString  = MaxPowerString;
            this.Socket          = Socket;
            this.MaxVolt         = MaxVolt;
            this.ChargeMode      = ChargeMode;
            this.SocketLabel     = SocketLabel;
            this.MaxPower        = MaxPower;
            this.MaxAmpere       = MaxAmpere;

        }


        private static ChargingModes ParseChargingMode(String ChargingMode)
        {

            switch (ChargingMode)
            {

                case "Mode 1":
                    return ChargingModes.Mode_1;

                case "Mode 2":
                    return ChargingModes.Mode_2;

                case "Mode 3":
                    return ChargingModes.Mode_3;

                case "Mode 4":
                    return ChargingModes.Mode_4;

                case "CHAdeMO":
                    return ChargingModes.CHAdeMO;

                default:
                    return ChargingModes.Unspecified;

            }

        }

        private static PlugTypes ParsePlugType(String PlugType)
        {

            switch (PlugType)
            {

                case "Type 2":
                    return PlugTypes.Type2Outlet;

                case "Schuko CEE7/4":
                    return PlugTypes.TypeFSchuko;

                default:
                    return PlugTypes.Unspecified;

            }

        }

        public static Connector ParseJSON(JObject JSON)
        {

            // {
            //   "evseid":  "DE*666*E1000101",
            //   "status":  "Available",
            //   "type": {
            //       "max_phase":         3,
            //       "max_power_string":  "22,1 kW",
            //       "socket":            "Type 2",
            //       "max_volt":          230,
            //       "charge_mode":       "Mode 3",
            //       "label":             "Type 2 / 22 kW",
            //       "max_power":         22080,
            //       "max_ampere":        32
            //   },
            //   "id":     480,
            //   "label":  "Typ 2 links"
            // }

            return new Connector(                  JSON["id"].                      Value<String>(),
                                                   JSON["label"].                   Value<String>(),
                                 EVSE_Id.Parse(    JSON["evseid"].                  Value<String>()),
                                                   JSON["status"].                  Value<String>(),
                                                   JSON["type"]["max_phase"].       Value<Byte>(),
                                                   JSON["type"]["max_power_string"].Value<String>(),
                                 ParsePlugType(    JSON["type"]["socket"].          Value<String>()),
                                                   JSON["type"]["max_volt"].        Value<UInt16>(),
                                 ParseChargingMode(JSON["type"]["charge_mode"].     Value<String>()),
                                                   JSON["type"]["label"].           Value<String>(),
                                                   JSON["type"]["max_power"].       Value<UInt32>(),
                                                   JSON["type"]["max_ampere"].      Value<UInt16>());

        }

    }

}
