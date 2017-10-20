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
    public class GeoPolygon : Geometry
    {
        private GeoLinearRing _ExteriorRing;
        private IList<GeoLinearRing> _InteriorRings = new List<GeoLinearRing>();

        public GeoPolygon()
            : base()
        {
        }

        public GeoPolygon(GeoLinearRing exteriorRing)
        {
            this.ExteriorRing = exteriorRing;
        }

        public GeoPolygon(GeoLinearRing exteriorRing, IList<GeoLinearRing> interiorRings)
            : this(exteriorRing)
        {
            if (interiorRings == null)
            {
                throw new ArgumentNullException("interiorRings");
            }

            this._InteriorRings = interiorRings;
        }

        public override GeometryType GeometryType
        {
            get { return SilverSpatial.GeometryType.Polygon;}
        }

        #region "Properties"

        public GeoLinearRing ExteriorRing
        {
            get
            {
                return this._ExteriorRing;
            }
            set
            {
                this._ExteriorRing = value;
            }
        }

        public IList<GeoLinearRing> InteriorRings
        {
            get
            {
                return this._InteriorRings;
            }
        }

        #endregion

        public override double STLength()
        {
            double totalLength = this.ExteriorRing.STLength();

            totalLength += Math.Distance(this.ExteriorRing.Geometries[this.ExteriorRing.Geometries.Count - 1], this.ExteriorRing.Geometries[0]);

            return totalLength;
        }

        public override int STNumPoints()
        {
            var pointCount = this.ExteriorRing.STNumPoints();

            foreach (var item in this.InteriorRings)
            {
                pointCount += item.STNumPoints();
            }

            return pointCount;
        }

        public override IGeometry STPointN(int index)
        {
            if (index < 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            var indexLeft = index;
            
            var numPoints = this.ExteriorRing.STNumPoints();
            if (indexLeft > numPoints)
            {
                indexLeft -= numPoints;
            }
            else
            {
                return this.ExteriorRing.STPointN(indexLeft);
            }

            foreach (var g in this.InteriorRings)
            {
                numPoints = g.STNumPoints();
                if (numPoints < indexLeft)
                {
                    indexLeft -= numPoints;
                }
                else
                {
                    return g.STPointN(indexLeft);
                }
            }

            return null;
        }

        public override bool STContains(IGeometry geometry)
        {
            throw new NotImplementedException();
        }
    }
}
