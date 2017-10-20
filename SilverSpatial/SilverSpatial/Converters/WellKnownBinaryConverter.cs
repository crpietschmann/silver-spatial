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
//
// SOURCE CODE IS MODIFIED FROM ANOTHER WORK AND IS ORIGINALLY BASED ON SharpMap.

//// Copyright 2005, 2006 - Morten Nielsen (www.iter.dk)
////
//// This file is part of SharpMap.
//// SharpMap is free software; you can redistribute it and/or modify
//// it under the terms of the GNU Lesser General Public License as published by
//// the Free Software Foundation; either version 2 of the License, or
//// (at your option) any later version.
//// 
//// SharpMap is distributed in the hope that it will be useful,
//// but WITHOUT ANY WARRANTY; without even the implied warranty of
//// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//// GNU Lesser General Public License for more details.

//// You should have received a copy of the GNU Lesser General Public License
//// along with SharpMap; if not, write to the Free Software
//// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA 

//// SOURCECODE IS MODIFIED FROM ANOTHER WORK AND IS ORIGINALLY BASED ON GeoTools.NET:
///*
// *  Copyright (C) 2002 Urban Science Applications, Inc. 
// *
// *  This library is free software; you can redistribute it and/or
// *  modify it under the terms of the GNU Lesser General Public
// *  License as published by the Free Software Foundation; either
// *  version 2.1 of the License, or (at your option) any later version.
// *
// *  This library is distributed in the hope that it will be useful,
// *  but WITHOUT ANY WARRANTY; without even the implied warranty of
// *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// *  Lesser General Public License for more details.
// *
// *  You should have received a copy of the GNU Lesser General Public
// *  License along with this library; if not, write to the Free Software
// *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// *
// */


using System;
using System.Windows.Data;
using System.IO;
using SilverSpatial.Converters.WellKnownBinary;

namespace SilverSpatial.Converters
{
    public class WellKnownBinaryConverter : ValueConverterBase<Geometry, byte[]>
    {
        #region "SpatialConverter<> Methods"

        public override byte[] Convert(Geometry obj)
        {
            return Convert(obj, WellKnownBinaryByteOrder.NDR);
        }

        public override Geometry ConvertBack(byte[] obj)
        {
            using (var ms = new MemoryStream(obj))
            {
                using (var reader = new BinaryReader(ms))
                {
                    return this.ConvertBack(reader);
                }
            }
        }

        #endregion

        #region "Public Methods"

        public byte[] Convert(Geometry obj, WellKnownBinaryByteOrder byteOrder)
        {
            using(var ms = new MemoryStream())
            {
                using (var writer = new BinaryWriter(ms))
                {
                    writer.Write((byte)byteOrder);

                    this.WriteGeometryType(obj, writer, byteOrder);

                    this.WriteGeometryData(obj, writer, byteOrder);

                    return ms.ToArray();
                }
            }
        }

        public Geometry ConvertBack(BinaryReader reader)
        {
            // Get first byte. This byte defines if the Well Known Binary (WKB) is in XDR or NDR format.
            var byteOrder = reader.ReadByte();

            // Get the Geometry Type defined within this WKB
            var geometryType = WellKnownBinaryHelper.ReadUInt(reader, (WellKnownBinaryByteOrder)byteOrder);

            switch ((GeometryType)geometryType)
            {
                case GeometryType.GeometryCollection:
                    return this.CreateGeometryCollection(reader, (WellKnownBinaryByteOrder)byteOrder);
                case GeometryType.LineString:
                    return this.CreateGeoLineString(reader, (WellKnownBinaryByteOrder)byteOrder);
                case GeometryType.MultiLineString:
                    return this.CreateGeoMultiLineString(reader, (WellKnownBinaryByteOrder)byteOrder);
                case GeometryType.MultiPoint:
                    return this.CreateGeoMultiPoint(reader, (WellKnownBinaryByteOrder)byteOrder);
                case GeometryType.MultiPolygon:
                    return this.CreateGeoMultiPolygon(reader, (WellKnownBinaryByteOrder)byteOrder);
                case GeometryType.Point:
                    return this.CreateGeoPoint(reader, (WellKnownBinaryByteOrder)byteOrder);
                case GeometryType.Polygon:
                    return this.CreateGeoPolygon(reader, (WellKnownBinaryByteOrder)byteOrder);
                default:
                    if (Enum.IsDefined(typeof(GeometryType), geometryType))
                    {
                        throw new ArgumentException(string.Format("Geometry type ({0}) is unsupported.", geometryType));
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Unknown Geometry Type Found: {0}", geometryType));
                    }
            }
        }

        #endregion

        #region "Protected Methods"

        protected GeometryCollection CreateGeometryCollection(BinaryReader reader, WellKnownBinaryByteOrder byteOrder)
        {
            var numGeometries = (int)WellKnownBinaryHelper.ReadUInt(reader, byteOrder);

            var geometries = new GeometryCollection();

            for (var i = 0; i < numGeometries; i++)
            {
                geometries.Geometries.Add(this.ConvertBack(reader));
            }

            return geometries;
        }

