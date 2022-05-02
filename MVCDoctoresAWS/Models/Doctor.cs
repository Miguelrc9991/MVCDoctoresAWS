using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCDoctoresAWS.Models
{
    public class Doctor
    {
        public string IdDoctor { get; set; }
        public string IdHospital { get; set; }
        public string Apellido { get; set; }
        public string Especialidad { get; set; }
        public int Salario { get; set; }
        public string Imagen { get; set; }
    }
}
