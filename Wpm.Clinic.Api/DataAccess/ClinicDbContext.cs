using Microsoft.EntityFrameworkCore;

namespace Wpm.Clinic.DataAccess;

public class ClinicDbContext(DbContextOptions<ClinicDbContext> options) : DbContext(options)
{
    public DbSet<Consultation> Consultations { get; set; }
}

public record Consultation(Guid Id,
    Guid PatientId,
    string PatientName,
    int PatientAge,
    DateTime StartTime);