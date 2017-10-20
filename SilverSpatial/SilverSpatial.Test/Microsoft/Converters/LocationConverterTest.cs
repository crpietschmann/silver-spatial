// Copyright 2010 - Chris Pietschmann <http://pietschsoft.com>
//
// This file is part of SilverSpatial <http://silverspatial.codeplex.com>
//
// SilverSpatial is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation; either version 2.1 of the License, or
// (at your option) any later version.
//
// SilverSpatial is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using Microsoft.Maps.MapControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SilverSpatial.Test.Microsoft.Converters
{
    [TestClass]
    public class LocationConverterTest
    {
        readonly GeoPoint testGeo = new GeoPoint(-78, 40);
        readonly Location testLocation = new Location(-78, 40);

        [TestMethod]
        public void Convert()
        {
            var converter = new SilverSpatial.Microsoft.Converters.LocationConverter();
            var geo = converter.Convert(testGeo);
            Assert.IsTrue(geo.Latitude == testGeo.Latitude);
            Assert.IsTrue(geo.Longitude == testGeo.Longitude);
        }

        [TestMethod]
        public void ConvertBack()
        {
            var converter = new SilverSpatial.Microsoft.Converters.LocationConverter();
            var loc = converter.ConvertBack(testLocation);
            Assert.IsTrue(loc.Latitude == testLocation.Latitude);
            Assert.IsTrue(loc.Longitude == testLocation.Longitude);
        }
    }
}