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
using SilverSpatial.Microsoft;

namespace SilverSpatial.Test.Microsoft
{
    [TestClass]
    public class MapShapeBaseExtensionsTest
    {
        readonly GeoPoint[] testGeos = new GeoPoint[] {
                                                     new GeoPoint(1, 2),
                                                     new GeoPoint(3, 4),
                                                     new GeoPoint(5, 6),
                                                     new GeoPoint(7, 8),
                                                     new GeoPoint(9, 10)
                                                };

        [TestMethod]
        public void SetLocations()
        {
            var shape = new MapPolygon();
            MapShapeBaseExtensions.SetLocations(shape, testGeos);
            Assert.IsTrue(testGeos.Length == shape.Locations.Count);
            for (var i = 0; i < testGeos.Length; i++)
            {
                Assert.IsTrue(testGeos[i].Latitude == shape.Locations[i].Latitude
                    && testGeos[i].Longitude == shape.Locations[i].Longitude);
            }
        }
    }
}