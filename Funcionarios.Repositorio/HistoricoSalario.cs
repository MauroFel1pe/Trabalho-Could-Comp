using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Funcionarios.Repositorio
{
    public class HistoricoSalario : TableEntity
    {
        public HistoricoSalario() { }

        public HistoricoSalario(int codigo)
        {
            PartitionKey = codigo.ToString();
            RowKey = DateTime.Now.ToString("yyyyMMHHmmddfff");
        }

        public decimal Salario { get; set; }
        public string Mensagem { get; set; }
    }
}
