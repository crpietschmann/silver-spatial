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

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SilverSpatial.Test
{
    [TestClass]
    public class GeoMultiPolygonTest
    {
        readonly GeoMultiPolygon testGeo = new GeoMultiPolygon(new List<GeoPolygon>{
            new GeoPolygon(new GeoLinearRing(new List<GeoPoint> {
                new GeoPoint(0,0),new GeoPoint(10,0),new GeoPoint(10,10), new GeoPoint(0,10), new GeoPoint(0,0)
            })),
            new GeoPolygon(new GeoLinearRing(new List<GeoPoint> {
                new GeoPoint(5,5),new GeoPoint(7,5),new GeoPoint(7,7),new GeoPoint(5,7),new GeoPoint(5,5)
            })),
        });
        readonly double testGeo_STLength = 48;
        readonly string testWKT = "MULTIPOLYGON(((0 0, 10 0, 10 10, 0 10, 0 0)),((5 5, 7 5, 7 7, 5 7, 5 5)))";
        readonly string testWKB_Base64 = "AQYAAAACAAAAAQMAAAABAAAABQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAkQAAAAAAAAAAAAAAAAAAAJEAAAAAAAAAkQAAAAAAAAAAAAAAAAAAAJEAAAAAAAAAAAAAAAAAAAAAAAQMAAAABAAAABQAAAAAAAAAAABRAAAAAAAAAFEAAAAAAAAAcQAAAAAAAABRAAAAAAAAAHEAAAAAAAAAcQAAAAAAAABRAAAAAAAAAHEAAAAAAAAAUQAAAAAAAABRA";

        public static void AssertMatch(GeoMultiPolygon first, GeoMultiPolygon second)
        {
            Assert.IsTrue(first.NumGeometries == second.NumGeometries);

            for (var a = 0; a < first.NumGeometries; a++)
            {
                for (var i = 0; i < first.Geometries[a].ExteriorRing.NumGeometries; i++)
                {
                    Assert.IsTrue(first.Geometries[a].ExteriorRing.Geometries[i].Longitude == second.Geometries[a].ExteriorRing.Geometries[i].Longitude);
                    Assert.IsTrue(first.Geometries[a].ExteriorRing.Geometries[i].Latitude == second.Geometries[a].ExteriorRing.Geometries[i].Latitude);
                }
            }
        }

        [TestMethod]
        public void Parse01()
        {
            GeometryGenericTests.Parse01<GeoMultiPolygon>(testGeo, testWKT, (g) =>
            {
                AssertMatch(g, testGeo);
            });
        }

        [TestMethod]
        public void STNumPoints()
        {
            Assert.IsTrue(testGeo.STNumPoints() == 10);
        }

        [TestMethod]
        public void FromWKBBase64()
        {
            GeometryGenericTests.FromWKBBase64<GeoMultiPolygon>(testGeo, testWKB_Base64, (g) =>
            {
                AssertMatch(g, testGeo);
            });
        }

        [TestMethod]
        public void ToWKBBase64()
        {
            var wkb = testGeo.ToWKBBase64();
            Assert.IsTrue(wkb == testWKB_Base64);
        }

        [TestMethod]
        public void STAsText()
        {
            var wkt = testGeo.STAsText();
            Assert.IsTrue(wkt == testWKT);
        }

        [TestMethod]
        public void WKB_TwoWay_Test()
        {
            GeometryGenericTests.ToAndFromWKB<GeoMultiPolygon>(testGeo, (g) =>
            {
                AssertMatch(g, testGeo);
            });
        }

        [TestMethod]
        public void STGeometryType()
        {
            Assert.IsTrue(testGeo.STGeometryType == "MultiPolygon");
        }

        [TestMethod]
        public void STLength()
        {
            Assert.IsTrue(testGeo.STLength() == testGeo_STLength);
        }

        [TestMethod]
        public void STGeometryN01()
        {
            Assert.IsTrue(testGeo.STGeometryN(2) == testGeo[1]);
        }

        [TestMethod]
        public void STGeometryN02()
        {
            Assert.IsTrue(testGeo.STGeometryN(3) == null);
        }

        [TestMethod]
        public void STPointN01()
        {
            var p = testGeo.STPointN(7) as GeoPoint;
            Assert.IsTrue(p.Longitude == 7 && p.Latitude == 5);
        }

        [TestMethod]
        public void STPointN02()
        {
            GeometryGenericTests.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                testGeo.STPointN(-1);
            });
        }

        [TestMethod]
        public void STPointN03()
        {
            Assert.IsNull(testGeo.STPointN(100));
        }

        [TestMethod]
        public void STStartPoint01()
        {
            Assert.IsTrue(testGeo.STStartPoint() == testGeo[0].ExteriorRing[0]);
        }
    }
}