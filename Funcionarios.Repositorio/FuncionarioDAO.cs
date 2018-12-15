using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funcionarios.Modelo;

namespace Funcionarios.Repositorio
{
    public class FuncionarioDAO
    {
        private Contexto contexto = new Contexto();

        public List<Funcionario> Listar()
        {
            var listagem = contexto.Funcionarios.ToList();

            return listagem;
        }

        public bool Incluir(Funcionario novo)
        {
            contexto.Entry<Funcionario>(novo).State = EntityState.Added;
            return (contexto.SaveChanges() > 0);
        }

        public bool Alterar(Funcionario funcionario)
        {
            contexto.Entry<Funcionario>(funcionario).State = EntityState.Modified;
            return (contexto.SaveChanges() > 0);
        }

        public Funcionario BuscarPorCodigo(int codigo)
        {
            var funcionario = contexto.Funcionarios.Where(f => f.Codigo == codigo).FirstOrDefault();
            return funcionario;
        }

        public bool Apagar(int codigo)
        {
            var funcionario = contexto.Funcionarios.Where(f => f.Codigo == codigo).FirstOrDefault();
            if (funcionario != null)
            {
                contexto.Entry<Funcionario>(funcionario).State = EntityState.Deleted;
            }
            return (contexto.SaveChanges() > 0);
        }
    }
}
