using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCorePaginacionRegistros.Models
{
    [Table("V_EMPLEADOS_INDIVIDUAL")]
    public class VistaEmpleado
    {
        [Key]
        [Column("EMP_NO")]
        public int IdEmpleado { get; set; }
        [Column("APELLIDO")]
        public string Apellido { get; set; }
        [Column("OFICIO")]
        public string Oficio { get; set; }
        [Column("SALARIO")]
        public int Salario { get; set; }
        [Column("DEPT_NO")]
        public int Dept_no { get; set; }
        [Column("POSICION")]
        public int Posicion { get; set; }
    }
}
