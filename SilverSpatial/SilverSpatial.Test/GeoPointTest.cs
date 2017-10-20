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
using System.Collections.Generic;
using System;

namespace SilverSpatial.Test
{
    [TestClass]
    public class GeoPointTest
    {
        readonly GeoPoint testGeo = new GeoPoint(-78, 40);
        readonly string testWKT = "POINT(-78, 40)";
        readonly string testWKB_Base64 = "AQEAAAAAAAAAAIBTwAAAAAAAAERA";

        readonly GeoPolygon testPoly = new GeoPolygon(new GeoLinearRing(new List<GeoPoint>() {
            new GeoPoint(-77, 39),
            new GeoPoint(-79, 39),
            new GeoPoint(-79, 41),
            new GeoPoint(-77, 41)
        }));

        public static void AssertMatch(GeoPoint first, GeoPoint second)
        {
            Assert.IsTrue(first.Longitude == second.Longitude);
            Assert.IsTrue(first.Latitude == second.Latitude);
        }

        [TestMethod]
        public void Parse01()
        {
            GeometryGenericTests.Parse01<GeoPoint>(testGeo, testWKT, (g) =>
            {
                AssertMatch(g, testGeo);
            });
        }

        [TestMethod]
        public void STNumPoints()
        {
            Assert.IsTrue(testGeo.STNumPoints() == 1);
        }

        [TestMethod]
        public void FromWKBBase64()
        {
            GeometryGenericTests.FromWKBBase64<GeoPoint>(testGeo, testWKB_Base64, (g) =>
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
            GeometryGenericTests.ToAndFromWKB<GeoPoint>(testGeo, (g) =>
            {
                AssertMatch(g, testGeo);
            });
        }

        [TestMethod]
        public void STGeometryType()
        {
            Assert.IsTrue(testGeo.STGeometryType == "Point");
        }

        [TestMethod]
        public void STLength()
        {
            Assert.IsTrue(testGeo.STLength() == 0);
        }

        [TestMethod]
        public void STSrid()
        {
            Assert.IsTrue(testGeo.STSrid() == 4326);
        }

        [TestMethod]
        public void STContains01()
        {
            Assert.IsTrue(testGeo.STContains(testGeo) == true);
        }

        [TestMethod]
        public void STContains02()
        {
            Assert.IsTrue(testGeo.STContains(new GeoPoint(0,0)) == false);
        }

        [TestMethod]
        public void STContains03()
        {
            Assert.IsTrue(testGeo.STContains(testPoly) == false);
        }

        [TestMethod]
        public void STWithin01()
        {
            Assert.IsTrue(testGeo.STWithin(testGeo) == true);
        }

        [TestMethod]
        public void STWithin02()
        {
            Assert.IsTrue(testGeo.STWithin(new GeoPoint(0, 0)) == false);
        }

        [TestMethod]
        public void STWithin03()
        {
            Assert.IsTrue(testGeo.STWithin(testPoly) == false);
        }

        [TestMethod]
        public void STGeometryN01()
        {
            Assert.IsTrue(testGeo.STGeometryN(1) == testGeo);
        }

        [TestMethod]
        public void STGeometryN02()
        {
            Assert.IsTrue(testGeo.STGeometryN(2) == null);
        }

        [TestMethod]
        public void STGeometryN03()
        {
            GeometryGenericTests.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                testGeo.STGeometryN(-1);
            });
        }

        [TestMethod]
        public void STNumInteriorRing01()
        {
            Assert.IsNull(testGeo.STNumInteriorRing);
        }

        [TestMethod]
        public void STPointN01()
        {
            Assert.IsTrue(testGeo.STPointN(1) == testGeo);
        }

        [TestMethod]
        public void STPointN02()
        {
            Assert.IsNull(testGeo.STPointN(2));
        }

        [TestMethod]
        public void STPointN03()
        {
            GeometryGenericTests.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                testGeo.STPointN(-1);
            });
        }

        [TestMethod]
        public void STStartPoint01()
        {
            Assert.IsTrue(testGeo.STStartPoint() == testGeo);
        }
    }
}