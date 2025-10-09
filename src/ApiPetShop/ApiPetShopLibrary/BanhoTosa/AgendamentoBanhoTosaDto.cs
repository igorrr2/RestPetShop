using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPetShopLibrary.BanhoTosa
{
    public class AgendamentoBanhoTosaDto
    {
        public string AgendamentolId { get; set; }

        public string AnimalId { get; set; }
        public DateTime DataAgendamento { get; set; }
        public string ModalidadeAgendamento { get; set; }
        public string Observacoes { get; set; }
    }
}
