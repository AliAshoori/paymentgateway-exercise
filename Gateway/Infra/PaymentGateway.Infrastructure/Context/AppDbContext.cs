using System.Reflection;
using EntityFrameworkCore.EncryptColumn;
using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Initialize.EncryptionKey = "E546C8DF278CD5931069B522E695D4F2"; // in real world, to be replaced with something secure and to be read for example from key vault 
            IEncryptionProvider provider = new GenerateEncryptionProvider();

            modelBuilder.UseEncryption(provider);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).GetTypeInfo().Assembly);
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }
    }
}
