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
using System.Windows.Data;

namespace SilverSpatial.Converters
{
    public abstract class ValueConverterBase<TObject, TSerialize> : IValueConverter
    {
        #region "Abstract Methods"

        public abstract TSerialize Convert(TObject obj);
        public abstract TObject ConvertBack(TSerialize obj);

        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.GetType() != typeof(TObject))
            {
                throw new ArgumentException(string.Format("Unexpected object type. Must be of type ({0})", typeof(TObject).AssemblyQualifiedName), "value");
            }
            if (targetType != typeof(TSerialize))
            {
                throw new ArgumentException(string.Format("Unexpected object type. Must be of type ({0})", typeof(TSerialize).AssemblyQualifiedName), "targetType");
            }
            return this.Convert((TObject)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.GetType() != typeof(TSerialize))
            {
                throw new ArgumentException(string.Format("Unexpected object type. Must be of type ({0})", typeof(TSerialize).AssemblyQualifiedName), "value");
            }
            if (targetType != typeof(TObject))
            {
                throw new ArgumentException(string.Format("Unexpected object type. Must be of type ({0})", typeof(TObject).AssemblyQualifiedName), "targetType");
            }
            return this.ConvertBack((TSerialize)value);
        }

        #endregion
    }
}
