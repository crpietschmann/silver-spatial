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

using Microsoft.Maps.MapControl.Core;
using System;

namespace SilverSpatial.Microsoft
{
    public static class MapBaseExtensions
    {
        /// <summary>
        /// Sets the center location of the map view
        /// </summary>
        /// <param name="map"></param>
        /// <param name="point">The GeoPoint that represents the location to set as the center location of the map view</param>
        /// <returns></returns>
        public static MapBase SetCenter(this MapBase map, GeoPoint point)
        {
            if (map == null) { throw new ArgumentNullException("map"); }
            if (point == null) { throw new ArgumentNullException("point"); }
            map.Center = (new SilverSpatial.Microsoft.Converters.LocationConverter()).Convert(point);
            return map;
        }
    }
}
