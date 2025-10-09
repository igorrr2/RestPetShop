using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPetShopLibrary.Animal
{

    public class AnimalSolicitacao
    {
        public string Token { get; set; }
        public AnimalDto Animal { get; set; } // Para criar/atualizar
    }
}

