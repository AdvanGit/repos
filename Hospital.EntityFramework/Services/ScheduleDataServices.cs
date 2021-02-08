﻿using Hospital.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.EntityFramework.Services
{
    public class ScheduleDataServices
    {
        private readonly HospitalDbContextFactory _contextFactory;

        public ScheduleDataServices(HospitalDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Entry>> GetEntriesByDate(int doctorId, DateTime date)
        {
            using (HospitalDbContext db = _contextFactory.CreateDbContext())
            {
                List<Entry> entries = await db.Entries
                    .Include(e => e.DoctorDestination).ThenInclude(s=>s.Department).ThenInclude(d=>d.Title)
                    .Include(e=>e.Patient)
                    .Include(e=>e.Registrator)
                    .Include(e=>e.MedCard).ThenInclude(m=>m.Diagnosis)
                    .AsQueryable()
                    .Where(e => e.DoctorDestination.Id == doctorId)
                    .Where(e => e.TargetDateTime.Date == date.Date)
                    .OrderBy(e=>e.TargetDateTime)
                    .ToListAsync();
                return entries;
            }
        }

    }
}