﻿using Hospital.Domain.Model;
using Hospital.EntityFramework;
using Hospital.EntityFramework.Filters;
using Hospital.EntityFramework.Services;
using Hospital.ViewModel.Notificator;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.ViewModel.Ambulatory
{

    public class EntryViewModel : MainViewModel
    {
        private readonly EntryDataServices entryDataServices = new EntryDataServices(new HospitalDbContextFactory());
        private readonly AmbulatoryDataService ambulatoryDataService;

        private Entry _currentEntry;
        private Entry _selectedEntry;
        private Entry _entryOut;
        private EntrySearchFilter _filter = new EntrySearchFilter
        {
            DepartmentType = DepartmentType.Ambulatory,
            IsAdress = true,
            IsDepartment = true,
            IsQualification = true,
            IsName = true,
            IsNearest = true,
            IsFree = true,
            DateTime = DateTime.Now
        };

        public EntryViewModel(Entry currentEntry, AmbulatoryDataService ambulatoryDataService)
        {
            CurrentEntry = currentEntry;
            this.ambulatoryDataService = ambulatoryDataService;
        }

        public ObservableCollection<Entry> FindedEntries { get; } = new ObservableCollection<Entry>();
        public ObservableCollection<Entry> BySelectEntries { get; } = new ObservableCollection<Entry>();
        public ObservableCollection<Entry> FilteredCollection { get; } = new ObservableCollection<Entry>();

        public Entry CurrentEntry { get => _currentEntry; set { _currentEntry = value; OnPropertyChanged(nameof(CurrentEntry)); } }
        public Entry SelectedEntry { get => _selectedEntry; set { _selectedEntry = value; OnPropertyChanged(nameof(SelectedEntry)); } }
        public Entry EntryOut { get => _entryOut; set { _entryOut = value; OnPropertyChanged(nameof(EntryOut)); } }

        public EntrySearchFilter Filter { get => _filter; set { _filter = value; OnPropertyChanged(nameof(Filter)); } }

        public async void FindEntry(string searchValue)
        {
            FindedEntries.Clear();
            var res = await entryDataServices.FindDoctor(searchValue, Filter);
            foreach (Entry entry in res) FindedEntries.Add(entry);
        }
        public async void FindBySelect(object item)
        {
            SelectedEntry = (Entry)item;
            BySelectEntries.Clear();
            FilteredCollection.Clear();
            var res = await entryDataServices.GetEntries(SelectedEntry.DoctorDestination, SelectedEntry.TargetDateTime.Date);
            foreach (Entry _entry in res) BySelectEntries.Add(_entry);
            foreach (Entry _entry in res.Where(e => e.EntryStatus == EntryStatus.Открыта)) FilteredCollection.Add(_entry);
        }
        public void SetEntryOut(object item) => EntryOut = (Entry)item;

        public async void ToAbsence()
        {
            CurrentEntry.EntryStatus = EntryStatus.Неявка;
            if (await entryDataServices.UpdateEntry(CurrentEntry))
                NotificationManager.AddItem(new NotificationItem(NotificationType.Success, TimeSpan.FromSeconds(2), "Запись успешно обновлена"));
        }
        public async Task UpdateEntry()
        {
            EntryOut.Registrator = CurrentEntry.DoctorDestination;
            EntryOut.Patient = CurrentEntry.Patient;
            EntryOut.CreateDateTime = DateTime.Now;
            EntryOut.EntryStatus = EntryStatus.Ожидание;
            EntryOut.MedCard = CurrentEntry.MedCard;
            await entryDataServices.UpdateEntry(EntryOut);
            CurrentEntry.EntryStatus = EntryStatus.Закрыта;
            CurrentEntry.EntryOut = EntryOut;
            await entryDataServices.UpdateEntry(CurrentEntry);
        }
    }
}