﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ClipboardCanvas.ValueConverters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not bool boolParam)
            {
                return null;
            }

            if (parameter is not string stringParam)
            {
                return boolParam ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                if (stringParam.ToLower() == "invert")
                {
                    return boolParam ? Visibility.Collapsed : Visibility.Visible;
                }

                return boolParam ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
