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
    public class GeoPolygonTest
    {
        readonly GeoPolygon testGeo = new GeoPolygon(
            new GeoLinearRing(new List<GeoPoint>{
                new GeoPoint(0,0), new GeoPoint(10,0), new GeoPoint(10,10), new GeoPoint(0,10), new GeoPoint(0,0)
            }),
            new List<GeoLinearRing> {
                new GeoLinearRing(new List<GeoPoint>{ new GeoPoint(5,5),new GeoPoint(7,5),new GeoPoint(7,7), new GeoPoint(5,7), new GeoPoint(5,5) }),
                new GeoLinearRing(new List<GeoPoint>{ new GeoPoint(1,2), new GeoPoint(3,4), new GeoPoint(5,6), new GeoPoint(7,8), new GeoPoint(1, 2) })
        });
        readonly double testGeo_STLength = 40.0; //49.4142135623731;
        readonly string testWKT = "POLYGON((0 0, 10 0, 10 10, 0 10, 0 0),(5 5, 7 5, 7 7, 5 7, 5 5),(1 2, 3 4, 5 6, 7 8, 1 2))";
        readonly string testWKB_Base64 = "AQMAAAADAAAABQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAkQAAAAAAAAAAAAAAAAAAAJEAAAAAAAAAkQAAAAAAAAAAAAAAAAAAAJEAAAAAAAAAAAAAAAAAAAAAABQAAAAAAAAAAABRAAAAAAAAAFEAAAAAAAAAcQAAAAAAAABRAAAAAAAAAHEAAAAAAAAAcQAAAAAAAABRAAAAAAAAAHEAAAAAAAAAUQAAAAAAAABRABQAAAAAAAAAAAPA/AAAAAAAAAEAAAAAAAAAIQAAAAAAAABBAAAAAAAAAFEAAAAAAAAAYQAAAAAAAABxAAAAAAAAAIEAAAAAAAADwPwAAAAAAAABA";


        public static void AssertMatch(GeoPolygon first, GeoPolygon second){
             Assert.IsTrue(first.ExteriorRing.NumGeometries == second.ExteriorRing.NumGeometries);
            Assert.IsTrue(first.InteriorRings.Count == second.InteriorRings.Count);

            for (var a = 0; a < first.ExteriorRing.NumGeometries; a++)
            {
                Assert.IsTrue(first.ExteriorRing.Geometries[a].Longitude == second.ExteriorRing.Geometries[a].Longitude);
                Assert.IsTrue(first.ExteriorRing.Geometries[a].Latitude == second.ExteriorRing.Geometries[a].Latitude);
            }

            for (var a = 0; a < first.InteriorRings.Count; a++)
            {
                for (var i = 0; i < first.InteriorRings[a].NumGeometries; i++)
                {
                    Assert.IsTrue(first.InteriorRings[a].NumGeometries == second.InteriorRings[a].NumGeometries);
                    Assert.IsTrue(first.InteriorRings[a].Geometries[i].Longitude == second.InteriorRings[a].Geometries[i].Longitude);
                    Assert.IsTrue(first.InteriorRings[a].Geometries[i].Latitude == second.InteriorRings[a].Geometries[i].Latitude);
                }
            }
        }


        [TestMethod]
        public void Parse01()
        {
            GeometryGenericTests.Parse01<GeoPolygon>(testGeo, testWKT, (g) =>
            {
                AssertMatch(testGeo, g);
            });
        }

        [TestMethod]
        public void STNumPoints()
        {
            Assert.IsTrue(testGeo.STNumPoints() == 15);
        }

        [TestMethod]
        public void FromWKBBase64()
        {
            GeometryGenericTests.FromWKBBase64<GeoPolygon>(testGeo, testWKB_Base64, (g) =>
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
            GeometryGenericTests.ToAndFromWKB<GeoPolygon>(testGeo, (g) =>
            {
                AssertMatch(testGeo, g);
            });
        }

        [TestMethod]
        public void STGeometryType()
        {
            Assert.IsTrue(testGeo.STGeometryType == "Polygon");
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
            //Assert.IsTrue(false);
            throw new NotImplementedException();
        }

        [TestMethod]
        public void STWithin01()
        {
            //Assert.IsTrue(false);
            throw new NotImplementedException();
        }

        [TestMethod]
        public void STGeometryN01()
        {
            Assert.IsTrue(testGeo.STGeometryN(2) == null);
        }

        [TestMethod]
        public void STNumInteriorRing01()
        {
            Assert.IsTrue(testGeo.STNumInteriorRing == 2);
        }

        [TestMethod]
        public void STPointN01()
        {
            var p = testGeo.STPointN(2) as GeoPoint;
            Assert.IsTrue(p.Longitude == 10 && p.Latitude == 0);
        }

        [TestMethod]
        public void STPointN02()
        {
            var p = testGeo.STPointN(12) as GeoPoint;
            Assert.IsTrue(p.Longitude == 3 && p.Latitude == 4);
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
        public void STPointN04()
        {
            Assert.IsNull(testGeo.STPointN(100));
        }

        [TestMethod]
        public void STStartPoint01()
        {
            Assert.IsTrue(testGeo.STStartPoint() == testGeo.ExteriorRing[0]);
        }
    }
}