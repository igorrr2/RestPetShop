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

            Mensagem mensagem = UsuarioRepository.TryGetByLogin(solicitacao.Login, out List<Models.Usuario> usuario);

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


            Mensagem mensagem = UsuarioRepository.TryGetByToken(token, out List<Models.Usuario> usuario);

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

            Mensagem mensagem = UsuarioRepository.TryGetByToken(token, out List<Models.Usuario> usuario);

            if (!mensagem.Sucesso)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }

            if (usuario == null || usuario.Count == 0)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Usuário não encontrado ou não autenticado";
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

        [HttpPost("CriarUsuario")]
        public IActionResult CriarUsuario([FromBody] CriarUsuarioSolicitacao solicitacao)
        {
            AlterarSenhaResposta resposta = new AlterarSenhaResposta();

            if (solicitacao == null || string.IsNullOrEmpty(solicitacao.NomeUsuario) || string.IsNullOrEmpty(solicitacao.Senha) || string.IsNullOrEmpty(solicitacao.login))
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Informe todos os campos para criação do usuário";
                return Ok(resposta);
            }

            Models.Usuario usuario = new Models.Usuario();
            usuario.Nome = solicitacao.NomeUsuario;
            usuario.Login = solicitacao.login;
            usuario.Token = Guid.Empty.ToString();
            Criptografia.Criptografar(solicitacao.Senha, usuario.Id, out string senhaCriptografada);
            usuario.Senha = senhaCriptografada;
            Mensagem mensagem = UsuarioRepository.TryAdd(usuario);
            if (!mensagem.Sucesso)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }

            resposta.statusCode = 200;
            resposta.MensagemRetorno = "Usuario cadastrado com sucesso";

            return Ok(resposta);
        }

        [HttpPost("AtualizarUsuario")]
        public IActionResult AtualizarUsuario([FromBody] AtualizarUsuarioSolicitacao solicitacao)
        {
            AlterarSenhaResposta resposta = new AlterarSenhaResposta();

            if (solicitacao == null || string.IsNullOrEmpty(solicitacao.NomeUsuario) || string.IsNullOrEmpty(solicitacao.login) || string.IsNullOrEmpty(solicitacao.IdUsuarioAtualizar))
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Informe todos os campos para criação do usuário";
                return Ok(resposta);
            }
            ShortGuid usuarioId = solicitacao.IdUsuarioAtualizar;
            Mensagem mensagem = UsuarioRepository.TryGet(out List<Models.Usuario> usuario, usuarioId);
            if (!mensagem.Sucesso)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }
            if (usuario == null || usuario.Count == 0)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Usuário não encontrado no banco de dados";
                return Ok(resposta);
            }

            usuario[0].Nome = solicitacao.NomeUsuario;
            usuario[0].Login = solicitacao.login;
            mensagem = UsuarioRepository.TryUpdate(usuario[0]);
            if (!mensagem.Sucesso)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }

            resposta.statusCode = 200;
            resposta.MensagemRetorno = "Usuario atualizado com sucesso";

            return Ok(resposta);
        }

        [HttpPost("ResetarSenha")]
        public IActionResult ResetarSenhaUsuario([FromBody] AtualizarUsuarioSolicitacao solicitacao)
        {
            AlterarSenhaResposta resposta = new AlterarSenhaResposta();

            if (solicitacao == null || string.IsNullOrEmpty(solicitacao.Senha) || string.IsNullOrEmpty(solicitacao.ConfirmacaoSenha))
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Informe todos os campos para criação do usuário";
                return Ok(resposta);
            }
            if (solicitacao.Senha != solicitacao.ConfirmacaoSenha)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Senhas não conferem";
                return Ok(resposta);
            }

            ShortGuid usuarioId = solicitacao.IdUsuarioAtualizar;
            Mensagem mensagem = UsuarioRepository.TryGet(out List<Models.Usuario> usuario, usuarioId);
            if (!mensagem.Sucesso)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }
            if (usuario == null || usuario.Count == 0)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = "Usuário não encontrado no banco de dados";
                return Ok(resposta);
            }

            Criptografia.Criptografar(solicitacao.Senha, usuario[0].Id, out string senhaCriptografada);
            usuario[0].Senha = senhaCriptografada;
            usuario[0].Token = Guid.Empty.ToString();
            mensagem = UsuarioRepository.TryUpdate(usuario[0]);
            if (!mensagem.Sucesso)
            {
                resposta.statusCode = 401;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }

            resposta.statusCode = 200;
            resposta.MensagemRetorno = "Senha resetada com sucesso";

            return Ok(resposta);
        }

        [HttpGet("ListarUsuarios")]
        public IActionResult ListarUsuarios()
        {
            UsuariosResposta resposta = new UsuariosResposta();

            Mensagem mensagem = UsuarioRepository.TryObterTodosUsuarios(out List<Models.Usuario> usuario);
            if (!mensagem.Sucesso)
            {
                resposta.StatusCode = 401;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }
            if (usuario == null || usuario.Count == 0)
            {
                resposta.StatusCode = 401;
                resposta.MensagemRetorno = "Usuário não encontrado no banco de dados";
                return Ok(resposta);
            }
            List<ApiPetShopLibrary.Login.Usuario> usuariosCopia = usuario
               .Select(u => new ApiPetShopLibrary.Login.Usuario
               {
                   Id = ToShortGuid(u.Id), // conversão Guid -> ShortGuid
                   Nome = u.Nome,
                   Login = u.Login,
                   Senha = u.Senha,
                   Token = u.Token
               })
               .ToList();

            resposta.StatusCode = 200;
            resposta.MensagemRetorno = "";
            resposta.Usuarios = usuariosCopia;
            return Ok(resposta);
        }
        private ShortGuid ToShortGuid(Guid id)
        {
            ShortGuid idShortGuid = id;
            return idShortGuid;
        }
    }


}
