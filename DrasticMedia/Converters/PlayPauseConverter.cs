// <copyright file="PlayPauseConverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Styles;

namespace DrasticMedia.Converters
{
    /// <summary>
    /// Play Pause Converter.
    /// </summary>
    public class PlayPauseConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isPlaying)
            {
                return isPlaying ? IconFont.PauseCircle : IconFont.PlayCircle;
            }

            return IconFont.CircleNotch;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
