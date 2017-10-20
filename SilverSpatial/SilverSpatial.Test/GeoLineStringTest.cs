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
    public class GeoLineStringTest
    {
        readonly GeoLineString testGeo = new GeoLineString(new List<GeoPoint> {
            new GeoPoint(0,0), new GeoPoint(10,10), new GeoPoint(20,25), new GeoPoint(50, 60)
        });
        readonly double testGeo_STLength = 78.267614287515329;
        readonly string testWKT = "LINESTRING(0 0, 10 10, 20 25, 50 60)";
        readonly string testWKB_Base64 = "AQIAAAAEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACRAAAAAAAAAJEAAAAAAAAA0QAAAAAAAADlAAAAAAAAASUAAAAAAAABOQA==";

        public static void AssertMatch(GeoLineString first, GeoLineString second)
        {
            Assert.IsTrue(first.NumGeometries == second.NumGeometries);

            for (var i = 0; i < first.NumGeometries; i++)
            {
                Assert.IsTrue(first.Geometries[i].Longitude == second.Geometries[i].Longitude);
                Assert.IsTrue(first.Geometries[i].Latitude == second.Geometries[i].Latitude);
            }
        }

        [TestMethod]
        public void Parse01()
        {
            GeometryGenericTests.Parse01<GeoLineString>(testGeo, testWKT, (g) =>
            {
                AssertMatch(g, testGeo);
            });
        }

        [TestMethod]
        public void STNumPoints()
        {
            Assert.IsTrue(testGeo.STNumPoints() == 4);
        }

        [TestMethod]
        public void FromWKBBase64()
        {
            GeometryGenericTests.FromWKBBase64<GeoLineString>(testGeo, testWKB_Base64, (g) =>
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
            GeometryGenericTests.ToAndFromWKB<GeoLineString>(testGeo, (g) =>
            {
                AssertMatch(g, testGeo);
            });
        }

        [TestMethod]
        public void STGeometryType()
        {
            Assert.IsTrue(testGeo.STGeometryType == "LineString");
        }

        [TestMethod]
        public void STLength()
        {
            var len = testGeo.STLength();
            Assert.IsTrue(len == testGeo_STLength);
        }

        [TestMethod]
        public void STContains01()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void STWithin01()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void STGeometryN01()
        {
            Assert.IsTrue(testGeo.STGeometryN(2) == testGeo[1]);
        }

        [TestMethod]
        public void STGeometryN02()
        {
            Assert.IsTrue(testGeo.STGeometryN(12) == null);
        }

        [TestMethod]
        public void STPointN01()
        {
            var p = testGeo.STPointN(3) as GeoPoint;
            Assert.IsTrue(p.Longitude == 20 && p.Latitude == 25);
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
    }
}