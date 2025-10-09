using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPetShopLibrary.BanhoTosa
{
    public class AgendamentoBanhoTosaResposta
    {
        public int StatusCode { get; set; }
        public string MensagemRetorno { get; set; }
        public string? Id { get; set; } 
        public List<AgendamentoBanhoTosaDto> Agendamentos{ get; set; }
    }
}
