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
    public class GeoPoint : Geometry
    {
        public GeoPoint()
            : base()
        {
        }

        /// <summary>
        /// Creates a new GeoPoint with the specified location
        /// </summary>
        /// <param name="longitude">Longitude / X coordinate</param>
        /// <param name="latitude">Latitude / Y coordinate</param>
        public GeoPoint(double longitude, double latitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        /// <summary>
        /// Latitude / Y coordinate
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Longitude / X coordinate
        /// </summary>
        public double Longitude { get; set; }

        public override GeometryType GeometryType
        {
            get { return SilverSpatial.GeometryType.Point; }
        }

        public override bool STContains(IGeometry geometry)
        {
            if (this.STSrid() == geometry.STSrid() && this.STGeometryType == geometry.STGeometryType)
            {
                return (this.Latitude == geometry.STY && this.Longitude == geometry.STX);
            }
            return false;
        }

        public override double STLength()
        {
            return 0;
        }

        public override int STNumPoints()
        {
            return 1;
        }

        public override IGeometry STPointN(int index)
        {
            if (index < 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            else if (index == 1)
            {
                return this;
            }
            return null;
        }
    }
}
