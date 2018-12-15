using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Funcionarios.Repositorio;

namespace FuncionarioWebJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("maurofelipe")] FuncionarioInfo informacoes, TextWriter log)
        {
            string resposta = null;

            string configuracaostorage = ConfigurationManager.
                ConnectionStrings["AzureWebJobsStorage"].ConnectionString;

            var funcionarioAzure = new FuncionarioAzure(configuracaostorage);

            // Criar tarefa para execução de conteúdo assíncrono
            Task.Run(() =>
            {
              resposta = funcionarioAzure.SalvarHistoricoAlteracao(informacoes);
            }).Wait();

            if (!string.IsNullOrEmpty(resposta))
                log.WriteLine(resposta);
        }
    }
}
