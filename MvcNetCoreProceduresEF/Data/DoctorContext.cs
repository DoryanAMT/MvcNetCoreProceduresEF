using Microsoft.EntityFrameworkCore;
using MvcNetCoreProceduresEF.Models;

namespace MvcNetCoreProceduresEF.Data
{
    public class DoctorContext:DbContext
    {
        public DoctorContext(DbContextOptions<DoctorContext> options)
            :base(options){ }
        public DbSet<Doctor> Doctores { get; set; }
    }
}
