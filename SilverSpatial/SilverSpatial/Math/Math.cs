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

namespace SilverSpatial
{
    public static partial class Math
    {
        /// <summary>
        /// Gets the difference between two radians. Subtracts r2 from r1.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        public static double DiffRadians(double r1, double r2)
        {
            return r2 - r1;
        }

        /// <summary>
        /// Converts Radians to Degrees
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static double ToDegrees(double radian)
        {
            return radian * 180 / System.Math.PI;
        }
        
        /// <summary>
        /// Converts Degrees to Radians
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static double ToRadians(double degree)
        {
            return degree * (System.Math.PI / 180);
        }

        /// <summary>
        /// Converts Miles to Kilometers
        /// </summary>
        /// <param name="miles"></param>
        /// <returns></returns>
        public static double ToKilometers(double miles)
        {
            return miles * 1.609344;
        }

        /// <summary>
        /// Converts Kilometers to Miles
        /// </summary>
        /// <param name="kilometers"></param>
        /// <returns></returns>
        public static double ToMiles(double kilometers)
        {
            return kilometers * 0.621371192;
        }

        public enum UnitOfMeasure : int
        {
            Miles = 0,
            Kilometers = 1
        }

        public static class EarthRadius
        {
            public const double Miles = 3956.0;
            public const double Kilometers = 6367.0;
        }
    }
}
