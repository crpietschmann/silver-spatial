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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Maps.MapControl;
using SilverSpatial.Microsoft.Converters;

namespace SilverSpatial.Test.Microsoft.Converters
{
    [TestClass]
    public class MapPolygonConverterTest
    {
        readonly GeoPolygon testGeo = new GeoPolygon()
        {
            ExteriorRing = new GeoLinearRing(new GeoPoint[] {
                new GeoPoint(1, 2),
                new GeoPoint(3, 4),
                new GeoPoint(5, 6),
                new GeoPoint(7, 8),
                new GeoPoint(9, 10)
            })
        };
        readonly MapPolygon testMapPolygon = new MapPolygon()
        {
            Locations = new LocationCollection() {
                new Location(1, 2),
                new Location(3, 4),
                new Location(5, 6),
                new Location(7, 8),
                new Location(9, 10),
            }
        };

        [TestMethod]
        public void Convert()
        {
            var poly = (new MapPolygonConverter()).Convert(testGeo);
            Assert.IsTrue(testGeo.ExteriorRing.Geometries.Count == poly.Locations.Count);
            for (var i = 0; i < testGeo.ExteriorRing.Geometries.Count; i++)
            {
                Assert.IsTrue(testGeo.ExteriorRing.Geometries[i].Latitude == poly.Locations[i].Latitude
                    && testGeo.ExteriorRing.Geometries[i].Longitude == poly.Locations[i].Longitude);
            }
        }

        [TestMethod]
        public void ConvertBack()
        {
            var geo = (new MapPolygonConverter()).ConvertBack(testMapPolygon);
            Assert.IsTrue(geo.ExteriorRing.Geometries.Count == testMapPolygon.Locations.Count);
            for (var i = 0; i < geo.ExteriorRing.Geometries.Count; i++)
            {
                Assert.IsTrue(geo.ExteriorRing.Geometries[i].Latitude == testMapPolygon.Locations[i].Latitude
                    && geo.ExteriorRing.Geometries[i].Longitude == testMapPolygon.Locations[i].Longitude);
            }
        }
    }
}