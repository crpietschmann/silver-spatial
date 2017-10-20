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
using Microsoft.Maps.MapControl;

namespace SilverSpatial.Microsoft.Converters
{
    public class GeoPolygonConverter : SilverSpatial.Converters.ValueConverterBase<MapPolygon, GeoPolygon>
    {
        private LocationConverter converter = new LocationConverter();

        public override GeoPolygon Convert(MapPolygon obj)
        {
            var locs = new List<GeoPoint>();
            foreach (var l in obj.Locations)
            {
                locs.Add(converter.ConvertBack(l));
            }
            return new GeoPolygon(new GeoLinearRing(locs));
        }

        public override MapPolygon ConvertBack(GeoPolygon obj)
        {
            var poly = new MapPolygon();
            poly.Locations = new LocationCollection();
            foreach (var p in obj.ExteriorRing.Geometries)
            {
                poly.Locations.Add(converter.Convert(p));
            }
            return poly;
        }
    }
}
