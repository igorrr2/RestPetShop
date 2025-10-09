using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPetShopLibrary.Animal
{
    public class Animal
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public string NomeAnimal { get; set; }
        public string NomeTutor { get; set; }
        public string Raca { get; set; }
        public string Sexo { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Observacoes { get; set; }
        public string NumeroTelefoneTutor { get; set; }

        public Animal() { }

        public Animal(string nomeAnimal, string nomeTutor, string raca, string sexo, DateTime dataNascimento, string observacoes, string nuoerotelefoneTutor, Guid id, Guid usuarioId)
        {
            NomeAnimal = nomeAnimal;
            NomeTutor = nomeTutor;
            Raca = raca;
            Sexo = sexo;
            DataNascimento = dataNascimento;
            Observacoes = observacoes;
            NumeroTelefoneTutor = nuoerotelefoneTutor;
            Id = id;
            UsuarioId = usuarioId;
        }
    }
}
