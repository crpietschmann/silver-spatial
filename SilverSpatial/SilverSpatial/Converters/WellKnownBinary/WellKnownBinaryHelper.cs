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
using System.IO;

namespace SilverSpatial.Converters.WellKnownBinary
{
    public class WellKnownBinaryHelper
    {
        public static uint ReadUInt(BinaryReader reader, WellKnownBinaryByteOrder byteOrder)
        {
            switch (byteOrder)
            {
                case WellKnownBinaryByteOrder.NDR:
                    return reader.ReadUInt32();

                case WellKnownBinaryByteOrder.XDR:
                    var bytes = BitConverter.GetBytes(reader.ReadUInt32());
                    Array.Reverse(bytes);
                    return BitConverter.ToUInt32(bytes, 0);

                default:
                    throw new ArgumentException("Unknown Byte Order", "byteOrder");
            }
        }

        public static double ReadDouble(BinaryReader reader, WellKnownBinaryByteOrder byteOrder)
        {
            switch (byteOrder)
            {
                case WellKnownBinaryByteOrder.NDR:
                    return reader.ReadDouble();

                case WellKnownBinaryByteOrder.XDR:
                    var bytes = BitConverter.GetBytes(reader.ReadDouble());
                    Array.Reverse(bytes);
                    return BitConverter.ToDouble(bytes, 0);

                default:
                    throw new ArgumentException("Unknown Byte Order", "byteOrder");
            }
        }

        public static void WriteDouble(double value, BinaryWriter writer, WellKnownBinaryByteOrder byteOrder)
        {
            switch (byteOrder)
            {
                case WellKnownBinaryByteOrder.NDR:
                    writer.Write(value);
                    
                    break;
                case WellKnownBinaryByteOrder.XDR:
                    var bytes = BitConverter.GetBytes(value);
                    Array.Reverse(bytes);
                    writer.Write(BitConverter.ToDouble(bytes, 0));

                    break;
                default:
                    throw new ArgumentException("Unknown Byte Order", "byteOrder");
            }
        }

        public static void WriteUInt(uint value, BinaryWriter writer, WellKnownBinaryByteOrder byteOrder)
        {
            switch (byteOrder)
            {
                case WellKnownBinaryByteOrder.NDR:
                    writer.Write(value);
                    break;

                case WellKnownBinaryByteOrder.XDR:
                    var bytes = BitConverter.GetBytes(value);
                    Array.Reverse(bytes);
                    writer.Write(BitConverter.ToUInt32(bytes, 0));
                    break;

                default:
                    throw new ArgumentException("Unknown Byte Order", "byteOrder");
            }
        }
    }
}