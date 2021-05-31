﻿using Hospital.Domain.Model;
using Hospital.EntityFramework;
using Hospital.EntityFramework.Filters;
using Hospital.EntityFramework.Services;
using Hospital.ViewModel.Notificator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.ViewModel
{
    public class RegistratorViewModel : MainViewModel
    {
        private readonly EntryDataServices registratorDataServices = new EntryDataServices(new HospitalDbContextFactory());
        private readonly GenericDataServices<Belay> genericDataServicesBelay = new GenericDataServices<Belay>(new HospitalDbContextFactory());
        private readonly GenericDataServices<Patient> genericDataServicesPatient = new GenericDataServices<Patient>(new HospitalDbContextFactory());
        private readonly GenericDataServices<Entry> genericDataServicesEntry = new GenericDataServices<Entry>(new HospitalDbContextFactory());

        private EntrySearchFilter _filter = new EntrySearchFilter()
        {
            IsName = true,
            IsFree = true,
            IsNearest = true,
            IsDepartment = true,
            IsAdress = true,
            IsQualification = true,
            DateTime = DateTime.Now,
            DepartmentType = Enum.Parse<DepartmentType>("0")
        };
        private string _searchString = "";

        private Entry _selectedEntry;
        private Patient _selectedPatient;
        private Patient _editingPatient;

        public Entry SelectedEntry { get => _selectedEntry; set { _selectedEntry = value; if (value != null) Filter.DateTime = value.TargetDateTime; OnPropertyChanged(nameof(SelectedEntry)); } }
        public Patient SelectedPatient { get => _selectedPatient; set { _selectedPatient = value; OnPropertyChanged(nameof(SelectedPatient)); } }
        public Patient EditingPatient { get => _editingPatient; set { _editingPatient = value; OnPropertyChanged(nameof(EditingPatient)); } }

        public ObservableCollection<Entry> Doctors { get; } = new ObservableCollection<Entry>();
        public ObservableCollection<Entry> Entries { get; } = new ObservableCollection<Entry>();
        public ObservableCollection<Entry> FilteredEntries { get; } = new ObservableCollection<Entry>();

        public ObservableCollection<Patient> Patients { get; } = new ObservableCollection<Patient>();
        public ObservableCollection<Belay> Belays { get; } = new ObservableCollection<Belay>();

        public EntrySearchFilter Filter { get => _filter; set { _filter = value; OnPropertyChanged(nameof(Filter)); } }
        public string SearchString { get => _searchString; set { _searchString = value; OnPropertyChanged(nameof(SearchString)); } }

        public async Task SearchPatient()
        {
            if (SearchString != "")
            {
                IsLoading = true;
                try
                {
                    IEnumerable<Patient> result = await registratorDataServices.FindPatient(SearchString);
                    if (result.Count() != 0)
                    {
                        Patients.Clear();
                        foreach (Patient patient in result) Patients.Add(patient);
                    }
                    else NotificationManager.AddItem(new NotificationItem(NotificationType.Information, TimeSpan.FromSeconds(3), "Ничего не найдено"));
                }
                catch (Exception ex)
                {
                    NotificationManager.AddException(ex, 6);
                }
                IsLoading = false;
            }
        }
        public async Task SearchDoctor()
        {
            if (SearchString != "")
            {
                IsLoading = true;
                try
                {
                    IEnumerable<Entry> result = await registratorDataServices.FindDoctor(SearchString, _filter);
                    if (result.Count() != 0)
                    {
                        Doctors.Clear();
                        foreach (Entry entry in result) Doctors.Add(entry);
                    }
                    else NotificationManager.AddItem(new NotificationItem(NotificationType.Information, TimeSpan.FromSeconds(3), "Ничего не найдено"));

                }
                catch (Exception ex)
                {
                    NotificationManager.AddException(ex, 6);
                }
                IsLoading = false;
            }
        }
        public async Task GetEntries(object obj)
        {
            if (obj != null)
            {
                SelectedEntry = (Entry)obj;
                IsLoading = true;
                try
                {
                    IEnumerable<Entry> result = await registratorDataServices.GetEntries(SelectedEntry.DoctorDestination, (SelectedEntry.TargetDateTime));
                    Entries.Clear();
                    FilteredEntries.Clear();
                    foreach (Entry entry in result) Entries.Add(entry);
                    foreach (Entry entry in result.Where(e => e.EntryStatus == EntryStatus.Открыта)) FilteredEntries.Add(entry);
                }
                catch (Exception ex)
                {
                    NotificationManager.AddException(ex, 6);
                }
                IsLoading = false;
            }
        }
        public async Task GetEntries(bool isBack)
        {
            if (SelectedEntry != null)
            {
                if (isBack) Filter.DateTime -= TimeSpan.FromDays(1);
                else Filter.DateTime += TimeSpan.FromDays(1);
                OnPropertyChanged(nameof(Filter));
                IsLoading = true;
                try
                {
                    IEnumerable<Entry> result = await registratorDataServices.GetEntries(SelectedEntry.DoctorDestination, Filter.DateTime);
                    Entries.Clear();
                    FilteredEntries.Clear();
                    foreach (Entry entry in result) Entries.Add(entry);
                    foreach (Entry entry in result.Where(e => e.EntryStatus == EntryStatus.Открыта)) FilteredEntries.Add(entry);
                }
                catch (Exception ex)
                {
                    NotificationManager.AddException(ex, 6);
                }
                IsLoading = false;
            }
        }
        public async Task GetBelays()
        {
            if (Belays.Count == 0)
            {
                try
                {
                    IEnumerable<Belay> result = await genericDataServicesBelay.GetAll();
                    foreach (Belay belay in result) Belays.Add(belay);
                }
                catch (Exception ex)
                {
                    NotificationManager.AddException(ex, 6);
                }
            }
        }
        public async Task SavePatient()
        {
            try
            {
                await genericDataServicesPatient.Update(EditingPatient.Id, EditingPatient);
                SelectedPatient = EditingPatient;
                Patients.Clear();
                Patients.Add(await genericDataServicesPatient.GetById(SelectedPatient.Id));
            }
            catch (Exception ex)
            {
                NotificationManager.AddException(ex, 6);
            }
        }
        public async Task CreateEntry()
        {
            SelectedEntry.Patient = SelectedPatient;
            SelectedEntry.Registrator = SelectedEntry.DoctorDestination; //---заглушка отсутсвия данных аккаунта
            SelectedEntry.EntryStatus = EntryStatus.Ожидание;
            await genericDataServicesEntry.Update(SelectedEntry.Id, SelectedEntry);
            SelectedEntry = null;
            SelectedPatient = null;
        } //check on exist баг создания записи, если уже есть запись на текущее время. вследствии механизма автоматического выбора. сделать проверку, и предложение заменить, проапдейтить запись

        public void SelectEntity(object entity)
        {
            if (entity != null && entity.GetType() == typeof(Patient)) SelectedPatient = (Patient)entity;
            else if (entity != null && entity.GetType() == typeof(Entry)) SelectedEntry = (Entry)entity;
        }
        public void EditPatient(bool isNew)
        {
            if (isNew) EditingPatient = new Patient();
            else EditingPatient = (Patient)SelectedPatient.Clone();
        }

    }
}
