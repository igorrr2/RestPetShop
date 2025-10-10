using ApiPetShop.Models;
using ApiPetShop.Repositories;
using ApiPetShopLibrary.Login;
using Microsoft.AspNetCore.Mvc;
using Util.Criptografia;
using Util.MensagemRetorno;

namespace ApiPetShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        [HttpPost("Logar")]
        public IActionResult Login([FromBody] LoginSolicitacao solicitacao)
        {
            LoginResposta resposta = new LoginResposta();

            if (solicitacao == null || string.IsNullOrWhiteSpace(solicitacao.Login) || string.IsNullOrWhiteSpace(solicitacao.Senha))
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Login e senha são obrigatórios.";
                return Ok(resposta);
            }

            Mensagem mensagem = UsuarioRepository.TryGetByLogin(solicitacao.Login, out List<Usuario> usuario);

            if (!mensagem.Sucesso)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }

            if (usuario == null || usuario.Count == 0)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Usuário não encontrado";
                return Ok(resposta);
            }

            mensagem = Criptografia.Descriptografar(usuario[0].Senha, usuario[0].Id, out string senhaDescriptografada);
            if (!mensagem.Sucesso)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }
            if (senhaDescriptografada != solicitacao.Senha)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Usuário ou senha inválidos";
                return Ok(resposta);
            }

            if (usuario[0].Token == Guid.Empty.ToString())
                usuario[0].Token = Guid.NewGuid().ToString();

            mensagem = UsuarioRepository.TryUpdate(usuario[0]);
            if (!mensagem.Sucesso)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Não foi possível gerar token de autenticação para o usuário";
                return Ok(resposta);
            }

            Guid.TryParse(usuario[0].Token, out Guid token);
            ShortGuid tokenShortGuid = token;
            ShortGuid usuarioIdShortGuid = usuario[0].Id;

            resposta.TokenAutenticacao = tokenShortGuid;
            resposta.UsuarioNome = usuario[0].Nome;
            resposta.UsuarioId = usuarioIdShortGuid;
            resposta.statusCode = 200;

            return Ok(resposta);
        }


        [HttpPost("Deslogar")]
        public IActionResult Deslogar([FromBody] DeslogarSolicitacao solicitacao)
        {
            DeslogarResposta resposta = new DeslogarResposta();
            ShortGuid token;
            if (solicitacao == null || string.IsNullOrWhiteSpace(solicitacao.Token))
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Token do Usuário autenticado não foi informado";
                return Ok(resposta);
            }
            token = solicitacao.Token;


            Mensagem mensagem = UsuarioRepository.TryGetByToken(token, out List<Usuario> usuario);

            if (!mensagem.Sucesso)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }

            if (usuario == null || usuario.Count == 0)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Usuário não encontrado";
                return Ok(resposta);
            }

            usuario[0].Token = Guid.Empty.ToString();
            mensagem = UsuarioRepository.TryUpdate(usuario[0]);
            if (!mensagem.Sucesso)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Não foi possível gerar token de autenticação para o usuário";
                return Ok(resposta);
            }

            resposta.statusCode = 200;
            resposta.MensagemRetorno = string.Empty;

            return Ok(resposta);
        }


        [HttpPost("AlterarSenha")]
        public IActionResult AlterarSenha([FromBody] AlterarSenhaSolicitacao solicitacao)
        {
            AlterarSenhaResposta resposta = new AlterarSenhaResposta();
            ShortGuid token;
            if (solicitacao == null || string.IsNullOrWhiteSpace(solicitacao.Token))
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Token do Usuário autenticado não foi informado";
                return Ok(resposta);
            }
            token = solicitacao.Token;

            Mensagem mensagem = UsuarioRepository.TryGetByToken(token, out List<Usuario> usuario);

            if (!mensagem.Sucesso)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }

            if (usuario == null || usuario.Count == 0)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Usuário não encontrado";
                return Ok(resposta);
            }

            Criptografia.Descriptografar(usuario[0].Senha, usuario[0].Id, out string senhaAtualDescriptografada);
            if(solicitacao.SenhaAtual != senhaAtualDescriptografada)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Senha informada não confere com a senha atual do usuário";
                return Ok(resposta);
            }

            if(solicitacao.NovaSenha != solicitacao.NovaSenhaConfirmacao)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Senhas informadas não são iguais";
                return Ok(resposta);
            }
            Criptografia.Criptografar(solicitacao.NovaSenha, usuario[0].Id, out string novaSenhaCriptografaca);
            usuario[0].Senha = novaSenhaCriptografaca;
            mensagem = UsuarioRepository.TryUpdate(usuario[0]);
            if (!mensagem.Sucesso)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }

            resposta.statusCode = 200;
            resposta.MensagemRetorno = "Senha atualizada com sucesso";

            return Ok(resposta);
        }
    }


}
