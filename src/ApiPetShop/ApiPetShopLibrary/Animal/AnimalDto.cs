using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPetShopLibrary.Animal
{
    public class AnimalDto
    {
        public string AnimalId { get; set; }
        public string NomeAnimal { get; set; }
        public string NomeTutor { get; set; }
        public string Raca { get; set; }
        public string Sexo { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Observacoes { get; set; }
        public string NumeroTelefoneTutor { get; set; }
    }
}
