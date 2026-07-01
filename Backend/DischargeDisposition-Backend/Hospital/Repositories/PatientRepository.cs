using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore;
using System;

namespace DischargeDisposition_Backend.Hospital.Repositories
{
    public class PatientRepository
        : IPatientRepository
    {
        private readonly
            ILogger<PatientRepository>
            _logger;

        private readonly
            HospitalDbContext
            _context;

        public PatientRepository(
            ILogger<PatientRepository> logger,
            HospitalDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<Patient>>
            GetPatientsAsync()
        {
            try
            {
                _logger.LogInformation(
                    "Retrieving patient data.");

                return await _context.Patients
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to retrieve patients.");

                throw;
            }
        }

        public async Task<Patient?>
            GetByIdAsync(int patientId)
        {
            return await _context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    p => p.PatientId == patientId);
        }

        public async Task<Patient?>
            GetTrackedByIdAsync(
                int patientId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(
                    p => p.PatientId == patientId);
        }

        public async Task UpdatePatientAsync(
            Patient patient)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<PatientByDeptIdResponse>> GetPatientsByDeptIdAsync(
            int physicianId,
            string? search)
        {
            var departmentId = await _context.Users

                .Where(u => u.UserId == physicianId)

                .Select(u => u.DeptId)

                .FirstOrDefaultAsync();

            var query = _context.Patients

                .AsNoTracking()

                .Where(p =>
                    p.DeptId == departmentId &&
                    p.IsActive == 1);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(p =>

                    p.FirstName.Contains(search) ||

                    p.LastName.Contains(search) ||

                    (p.FirstName + " " + p.LastName)
                        .Contains(search) ||

                    p.Mrn.Contains(search));
            }

            return await query

                .OrderBy(p => p.FirstName)
                .ThenBy(p => p.LastName)

                .Select(p => new PatientByDeptIdResponse
                {
                    PatientId = p.PatientId,

                    PatientName =
                        p.FirstName + " " + p.LastName,

                    Dob = p.Dob,

                    Status = p.IsActive,

                    DeptId = p.DeptId,

                    HasDecision =
                        _context.DispositionDecisions
                            .Any(d =>
                                d.PatientId == p.PatientId),

                    DecisionId =
                        _context.DispositionDecisions

                            .Where(d =>
                                d.PatientId == p.PatientId)

                            .OrderByDescending(d =>
                                d.DecisionDate)

                            .Select(d => (int?)d.DecisionId)

                            .FirstOrDefault()
                })

                .ToListAsync();
        }
    }
}