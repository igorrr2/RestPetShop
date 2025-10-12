using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPetShopLibrary.Login
{
    public class UsuariosResposta
    {
        public int StatusCode { get; set; }

        public string MensagemRetorno { get; set; }
        public List<Usuario> Usuarios {  get; set; }
    }
}
