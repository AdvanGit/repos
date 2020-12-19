﻿using Hospital.ViewModel.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Hospital.WPF.Services
{
    public class MultiCommandConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            List<object> _commands = new List<object>(values);
            return new RelayCommand(GetCompoundExecute(_commands), GetCompoundCanExecute(_commands));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

        private Action<object> GetCompoundExecute(List<object> _commands)
        {
            return (parameter) =>
            {
                foreach (RelayCommand command in _commands)
                {
                    if (command != default(RelayCommand))
                        command.Execute(parameter);
                }
            };
        }

        private Func<object, bool> GetCompoundCanExecute(List<object> _commands)
        {
            return (parameter) =>
            {
                bool res = true;
                foreach (RelayCommand command in _commands)
                    if (command != default(RelayCommand))
                        res &= command.CanExecute(parameter);
                return res;
            };
        }
    }
}
