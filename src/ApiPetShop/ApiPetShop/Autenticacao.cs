using ApiPetShop.Models;
using ApiPetShop.Repositories;
using Util.Criptografia;
using Util.MensagemRetorno;

namespace ApiPetShop
{
    public class Autenticacao
    {
        public static Mensagem ValidarAutenticacao(string token, out List<Usuario> usuarioAutenticado)
        {

            ShortGuid tokenShotGuid = token;
            usuarioAutenticado = null;
            Mensagem mensagem = UsuarioRepository.TryGetByToken(tokenShotGuid, out usuarioAutenticado);
            if (!mensagem.Sucesso)
                return mensagem;

            return mensagem;
        }
    }
}
