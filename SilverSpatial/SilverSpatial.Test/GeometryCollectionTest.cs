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

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SilverSpatial.Test
{
    [TestClass]
    public class GeometryCollectionTest
    {
        readonly GeometryCollection testGeo = new GeometryCollection(new List<Geometry> {
            new GeoPoint(10,10),
            new GeoPoint(30,30),
            new GeoLineString(new List<GeoPoint>{
                new GeoPoint(15,15), new GeoPoint(20,20)
            })
        });
        readonly double testGeo_STLength = 7.0710678118654755;
        readonly string testWKT = "GEOMETRYCOLLECTION(POINT(10, 10), POINT(30, 30), LINESTRING(15 15, 20 20))";
        readonly string testWKB_Base64 = "AQcAAAADAAAAAQEAAAAAAAAAAAAkQAAAAAAAACRAAQEAAAAAAAAAAAA+QAAAAAAAAD5AAQIAAAACAAAAAAAAAAAALkAAAAAAAAAuQAAAAAAAADRAAAAAAAAANEA=";

        public static void AssertMatch(GeometryCollection first, GeometryCollection second)
        {
            Assert.IsTrue(first.NumGeometries == second.NumGeometries);

            for (var a = 0; a < first.NumGeometries; a++)
            {
                Assert.IsTrue(first.Geometries[a].STGeometryType == second.Geometries[a].STGeometryType);
                
                // TODO: FINISH THIS -- MAKE A HELPER CLASS THAT HAS METHODS FOR EACH TYPE THAT CALL EACH OTHER APPROPRIATELY
            }
        }

        [TestMethod]
        public void Parse01()
        {
            GeometryGenericTests.Parse01<GeometryCollection>(testGeo, testWKT, (g) =>
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
            GeometryGenericTests.FromWKBBase64<GeometryCollection>(testGeo, testWKB_Base64, (g) =>
            {
                AssertMatch(testGeo, g);
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
            GeometryGenericTests.ToAndFromWKB<GeometryCollection>(testGeo, (g) =>
            {
                AssertMatch(testGeo, g);
            });
        }

        [TestMethod]
        public void STGeometryType()
        {
            Assert.IsTrue(testGeo.STGeometryType == "GeometryCollection");
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
            Assert.IsTrue(testGeo.STGeometryN(3) == testGeo[2]);
        }

        [TestMethod]
        public void STGeometryN02()
        {
            Assert.IsTrue(testGeo.STGeometryN(4) == null);
        }

        [TestMethod]
        public void STPointN01()
        {
            var p = testGeo.STPointN(3) as GeoPoint;
            Assert.IsTrue(p.Longitude == 15 && p.Latitude == 15);
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
            Assert.IsTrue(testGeo.STStartPoint() == testGeo[0]);
        }
    }
}