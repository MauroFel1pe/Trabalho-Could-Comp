using System;
using Funcionarios.Modelo;
using System.Data.Entity;

namespace Funcionarios.Repositorio
{
    public class Contexto : DbContext
    {
        public Contexto() : base("name=ConexaoBanco") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Funcionario> Funcionarios { get; set; }
    }
}
