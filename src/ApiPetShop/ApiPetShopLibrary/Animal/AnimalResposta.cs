using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPetShopLibrary.Animal
{
    public class AnimalResposta
    {
        public int StatusCode { get; set; }
        public string MensagemRetorno { get; set; }
        public string? Id { get; set; } 
        public List<AnimalDto> Animais { get; set; }
    }
}
