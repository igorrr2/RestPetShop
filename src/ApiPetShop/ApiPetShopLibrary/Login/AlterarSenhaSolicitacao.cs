using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPetShopLibrary.Login
{
    public class AlterarSenhaSolicitacao
    {
        public string Token { get; set; }

        public string SenhaAtual { get; set; }

        public string NovaSenha { get; set; }

        public string NovaSenhaConfirmacao { get; set; }
    }
}
