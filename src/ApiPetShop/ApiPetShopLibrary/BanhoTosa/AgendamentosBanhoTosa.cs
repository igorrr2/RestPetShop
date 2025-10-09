using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPetShopLibrary.BanhoTosa
{
    public class AgendamentosBanhoTosa
    {
        public Guid Id { get; set; }

        public Guid UsuarioId { get; set; }

        public DateTime DataAgendamento { get; set; }

        public string ModalidadeAgendamento { get; set; }

        public string Observacoes { get; set; }

        public Guid AnimalId { get; set; }

        public AgendamentosBanhoTosa() { }

        public AgendamentosBanhoTosa(Guid id, Guid usuarioId, Guid animalId, DateTime dataAgendamento, string modalidadeAgendamento, string observacoes)
        {
            ModalidadeAgendamento = modalidadeAgendamento;
            Observacoes = observacoes;
            Id = id;
            UsuarioId = usuarioId;
            DataAgendamento = dataAgendamento;
            AnimalId = animalId;
        }
    }
}
