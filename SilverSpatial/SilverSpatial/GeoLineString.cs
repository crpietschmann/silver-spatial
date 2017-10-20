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
using System;
namespace SilverSpatial
{
    public class GeoLineString : GeometryCollection<GeoPoint>
    {
        public GeoLineString()
            : base()
        {
        }

        public GeoLineString(IList<GeoPoint> geometries)
            : base(geometries)
        {
        }

        public override GeometryType GeometryType
        {
            get { return SilverSpatial.GeometryType.LineString; }
        }

        public override double STLength()
        {
            double totalLength = 0;
            for (var i = 1; i < this.Geometries.Count; i++)
            {
                totalLength += Math.Distance(this.Geometries[i - 1], this.Geometries[i]);
            }
            return totalLength;
        }

        public override bool STContains(IGeometry geometry)
        {
            //return false;
            throw new NotImplementedException();
        }

        public override IGeometry STPointN(int index)
        {
            if (index < 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            if (index > this.STNumPoints()) {
                return null;
            }

            return this.Geometries[index - 1];
        }
    }
}
