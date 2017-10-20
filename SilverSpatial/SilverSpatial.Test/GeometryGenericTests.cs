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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SilverSpatial.Test
{
    public static class GeometryGenericTests
    {
        public static void Parse01<T>(T testObj, string wkt, Action<T> additionalAsserts = null) where T: Geometry
        {
            var g = SilverSpatial.Geometry.Parse(wkt);
            Assert.IsInstanceOfType(g, typeof(T));

            if (additionalAsserts != null)
            {
                additionalAsserts(g as T);
            }
        }

        public static void FromWKBBase64<T>(T testObj, string wkb_base64, Action<T> additionalAsserts = null) where T: Geometry
        {
            var g = SilverSpatial.Geometry.FromWKBBase64(wkb_base64);
            Assert.IsInstanceOfType(g, typeof(T));

            if (additionalAsserts != null)
            {
                additionalAsserts(g as T);
            }
        }

        public static void ToAndFromWKB<T>(T testObj, Action<T> asserts = null) where T : Geometry
        {
            var wkb = testObj.STAsBinary();
            var g = Geometry.Parse(wkb);

            Assert.IsTrue(testObj.STGeometryType == g.STGeometryType);

            if (asserts != null)
            {
                asserts(g as T);
            }
        }

        public static void ThrowsException<T>(Action action)
            where T : Exception
        {
            bool correctExceptionThrown = false;
            try
            {
                action.Invoke();
            }
            catch (T)
            {
                correctExceptionThrown = true; // This is supposed to throw this exception
            }

            if (correctExceptionThrown == false)
            {
                Assert.Fail();
            }
        }
    }
}
