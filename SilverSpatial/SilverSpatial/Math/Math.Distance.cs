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
namespace SilverSpatial
{
    public static partial class Math
    {
        /// <summary>
        /// Calculates the distance between two GeoPoint objects.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double Distance(GeoPoint p1, GeoPoint p2)
        {
            return System.Math.Sqrt(
                System.Math.Pow((p2.Longitude - p1.Longitude), 2.0) + 
                System.Math.Pow((p2.Latitude - p1.Latitude), 2.0)
                );
        }

        /// <summary>
        /// Calculates the distance between two GeoPoint objects using the specified UnitOfMeasure (Miles/Kilometers based on Earth's radius)
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static double Distance(GeoPoint p1, GeoPoint p2, UnitOfMeasure unit)
        {
            if (p1 == null) { throw new ArgumentNullException("p1"); }
            if (p2 == null) { throw new ArgumentNullException("p2"); }
            if (!Enum.IsDefined(typeof(UnitOfMeasure), unit)) { throw new ArgumentOutOfRangeException("unit"); }

            double earthRadius = EarthRadius.Miles;
            if (unit == UnitOfMeasure.Kilometers)
            {
                earthRadius = EarthRadius.Kilometers;
            }

            return earthRadius * 2 * System.Math.Asin(
                System.Math.Min(1,
                    System.Math.Sqrt(
                        (
                            System.Math.Pow(System.Math.Sin((Math.DiffRadians(Math.ToRadians(p1.Latitude), Math.ToRadians(p2.Latitude))) / 2.0), 2.0) +
                            System.Math.Cos(Math.ToRadians(p1.Latitude)) * System.Math.Cos(Math.ToRadians(p2.Latitude)) *
                            System.Math.Pow(System.Math.Sin((Math.DiffRadians(Math.ToRadians(p1.Longitude), Math.ToRadians(p2.Longitude))) / 2.0), 2.0)
                        )
                   )
               )
            );
        }
    }
}
