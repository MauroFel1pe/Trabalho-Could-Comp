using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Funcionarios.Modelo
{
    [Table("Funcionario")]
    public class Funcionario
    {
        [Key()]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Código")]
        public int Codigo { get; set; }
        [Required()]
        public string Nome { get; set; }
        [Required(), DisplayName("Salário")]
        public decimal Salario { get; set; }
        [NotMapped]
        public decimal SalarioOriginal { get; set; }

        public Funcionario()
        {
            SalarioOriginal = Salario;
        }
    }
}
