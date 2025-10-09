using ApiPetShop.Models;
using ApiPetShop.Repositories;
using ApiPetShopLibrary.Animal;
using Microsoft.AspNetCore.Mvc;
using Util.Criptografia;
using Util.MensagemRetorno;

namespace ApiPetShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalController : ControllerBase
    {
        [HttpGet("ListarAnimais")]
        public IActionResult Listar([FromQuery] AnimalListarSolicitacao solicitacao)
        {
            AnimalResposta resposta = new AnimalResposta();

            if (solicitacao == null || string.IsNullOrWhiteSpace(solicitacao.Token))
            {
                resposta.StatusCode = 401;
                resposta.MensagemRetorno = "Token do usuário não informado";
                return Ok(resposta);
            }

            Mensagem mensagem = Autenticacao.ValidarAutenticacao(solicitacao.Token, out List<Usuario> usuario);
            if (!mensagem.Sucesso || usuario == null || usuario.Count == 0)
            {
                resposta.StatusCode = 401;
                resposta.MensagemRetorno = "Usuário não encontrado ou não autenticado";
                return Ok(resposta);
            }

            mensagem = AnimalRepository.TryGetByUsuarioId(usuario[0].Id, out List<Animal> animais);
            if (!mensagem.Sucesso)
            {
                resposta.StatusCode = 500;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }
            mensagem = PreencherResposta(animais, out List<AnimalDto> listaAnimais);
            if (!mensagem.Sucesso)
            {
                resposta.StatusCode = 500;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }

            resposta.StatusCode = 200;
            resposta.MensagemRetorno = "Sucesso";
            resposta.Animais = listaAnimais;
            return Ok(resposta);
        }

        [HttpPost("CadastrarAnimal")]
        public IActionResult Criar([FromBody] AnimalSolicitacao solicitacao)
        {
            AnimalResposta resposta = new AnimalResposta();

            if (solicitacao == null || string.IsNullOrWhiteSpace(solicitacao.Token) || solicitacao.Animal == null)
            {
                resposta.StatusCode = 401;
                resposta.MensagemRetorno = "Token e dados do animal são obrigatórios";
                return Ok(resposta);
            }

            Mensagem mensagem = Autenticacao.ValidarAutenticacao(solicitacao.Token, out List<Usuario> usuario);
            if (!mensagem.Sucesso || usuario == null || usuario.Count == 0)
            {
                resposta.StatusCode = 401;
                resposta.MensagemRetorno = "Usuário não encontrado ou não autenticado";
                return Ok(resposta);
            }
            Animal animal = new Animal(solicitacao.Animal.NomeAnimal, solicitacao.Animal.NomeTutor, solicitacao.Animal.Raca, solicitacao.Animal.Sexo, solicitacao.Animal.DataNascimento, solicitacao.Animal.Observacoes, solicitacao.Animal.NumeroTelefoneTutor, Guid.NewGuid(), usuario[0].Id);

            mensagem = AnimalRepository.TryAdd(animal);
            if (!mensagem.Sucesso)
            {
                resposta.StatusCode = 500;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }
            ShortGuid animalId = animal.Id;
            resposta.StatusCode = 200;
            resposta.MensagemRetorno = "Animal criado com sucesso";
            resposta.Id = animalId;
            return Ok(resposta);
        }

        [HttpPost("AtualizarAnimal")]
        public IActionResult Atualizar([FromBody] AnimalSolicitacao solicitacao)
        {
            AnimalResposta resposta = new AnimalResposta();

            if (solicitacao == null || string.IsNullOrWhiteSpace(solicitacao.Token) || solicitacao.Animal == null || string.IsNullOrEmpty(solicitacao.Animal.AnimalId))
            {
                resposta.StatusCode = 401;
                resposta.MensagemRetorno = "Token e dados do animal são obrigatórios";
                return Ok(resposta);
            }

            ShortGuid token = solicitacao.Token;
            var msgUsuario = UsuarioRepository.TryGetByToken(token, out List<Usuario> usuario);
            if (!msgUsuario.Sucesso || usuario.Count == 0)
            {
                resposta.StatusCode = 401;
                resposta.MensagemRetorno = "Usuário não encontrado ou não autenticado";
                return Ok(resposta);
            }
            ShortGuid animalId = solicitacao.Animal.AnimalId;

            Animal animal = new Animal(solicitacao.Animal.NomeAnimal, solicitacao.Animal.NomeTutor, solicitacao.Animal.Raca, solicitacao.Animal.Sexo, solicitacao.Animal.DataNascimento, solicitacao.Animal.Observacoes, solicitacao.Animal.NumeroTelefoneTutor, animalId, usuario[0].Id);

            var msgAtualizar = AnimalRepository.TryUpdate(animal);
            if (!msgAtualizar.Sucesso)
            {
                resposta.StatusCode = 500;
                resposta.MensagemRetorno = msgAtualizar.Descricao;
                return Ok(resposta);
            }

            resposta.StatusCode = 200;
            resposta.MensagemRetorno = "Animal atualizado com sucesso";
            resposta.Id = animalId;
            return Ok(resposta);
        }

        [HttpPost("ApagarAnimal")]
        public IActionResult Deletar([FromBody] AnimalApagarSolicitacao solicitacao)
        {
            AnimalResposta resposta = new AnimalResposta();

            if (solicitacao == null || string.IsNullOrWhiteSpace(solicitacao.Token) || string.IsNullOrEmpty(solicitacao.AnimalId))
            {
                resposta.StatusCode = 401;
                resposta.MensagemRetorno = "Token e dados do animal são obrigatórios";
                return Ok(resposta);
            }

            ShortGuid token = solicitacao.Token;
            var msgUsuario = UsuarioRepository.TryGetByToken(token, out List<Usuario> usuario);
            if (!msgUsuario.Sucesso || usuario.Count == 0)
            {
                resposta.StatusCode = 401;
                resposta.MensagemRetorno = "Usuário não encontrado ou não autenticado";
                return Ok(resposta);
            }
            ShortGuid animalId = solicitacao.AnimalId;

            var msgAtualizar = AnimalRepository.TryDelete(animalId);
            if (!msgAtualizar.Sucesso)
            {
                resposta.StatusCode = 500;
                resposta.MensagemRetorno = msgAtualizar.Descricao;
                return Ok(resposta);
            }

            resposta.StatusCode = 200;
            resposta.MensagemRetorno = "Animal apagado com sucesso";
            resposta.Id = animalId;
            return Ok(resposta);
        }
        public static Mensagem PreencherResposta(List<Animal> animais, out List<AnimalDto> listaAnimais)
        {
            listaAnimais = new List<AnimalDto>();
            try
            {
                listaAnimais = animais.Select(a => new AnimalDto
                {
                    AnimalId = new ShortGuid(a.Id).ToString(),
                    NomeAnimal = a.NomeAnimal,
                    NomeTutor = a.NomeTutor,
                    Raca = a.Raca,
                    Sexo = a.Sexo,
                    DataNascimento = a.DataNascimento,
                    Observacoes = a.Observacoes,
                    NumeroTelefoneTutor = a.NumeroTelefoneTutor
                }).ToList();

                return new Mensagem();
                
            }catch (Exception ex)
            {
                return new Mensagem(ex.Message, ex);
            }
        }
    }
}

