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


// Reference SQL Spatial data types for the method names
// Initial purpose of project is to mirror their functionality
// http://64.4.11.252/en-us/library/bb933960%28SQL.100%29.aspx

using SilverSpatial.Converters;
using System;
namespace SilverSpatial
{
    public abstract class Geometry : IGeometry
    {
        #region "Abstract Properties"

        public abstract GeometryType GeometryType
        {
            get;
        }

        #endregion

        #region "Virtual Methods"

        public virtual int STSrid()
        {
            return 4326;
        }

        #endregion

        #region "Abstract Methods"

        /// <summary>
        /// Returns True if the specified IGeometry instance is contained within this Geometry instance
        /// </summary>
        /// <returns></returns>
        public abstract bool STContains(IGeometry geometry);

        /// <summary>
        /// Returns the total length of the Geometry instance.
        /// </summary>
        /// <returns></returns>
        public abstract double STLength();

        public abstract int STNumPoints();

        /// <summary>
        /// Returns the specified point in the IGeometry instance
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract IGeometry STPointN(int index);

        #endregion

        #region "Public Properties"

        /// <summary>
        /// Returns the Open Geospatial Consortium type name of the Geometry object
        /// </summary>
        public string STGeometryType
        {
            get
            {
                return this.GeometryType.ToString();
            }
        }

        /// <summary>
        /// Returns the number of Geometry object that make up the GeometryCollection. If the object is not a GeometryCollection then 1 is returned.
        /// </summary>
        public int STNumGeometries
        {
            get
            {
                var col = this as IGeometryCollection;
                if (col != null)
                {
                    return col.NumGeometries;
                }
                return 1;
            }
        }

        /// <summary>
        /// Returns the number of interior rings of a GeoPolygon instance
        /// </summary>
        public int? STNumInteriorRing
        {
            get
            {
                var poly = this as GeoPolygon;
                if (poly != null)
                {
                    return poly.InteriorRings.Count;
                }
                return null;
            }
        }

        /// <summary>
        /// The X coordinate of a GeoPoint object. Returns NULL if GeometryType is not a Point
        /// </summary>
        public double? STX
        {
            get
            {
                var point = this as GeoPoint;
                if (point != null)
                {
                    return point.Longitude;
                }
                return null;
            }
        }

        /// <summary>
        /// The Y coordinate of a GeoPoint object. Returns NULL if GeometryType is not a Point
        /// </summary>
        public double? STY
        {
            get
            {
                var point = this as GeoPoint;
                if (point != null)
                {
                    return point.Latitude;
                }
                return null;
            }
        }

        #endregion

        #region "Public Methods"

        /// <summary>
        /// Returns an Open Geospatial Consortium Well-Known-Binary representation of the Geometry object
        /// </summary>
        /// <returns></returns>
        public byte[] STAsBinary()
        {
            var converter = new WellKnownBinaryConverter();
            return converter.Convert(this);
        }

        /// <summary>
        /// Returns an Open Geospatial Consortium Well-Known-Text representation of the Geometry object
        /// </summary>
        /// <returns></returns>
        public string STAsText()
        {
            var converter = new WellKnownTextConverter();
            return converter.Convert(this);
        }

        /// <summary>
        /// Returns the IGeometry in the collection as the specified index. The first item in the collection has an index of 1
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IGeometry STGeometryN(int index)
        {
            if (index < 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            var geomCol = this as IGeometryCollection;
            if (geomCol != null && index <= geomCol.NumGeometries)
            {
                return (IGeometry)geomCol[index - 1];
            }

            var geoPoint = this as GeoPoint;
            if (geoPoint != null && index == 1)
            {
                return this;
            }

            return null;
        }

        /// <summary>
        /// Returns the start point of an IGeometry instance
        /// </summary>
        /// <returns></returns>
        public IGeometry STStartPoint()
        {
            return this.STPointN(1);
        }

        /// <summary>
        /// Returns True if this Geometry instance is completely within the specified Geometry instance
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public bool STWithin(IGeometry geometry)
        {
            return geometry.STContains(this);
        }

        /// <summary>
        /// Returns an Open Geospatial Consortium Well-Known-Text representation of the Geometry object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.STAsText();
        }

        /// <summary>
        /// Returns a Base64 Encoding of the Geometry objects Well-Known-Binary representation.
        /// </summary>
        /// <returns></returns>
        public string ToWKBBase64()
        {
            return System.Convert.ToBase64String(this.STAsBinary());
        }

        #endregion

        #region "Static Methods"

        /// <summary>
        /// Returns a Geometry object from the passed in Well-Known-Binary representation that is Base64 encoded.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Geometry FromWKBBase64(string bytes)
        {
            var byteArray = System.Convert.FromBase64String(bytes);
            return Parse(byteArray);
        }

        /// <summary>
        /// Returns a Geometry object from the passed in Open Geospatial Consortium Well-Known-Text representation
        /// </summary>
        /// <param name="wkt"></param>
        /// <returns></returns>
        public static Geometry Parse(string wkt)
        {
            var converter = new WellKnownTextConverter();
            return converter.ConvertBack(wkt);
        }

        /// <summary>
        /// Returns a Geometry object from the passed in Open Geospatial Consortium Well-Known-Binary representation
        /// </summary>
        /// <param name="wkb"></param>
        /// <returns></returns>
        public static Geometry Parse(byte[] wkb)
        {
            var converter = new WellKnownBinaryConverter();
            return converter.ConvertBack(wkb);
        }

        #endregion
    }
}
