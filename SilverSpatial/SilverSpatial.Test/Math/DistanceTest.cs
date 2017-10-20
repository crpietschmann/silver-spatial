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

namespace SilverSpatial.Test.Math
{
    [TestClass]
    public class DistanceTest
    {
        private GeoPoint point1 = new GeoPoint(-87.9081344604492, 43.0420453718909); // Milwaukee, WI
        private GeoPoint point2 = new GeoPoint(-88.1817626953125, 43.4072923057905); // West Bend, WI
        private const double point1DistanceMiles = 28.731398772617897;
        private const double point1DistanceKilometers = 46.241864505879207;

        [TestMethod]
        public void Distance01()
        {
            var p1 = new GeoPoint(0, 0);
            var p2 = new GeoPoint(1, 0);

            var distance = SilverSpatial.Math.Distance(p1, p2);
            Assert.IsTrue(distance == 1);
        }

        [TestMethod]
        public void Distance02()
        {
            var p1 = new GeoPoint(0, 1);
            var p2 = new GeoPoint(1, 0);

            var distance = SilverSpatial.Math.Distance(p1, p2);
            Assert.IsTrue(distance == 1.4142135623730951);
        }

        [TestMethod]
        public void Distance03()
        {
            var p1 = new GeoPoint(0, 6);
            var p2 = new GeoPoint(0, 0);

            var distance = SilverSpatial.Math.Distance(p1, p2);
            Assert.IsTrue(distance == 6);
        }

        [TestMethod]
        public void Distance04()
        {
            var p1 = new GeoPoint(0, 2);
            var p2 = new GeoPoint(2, 0);

            var distance = SilverSpatial.Math.Distance(p1, p2);
            Assert.IsTrue(distance == (1.4142135623730951 * 2));
        }

        [TestMethod]
        public void Distance05()
        {
            var distance = SilverSpatial.Math.Distance(point1, point2, SilverSpatial.Math.UnitOfMeasure.Miles);
            Assert.IsTrue(distance == point1DistanceMiles);
        }

        [TestMethod]
        public void Distance06()
        {
            var distance = SilverSpatial.Math.Distance(point1, point2, SilverSpatial.Math.UnitOfMeasure.Kilometers);
            Assert.IsTrue(distance == point1DistanceKilometers);
        }
    }
}