        protected GeoLinearRing CreateGeoLinearRing(BinaryReader reader, WellKnownBinaryByteOrder byteOrder)
        {
            var linearRing = new GeoLinearRing();

            this.ReadGeoPoints(linearRing, reader, byteOrder);

            if (linearRing[0].Latitude != linearRing[linearRing.STNumGeometries - 1].Latitude ||
                linearRing[0].Longitude != linearRing[linearRing.STNumGeometries - 1].Longitude)
            {
                linearRing.Geometries.Add(new GeoPoint(linearRing[0].Latitude, linearRing[1].Longitude));
            }

            return linearRing;
        }

        protected GeoLineString CreateGeoLineString(BinaryReader reader, WellKnownBinaryByteOrder byteOrder)
        {
            var lineString = new GeoLineString();

            //GeoPoint[] points = ReadGeoPoints(reader, byteOrder);
            //for (var i = 0; i < points.Length; i++)
            //{
            //    lineString.Geometries.Add(points[i]);
            //}
            this.ReadGeoPoints(lineString, reader, byteOrder);

            return lineString;
        }

        protected GeoMultiLineString CreateGeoMultiLineString(BinaryReader reader, WellKnownBinaryByteOrder byteOrder)
        {
            var numLineStrings = (int)WellKnownBinaryHelper.ReadUInt(reader, byteOrder);

            var multiLineString = new GeoMultiLineString();

            for (var i = 0; i < numLineStrings; i++)
            {
                reader.ReadByte();
                WellKnownBinaryHelper.ReadUInt(reader, byteOrder);

                multiLineString.Geometries.Add(this.CreateGeoLineString(reader, byteOrder));
            }

            return multiLineString;
        }

        protected GeoMultiPoint CreateGeoMultiPoint(BinaryReader reader, WellKnownBinaryByteOrder byteOrder)
        {
            var numPoints = (int)WellKnownBinaryHelper.ReadUInt(reader, byteOrder);

            var multiPoint = new GeoMultiPoint();

            for (var i = 0; i < numPoints; i++)
            {
                reader.ReadByte();
                WellKnownBinaryHelper.ReadUInt(reader, byteOrder);

                multiPoint.Geometries.Add(this.CreateGeoPoint(reader, byteOrder));
            }

            return multiPoint;
        }

        protected GeoMultiPolygon CreateGeoMultiPolygon(BinaryReader reader, WellKnownBinaryByteOrder byteOrder)
        {
            var numPolygons = (int)WellKnownBinaryHelper.ReadUInt(reader, byteOrder);

            var multiPolygon = new GeoMultiPolygon();

            for(var i = 0; i < numPolygons; i++){
                reader.ReadByte();
                WellKnownBinaryHelper.ReadUInt(reader, byteOrder);

                multiPolygon.Geometries.Add(this.CreateGeoPolygon(reader, byteOrder));
            }
            
            return multiPolygon;
        }

        protected GeoPolygon CreateGeoPolygon(BinaryReader reader, WellKnownBinaryByteOrder byteOrder)
        {
            var numRings = (int)WellKnownBinaryHelper.ReadUInt(reader, byteOrder);

            var polygon = new GeoPolygon(this.CreateGeoLinearRing(reader, byteOrder));

            for(var i = 0; i < (numRings -1); i++){
                polygon.InteriorRings.Add(this.CreateGeoLinearRing(reader, byteOrder));
            }

            return polygon;
        }

        protected void ReadGeoPoints(GeoLineString collection, BinaryReader reader, WellKnownBinaryByteOrder byteOrder)
        {
            var numPoints = (int)WellKnownBinaryHelper.ReadUInt(reader, byteOrder);

            for (var i = 0; i < numPoints; i++)
            {
                collection.Geometries.Add(this.CreateGeoPoint(reader, byteOrder));
            }
        }

        //protected GeoPoint[] ReadGeoPoints(BinaryReader reader, WellKnownBinaryByteOrder byteOrder)
        //{
        //    var numPoints = (int)WellKnownBinaryHelper.ReadUInt(reader, byteOrder);

        //    var points = new GeoPoint[numPoints];

        //    for (var i = 0; i < numPoints; i++)
        //    {
        //        points[i] = WellKnownBinaryHelper.CreateGeoPoint(reader, byteOrder);
        //    }

        //    return points;
        //}

        protected void WriteGeometryData(Geometry obj, BinaryWriter writer, WellKnownBinaryByteOrder byteOrder)
        {
            switch (obj.GeometryType)
            {
                case GeometryType.GeometryCollection:
                    this.WriteGeoCollection(obj as GeometryCollection, writer, byteOrder);
                    break;
                case GeometryType.LineString:
                    this.WriteGeoLineString(obj as GeoLineString, writer, byteOrder);
                    break;
                case GeometryType.MultiLineString:
                    this.WriteGeoMultiLineString(obj as GeoMultiLineString, writer, byteOrder);
                    break;
                case GeometryType.MultiPoint:
                    this.WriteGeoMultiPoint(obj as GeoMultiPoint, writer, byteOrder);
                    break;
                case GeometryType.MultiPolygon:
                    this.WriteGeoMultiPolygon(obj as GeoMultiPolygon, writer, byteOrder);
                    break;
                case GeometryType.Point:
                    this.WriteGeoPoint(obj as GeoPoint, writer, byteOrder);
                    break;
                case GeometryType.Polygon:
                    this.WriteGeoPolygon(obj as GeoPolygon, writer, byteOrder);
                    break;
                default:
                    throw new ArgumentException(string.Format("Unsupported Geometry Type. ({0})", obj.GeometryType.ToString()));
            }
        }

