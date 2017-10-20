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
    public class GeometryCollection : GeometryCollection<Geometry>
    {
        public GeometryCollection() : base() { }

        public GeometryCollection(IList<Geometry> geometries) : base(geometries) { }

        public override GeometryType GeometryType
        {
            get { return SilverSpatial.GeometryType.GeometryCollection; }
        }

        public override IGeometry STPointN(int index)
        {
            if (index < 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            var indexLeft = index;
            foreach (var g in this.Geometries)
            {
                var numPoints = g.STNumPoints();
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
    }

    public abstract class GeometryCollection<T> : Geometry, IGeometryCollection<T> where T : Geometry
    {
        private IList<T> _Geometries = new List<T>();

        public GeometryCollection()
        {

        }

        public GeometryCollection(IList<T> geometries)
        {
            this._Geometries = geometries;
        }

        public abstract override GeometryType GeometryType
        {
            get;
        }

        public abstract override IGeometry STPointN(int index);

        public override bool STContains(IGeometry geometry)
        {
            foreach (var g in this)
            {
                if (g.STContains(geometry))
                {
                    return true;
                }
            }
            return false;
        }

        public override double STLength()
        {
            double totalLength = 0;
            foreach (var g in this)
            {
                totalLength += g.STLength();
            }
            return totalLength;
        }

        public override int STNumPoints()
        {
            var pointCount = 0;
            foreach (var item in this.Geometries)
            {
                pointCount += item.STNumPoints();
            }
            return pointCount;
        }

        #region IGeometryCollection<T> Members

        public T this[int index]
        {
            get { return this._Geometries[index]; }
        }

        object IGeometryCollection.this[int index]
        {
            get
            {
                return this[index];
            }
        }

        public System.Collections.Generic.IList<T> Geometries
        {
            get { return this._Geometries; }
        }

        public int NumGeometries
        {
            get
            {
                return this._Geometries.Count;
            }
        }

        #endregion

        #region IEnumerable<T> Members

        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            foreach (var obj in this.Geometries)
            {
                yield return obj;
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
