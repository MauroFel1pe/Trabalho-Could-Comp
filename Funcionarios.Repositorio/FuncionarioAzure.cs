using Funcionarios.Modelo;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Repositorio
{
    public class FuncionarioAzure
    {
        private const string QUEUE = "maurofelipe";
        private const string TABLE = "HistoricoSalario";

        private CloudStorageAccount conta = null;

        public FuncionarioAzure(string configuracaoConta)
        {
            conta = CloudStorageAccount.Parse(configuracaoConta);
        }

        public async Task<string> salvandoAuditoria(int codigo, decimal salarioOriginal, DateTime dataAlteracao)
        {
            var mensagem = "";

                try
                {
                    var info = new FuncionarioInfo()
                    {
                        Codigo = codigo,
                        SalarioAntigo = salarioOriginal,
                        dataAlteracao = dataAlteracao
                    };

                    mensagem = JsonConvert.SerializeObject(info);

                    var client = conta.CreateCloudQueueClient();
                    var fila = client.GetQueueReference(QUEUE);

                    fila.CreateIfNotExists();

                    await fila.AddMessageAsync(
                        new CloudQueueMessage(mensagem));
                }
                catch (Exception ex)
                {
                    mensagem = ex.Message;
                }
            
            return mensagem;
        }

        public string SalvarHistoricoAlteracao(FuncionarioInfo info)
        {
            var mensagemErro = "";

            try
            {
                // acessando table storage
                CloudTableClient tableClient = conta.CreateCloudTableClient();
                CloudTable cloudTable = tableClient.GetTableReference(TABLE);
                cloudTable.CreateIfNotExists();

                var historicoSalario = new HistoricoSalario(info.Codigo)
                {
                    Salario = info.SalarioAntigo,
                    Timestamp = DateTime.UtcNow.AddHours(-3),
                    Mensagem = "Processamento realizado com sucesso!"
                };

                TableOperation inclusao = TableOperation.Insert(historicoSalario);
                cloudTable.Execute(inclusao);
            }
            catch (Exception e)
            {
                mensagemErro = "Erro ao executar a tarefa! Erro: " + e.Message;
            }
            return mensagemErro;
        }
    }
}
