﻿// Copyright 2010 - Chris Pietschmann <http://pietschsoft.com>
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

namespace SilverSpatial
{
    public interface IGeometry
    {
        #region "Properties"

        GeometryType GeometryType { get; }

        string STGeometryType { get; }
        int STNumGeometries { get; }
        int? STNumInteriorRing { get; }
        double? STX { get; }
        double? STY { get; }

        #endregion

        #region "Methods"

        byte[] STAsBinary();
        string STAsText();
        bool STContains(IGeometry geometry);
        IGeometry STGeometryN(int index);
        double STLength();
        int STNumPoints();
        IGeometry STPointN(int index);
        int STSrid();
        IGeometry STStartPoint();
        bool STWithin(IGeometry geometry);

        #endregion
    }
}
