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
    public class GeoMultiPoint : GeometryCollection<GeoPoint>
    {
        public GeoMultiPoint()
            : base()
        {
        }

        public GeoMultiPoint(IList<GeoPoint> geometries)
            : base(geometries)
        {
        }

        public override GeometryType GeometryType
        {
            get { return SilverSpatial.GeometryType.MultiPoint; }
        }

        public override IGeometry STPointN(int index)
        {
            if (index < 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (index > this.Geometries.Count)
            {
                return null;
            }
            return this.Geometries[index - 1];
        }
    }
}
