using Microsoft.EntityFrameworkCore;
using MvcNetCoreProceduresEF.Models;


namespace MvcNetCoreProceduresEF.Data
{
    public class EnfermoContext:DbContext
    {
        public EnfermoContext(DbContextOptions<EnfermoContext> options)
            : base(options){ }
        public DbSet<Enfermo> Enfermos { get; set; }
    }
}
