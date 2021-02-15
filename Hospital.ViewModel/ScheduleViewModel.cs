﻿using Hospital.Domain.Model;
using Hospital.EntityFramework;
using Hospital.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Hospital.ViewModel
{
    public class ScheduleViewModel : MainViewModel
    {
        private ScheduleDataServices scheduleDataServices = new ScheduleDataServices(new HospitalDbContextFactory());

        public DateTime SelectedDate { get => _selectedDate; set { _selectedDate = value; OnPropertyChanged(nameof(SelectedDate)); GetEntry(value); } }
        private DateTime _selectedDate;

        private Entry _currentEntry;
        public Entry CurrentEntry { get => _currentEntry; set { _currentEntry = value; OnPropertyChanged(nameof(CurrentEntry)); } }

        public ObservableCollection<Entry> Entries { get; } = new ObservableCollection<Entry>();

        public ScheduleViewModel()
        {
            SelectedDate = new DateTime(2020, 11, 26);
        }

        private async void GetEntry(DateTime date)
        {
            Entries.Clear();
            var result = await scheduleDataServices.GetEntriesByDate(CurrentStuffId, date);
            foreach (Entry entry in result) Entries.Add(entry);
        }
    }
}
