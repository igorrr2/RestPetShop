using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPetShopLibrary.Login
{
    public class AtualizarUsuarioSolicitacao
    {
        public string IdUsuarioAtualizar { get; set; }

        public string NomeUsuario { get; set; }

        public string Senha { get; set; }

        public string ConfirmacaoSenha { get; set; }

        public string login { get; set; }

        public bool Ativo { get; set; }
    }
}
