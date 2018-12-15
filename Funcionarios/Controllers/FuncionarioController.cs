using Funcionarios.Modelo;
using Funcionarios.Repositorio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Funcionarios.Controllers
{
    public class FuncionarioController : Controller
    {
        private FuncionarioDAO funcionarioDAO = new FuncionarioDAO();

        // GET: Funcionario
        public ActionResult Index()
        {
            var listaFuncionario = funcionarioDAO.Listar();
            return View(listaFuncionario);
        }

        // GET: Funcionario/Create
        public ActionResult CreateOrEdit(int codigo)
        {
            Funcionario funcionario;

            if (codigo != 0)
            {
                funcionario = funcionarioDAO.BuscarPorCodigo(codigo);
                if (funcionario == null)
                {
                    return new HttpNotFoundResult("Funcionário não localizado!");
                }
            }
            else
            {
                funcionario = new Funcionario();
                funcionario.Salario = 0;
            }
            return View(funcionario);
        }

        // POST: Funcionario/Create
        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(Funcionario novo)
        {
            if (ModelState.IsValid)
            {
                if (novo.Codigo == 0)
                    funcionarioDAO.Incluir(novo);
                else if (funcionarioDAO.Alterar(novo))
                {
                    var configStorage = ConfigurationManager.ConnectionStrings["ConexaoAzureStorage"].ConnectionString;
                    var funcionarioAzure = new FuncionarioAzure(configStorage);

                    await funcionarioAzure.salvandoAuditoria(novo.Codigo, novo.SalarioOriginal, DateTime.UtcNow.AddHours(-3));
                }

                return RedirectToAction("Index");
            }
            return View(novo);
        }

        // GET: Funcionario/Delete/5
        public ActionResult Delete(int codigo)
        {
            funcionarioDAO.Apagar(codigo);
            return RedirectToAction("Index");
        }
    }
}