        protected void WriteGeometryType(Geometry obj, BinaryWriter writer, WellKnownBinaryByteOrder byteOrder)
        {
            WellKnownBinaryHelper.WriteUInt((uint)obj.GeometryType, writer, byteOrder);
        }

        protected void WriteGeoCollection(GeometryCollection obj, BinaryWriter writer, WellKnownBinaryByteOrder byteOrder)
        {
            var geoCount = obj.STNumGeometries;

            // Write out the Number of Geometry objects in the collection
            WellKnownBinaryHelper.WriteUInt((uint)geoCount, writer, byteOrder);

            // Write out each Geometry object in the collection
            for (var i = 0; i < geoCount; i++)
            {
                // Write the Geometry objects Byte Order
                writer.Write((byte)byteOrder);
                // Write the Geometry Type
                this.WriteGeometryType(obj[i], writer, byteOrder);
                // Write the Geometry object
                this.WriteGeometryData(obj[i], writer, byteOrder);
            }
        }

        protected void WriteGeoLineString(GeoLineString obj, BinaryWriter writer, WellKnownBinaryByteOrder byteOrder)
        {
            WellKnownBinaryHelper.WriteUInt((uint)obj.STNumGeometries, writer, byteOrder);

            foreach (var point in obj.Geometries)
            {
                this.WriteGeoPoint(point, writer, byteOrder);
            }
        }

        protected void WriteGeoMultiLineString(GeoMultiLineString obj, BinaryWriter writer, WellKnownBinaryByteOrder byteOrder)
        {
            WellKnownBinaryHelper.WriteUInt((uint)obj.STNumGeometries, writer, byteOrder);

            foreach (var lineString in obj.Geometries)
            {
                writer.Write((byte)byteOrder);
                WellKnownBinaryHelper.WriteUInt((uint)lineString.GeometryType, writer, byteOrder);

                this.WriteGeoLineString(lineString, writer, byteOrder);
            }
        }

        protected void WriteGeoMultiPoint(GeoMultiPoint obj, BinaryWriter writer, WellKnownBinaryByteOrder byteOrder)
        {
            WellKnownBinaryHelper.WriteUInt((uint)obj.STNumGeometries, writer, byteOrder);

            foreach (var point in obj.Geometries)
            {
                writer.Write((byte)byteOrder);
                WellKnownBinaryHelper.WriteUInt((uint)point.GeometryType, writer, byteOrder);

                this.WriteGeoPoint(point, writer, byteOrder);
            }
        }

        protected void WriteGeoMultiPolygon(GeoMultiPolygon obj, BinaryWriter writer, WellKnownBinaryByteOrder byteOrder)
        {
            WellKnownBinaryHelper.WriteUInt((uint)obj.STNumGeometries, writer, byteOrder);

            foreach (var polygon in obj.Geometries)
            {
                writer.Write((byte)byteOrder);
                WellKnownBinaryHelper.WriteUInt((uint)polygon.GeometryType, writer, byteOrder);

                this.WriteGeoPolygon(polygon, writer, byteOrder);
            }
        }

        protected void WriteGeoPoint(GeoPoint obj, BinaryWriter writer, WellKnownBinaryByteOrder byteOrder)
        {
            if (obj == null)
            {
                throw new ArgumentException("Value can not be Null.", "obj");
            }

            WellKnownBinaryHelper.WriteDouble(obj.Longitude, writer, byteOrder);
            WellKnownBinaryHelper.WriteDouble(obj.Latitude, writer, byteOrder);
        }

        protected void WriteGeoPolygon(GeoPolygon obj, BinaryWriter writer, WellKnownBinaryByteOrder byteOrder)
        {
            var numInteriorRings = obj.InteriorRings.Count + 1;

            WellKnownBinaryHelper.WriteUInt((uint)numInteriorRings, writer, byteOrder);

            this.WriteGeoLineString(obj.ExteriorRing, writer, byteOrder);

            foreach (var linearRing in obj.InteriorRings)
            {
                this.WriteGeoLineString(linearRing, writer, byteOrder);
            }
        }

        protected GeoPoint CreateGeoPoint(BinaryReader reader, WellKnownBinaryByteOrder byteOrder)
        {
            return new GeoPoint(WellKnownBinaryHelper.ReadDouble(reader, byteOrder), WellKnownBinaryHelper.ReadDouble(reader, byteOrder));
        }

        #endregion

    }
}
