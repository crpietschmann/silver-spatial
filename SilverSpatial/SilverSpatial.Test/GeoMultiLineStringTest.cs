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
    public class GeoMultiLineStringTest
    {
        readonly GeoMultiLineString testGeo = new GeoMultiLineString(new List<GeoLineString> {
            new GeoLineString(new List<GeoPoint>{ new GeoPoint(10, 10), new GeoPoint(20, 20) }),
            new GeoLineString(new List<GeoPoint>{ new GeoPoint(15, 15), new GeoPoint(30, 15) }),
            new GeoLineString(new List<GeoPoint>{ new GeoPoint(42, 15), new GeoPoint(30, 15) }),
        });
        readonly double testGeo_STLength = 41.142135623730951;
        readonly string testWKT = "MULTILINESTRING((10 10, 20 20), (15 15, 30 15), (42 15, 30 15))";
        readonly string testWKB_Base64 = "AQUAAAADAAAAAQIAAAACAAAAAAAAAAAAJEAAAAAAAAAkQAAAAAAAADRAAAAAAAAANEABAgAAAAIAAAAAAAAAAAAuQAAAAAAAAC5AAAAAAAAAPkAAAAAAAAAuQAECAAAAAgAAAAAAAAAAAEVAAAAAAAAALkAAAAAAAAA+QAAAAAAAAC5A";

        public static void AssertMatch(GeoMultiLineString first, GeoMultiLineString second)
        {
            Assert.IsTrue(first.NumGeometries == second.NumGeometries);

            for (var a = 0; a < first.NumGeometries; a++)
            {
                for (var i = 0; i < first.Geometries[a].NumGeometries; i++)
                {
                    Assert.IsTrue(first.Geometries[a].Geometries[i].Longitude == second.Geometries[a].Geometries[i].Longitude);
                    Assert.IsTrue(first.Geometries[a].Geometries[i].Latitude == second.Geometries[a].Geometries[i].Latitude);
                }
            }
        }

        [TestMethod]
        public void Parse01()
        {
            GeometryGenericTests.Parse01<GeoMultiLineString>(testGeo, testWKT, (g) =>
            {
                AssertMatch(g, testGeo);
            });
        }

        [TestMethod]
        public void STNumPoints()
        {
            Assert.IsTrue(testGeo.STNumPoints() == 6);
        }

        [TestMethod]
        public void FromWKBBase64()
        {
            GeometryGenericTests.FromWKBBase64<GeoMultiLineString>(testGeo, testWKB_Base64, (g) =>
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
            GeometryGenericTests.ToAndFromWKB<GeoMultiLineString>(testGeo, (g) =>
            {
                AssertMatch(g, testGeo);
            });
        }

        [TestMethod]
        public void STGeometryType()
        {
            Assert.IsTrue(testGeo.STGeometryType == "MultiLineString");
        }

        [TestMethod]
        public void STLength()
        {
            var val = testGeo.STLength();
            Assert.IsTrue(val == testGeo_STLength);
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
            Assert.IsTrue(testGeo.STGeometryN(4) == null);
        }

        [TestMethod]
        public void STPointN01()
        {
            var p = testGeo.STPointN(5) as GeoPoint;
            Assert.IsTrue(p.Longitude == 42 && p.Latitude == 15);
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
            Assert.IsTrue(testGeo.STStartPoint() == testGeo[0].Geometries[0]);
        }
    }
}