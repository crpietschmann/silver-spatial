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
using System.Collections.Generic;
using System.Text;

// Well-Known-Text Documentation/Specifications
// http://dev.mysql.com/doc/refman/5.0/en/gis-wkt-format.html
// http://www.opengeospatial.org/standards/sfa
// http://en.wikipedia.org/wiki/Well-known_text


namespace SilverSpatial.Converters
{
    public class WellKnownTextConverter : ValueConverterBase<Geometry, string>
    {
        public override string Convert(Geometry obj)
        {
            StringBuilder sb = null;

            switch (obj.GeometryType)
            {
                case GeometryType.LineString:
                    var ls = (GeoLineString)obj;
                    return string.Format("LINESTRING({0})", PointCollectionToString(ls.Geometries));

                case GeometryType.Point:
                    var p = (GeoPoint)obj;
                    return string.Format("POINT({0}, {1})", p.Longitude.ToString(), p.Latitude.ToString());

                case GeometryType.MultiPoint:
                    var mp = (GeoMultiPoint)obj;
                    return string.Format("MULTIPOINT({0})", PointCollectionToString(mp.Geometries));

                case GeometryType.MultiLineString:
                    var ml = (GeoMultiLineString)obj;
                    return string.Format("MULTILINESTRING({0})", GeoLineStringCollectionToString(ml.Geometries));

                case GeometryType.Polygon:
                    var poly = (GeoPolygon)obj;
                    //return string.Format("POLYGON(({0}))", PointCollectionToString(poly.ExteriorRing.Geometries));

                    sb = new StringBuilder("POLYGON(");
                    sb.AppendFormat("({0})", PointCollectionToString(poly.ExteriorRing.Geometries));

                    for(var i = 0; i < poly.InteriorRings.Count; i++)
                    {
                        sb.AppendFormat(",({0})", PointCollectionToString(poly.InteriorRings[i].Geometries));
                    }

                    sb.Append(")");
                    return sb.ToString();
                
                case GeometryType.MultiPolygon:
                    var mpoly = (GeoMultiPolygon)obj;
                    return string.Format("MULTIPOLYGON({0})", PolygonCollectionToString(mpoly.Geometries));

                case GeometryType.GeometryCollection:
                    var isFirst = true;
                    var gc = (GeometryCollection)obj;
                    
                    sb = new StringBuilder("GEOMETRYCOLLECTION(");
                    
                    foreach (var g in gc.Geometries)
                    {
                        if (!isFirst)
                        {
                            sb.Append(", ");
                        }
                        else
                        {
                            isFirst = false;
                        }
                        sb.Append(this.Convert(g));
                    }

                    sb.Append(")");
                    return sb.ToString();

                default:
                    // Unrecognized GeometryType
                    throw new NotSupportedException(string.Format("Unsupported GeometryType ({0})", obj.GeometryType));
            }
        }

        public override Geometry ConvertBack(string obj)
        {
            if (obj.StartsWith("POINT("))
            {
                var pointStr = obj.Substring(6, obj.Length - 7);
                var point = pointStr.Split(',');
                if (point.Length == 2)
                {
                    return new GeoPoint(double.Parse(point[0].Trim()), double.Parse(point[1].Trim()));
                }
            }
            else if (obj.StartsWith("LINESTRING("))
            {
                var pointStr = obj.Substring(11, obj.Length - 12);
                return new GeoLineString(StringToPointCollection(pointStr));
            }
            else if (obj.StartsWith("MULTIPOINT("))
            {
                var pointStr = obj.Substring(11, obj.Length - 12);
                return new GeoMultiPoint(StringToPointCollection(pointStr));
            }
            else if (obj.StartsWith("MULTILINESTRING("))
            {
                var str = obj.Substring(16, obj.Length - 17);

                var rx = new System.Text.RegularExpressions.Regex(@"\)\s*,\s*\(");
                var parts = rx.Split(str);

                string pointStr;
                var lines = new List<GeoLineString>();
                foreach (var p in parts)
                {
                    pointStr = TrimCommas(p);
                    lines.Add(new GeoLineString(StringToPointCollection(pointStr)));
                }

                return new GeoMultiLineString(lines);
            }
            else if (obj.StartsWith("MULTIPOLYGON("))
            {
                var mp = new GeoMultiPolygon();

                var str = obj.Substring(13, obj.Length - 14);

                var rx = new System.Text.RegularExpressions.Regex(@"\)\)\s*,\s*\(\(");
                var parts = rx.Split(str);

                foreach (var p in parts)
                {
                    mp.Geometries.Add((GeoPolygon)ConvertBack(string.Format("POLYGON({0})", TrimCommas(p))));
                }

                return mp;
            }
            else if (obj.StartsWith("POLYGON("))
            {
                var str = obj.Substring(8, obj.Length - 9);

                var rx = new System.Text.RegularExpressions.Regex(@"\)\s*,\s*\(");
                var parts = rx.Split(str);

                if (parts.Length > 0)
                {
                    var exteriorRing = new GeoLinearRing(this.StringToPointCollection(TrimCommas(parts[0])));

                    var poly = new GeoPolygon(exteriorRing);

                    for(var i = 1; i < parts.Length; i++)
                    {
                        poly.InteriorRings.Add(new GeoLinearRing(this.StringToPointCollection(TrimCommas(parts[i]))));
                    }

                    return poly;
                }
            }
            else if (obj.StartsWith("GEOMETRYCOLLECTION("))
            {
                throw new NotSupportedException("Converting from Well-Known-Text GeometryCollection Is Not Supported");
            }

            throw new NotSupportedException("Unrecognized Well-Known-Text String");
        }

        private static string TrimCommas(string p)
        {
            var r = p;
            while (r.StartsWith("("))
            {
                r = r.Substring(1);
            }
            while (r.EndsWith(")"))
            {
                r = r.Substring(0, r.Length - 1);
            }
            return r;
        }

        #region "Private Static Methods"

        private string PolygonCollectionToString(IList<GeoPolygon> polygons)
        {
            var sb = new StringBuilder();
            var isFirst = true;
            foreach (var poly in polygons)
            {
                if (!isFirst)
                {
                    sb.Append(",");
                }
                else
                {
                    isFirst = false;
                }

                sb.AppendFormat("(({0}))", PointCollectionToString(poly.ExteriorRing.Geometries));
            }

            return sb.ToString();
        }

        private string PointCollectionToString(IList<GeoPoint> points)
        {
            var sb = new StringBuilder();
            var isFirstItem = true;
            foreach (var p in points)
            {
                if (!isFirstItem)
                {
                    sb.Append(", ");
                }
                sb.AppendFormat("{0} {1}", p.Longitude.ToString(), p.Latitude.ToString());
                isFirstItem = false;
            }

            return sb.ToString();
        }

        private IList<GeoPoint> StringToPointCollection(string pointStr)
        {
            string[] p;
            IList<GeoPoint> pointCollection = new List<GeoPoint>();

            var points = pointStr.Split(',');
            
            foreach (var point in points)
            {
                p = point.Trim().Split(' ');
                pointCollection.Add(new GeoPoint(double.Parse(p[0]), double.Parse(p[1])));
            }

            return pointCollection;
        }

        private string GeoLineStringCollectionToString(IList<GeoLineString> col)
        {
            var sb = new StringBuilder();
            bool isFirstItem = true;
            foreach (var g in col)
            {
                if (!isFirstItem)
                {
                    sb.Append(", ");
                }
                sb.AppendFormat("({0})", PointCollectionToString(g.Geometries));
                isFirstItem = false;
            }

            return sb.ToString();
        }

        #endregion
    }
}
