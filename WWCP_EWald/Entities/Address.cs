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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.EWald
{

    public class Address
    {

        public String   City     { get; }
        public String   Street   { get; }
        public String   Zip      { get; }
        public Country  Country  { get; }

        public Address(String   City,
                       String   Street,
                       String   Zip,
                       Country  Country)
        {

            this.City     = City;
            this.Street   = Street;
            this.Zip      = Zip;
            this.Country  = Country;

        }


        public static Address ParseJSON(JObject feature)
        {

            return new Address(feature["city"].  Value<String>(),
                               feature["street"].Value<String>(),
                               feature["zip"].   Value<String>(),
                               Country.ParseCountryName(feature["country"].Value<String>()));

        }

    }

}
