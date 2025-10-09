using ApiPetShop.Data;
using ApiPetShop.Models;
using ApiPetShop.Repositories;
using ApiPetShopLibrary.Animal;
using ApiPetShopLibrary.BanhoTosa;
using Microsoft.AspNetCore.Mvc;
using Util.Criptografia;
using Util.MensagemRetorno;

namespace ApiPetShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BanhoTosaController : ControllerBase
    {
        [HttpGet("ListarAgendamentosBanhoTosa")]
        public IActionResult Listar([FromQuery] AgendamentoBanhoTosaListarSolicitacao solicitacao)
        {
            AgendamentoBanhoTosaResposta resposta = new AgendamentoBanhoTosaResposta();

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

            mensagem = AgendamentosBanhoTosaRepository.TryGetByUsuarioId(usuario[0].Id, out List<AgendamentosBanhoTosa> agendamentos);
            if (!mensagem.Sucesso)
            {
                resposta.StatusCode = 500;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }

            mensagem = PreencherResposta(agendamentos, out List<AgendamentoBanhoTosaDto> listaAgendamentos);
            if (!mensagem.Sucesso)
            {
                resposta.StatusCode = 500;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }

            resposta.StatusCode = 200;
            resposta.MensagemRetorno = "Sucesso";
            resposta.Agendamentos = listaAgendamentos;
            return Ok(resposta);
        }

        [HttpPost("CadastrarAgendamentoBanhoTosa")]
        public IActionResult Criar([FromBody] AgendamentoBanhoTosaSolicitacao solicitacao)
        {
            AgendamentoBanhoTosaResposta resposta = new AgendamentoBanhoTosaResposta();

            if (solicitacao == null || string.IsNullOrWhiteSpace(solicitacao.Token) || solicitacao.Agendamento == null)
            {
                resposta.StatusCode = 401;
                resposta.MensagemRetorno = "Token e dados para o agendamento são obrigatórios";
                return Ok(resposta);
            }

            Mensagem mensagem = Autenticacao.ValidarAutenticacao(solicitacao.Token, out List<Usuario> usuario);
            if (!mensagem.Sucesso || usuario == null || usuario.Count == 0)
            {
                resposta.StatusCode = 401;
                resposta.MensagemRetorno = "Usuário não encontrado ou não autenticado";
                return Ok(resposta);
            }
            ShortGuid animalId = solicitacao.Agendamento.AnimalId;
            AgendamentosBanhoTosa agendamento = new AgendamentosBanhoTosa(Guid.NewGuid(), usuario[0].Id, animalId, solicitacao.Agendamento.DataAgendamento, solicitacao.Agendamento.ModalidadeAgendamento, solicitacao.Agendamento.Observacoes);

            mensagem = AgendamentosBanhoTosaRepository.TryAdd(agendamento);
            if (!mensagem.Sucesso)
            {
                resposta.StatusCode = 500;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }
            ShortGuid agendamentolId = agendamento.Id;
            resposta.StatusCode = 200;
            resposta.MensagemRetorno = "Agendamento criado com sucesso";
            resposta.Id = agendamentolId;
            return Ok(resposta);
        }

        [HttpPost("AtualizarAgendamentoBanhoTosa")]
        public IActionResult Atualizar([FromBody] AgendamentoBanhoTosaSolicitacao solicitacao)
        {
            AgendamentoBanhoTosaResposta resposta = new AgendamentoBanhoTosaResposta();
            Mensagem mensagem = new Mensagem();
            if (solicitacao == null || string.IsNullOrWhiteSpace(solicitacao.Token) || solicitacao.Agendamento == null)
            {
                resposta.StatusCode = 401;
                resposta.MensagemRetorno = "Token e dados para o agendamento são obrigatórios";
                return Ok(resposta);
            }

            ShortGuid token = solicitacao.Token;
            mensagem = UsuarioRepository.TryGetByToken(token, out List<Usuario> usuario);
            if (!mensagem.Sucesso || usuario.Count == 0)
            {
                resposta.StatusCode = 401;
                resposta.MensagemRetorno = "Usuário não encontrado ou não autenticado";
                return Ok(resposta);
            }
            ShortGuid agendamentoId = solicitacao.Agendamento.AgendamentolId;
            ShortGuid animalId = solicitacao.Agendamento.AnimalId;
            AgendamentosBanhoTosa agendamento = new AgendamentosBanhoTosa(agendamentoId, usuario[0].Id, animalId, solicitacao.Agendamento.DataAgendamento, solicitacao.Agendamento.ModalidadeAgendamento, solicitacao.Agendamento.Observacoes);

            mensagem = AgendamentosBanhoTosaRepository.TryUpdate(agendamento);
            if (!mensagem.Sucesso)
            {
                resposta.StatusCode = 500;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }

            resposta.StatusCode = 200;
            resposta.MensagemRetorno = "Agendamento atualizado com sucesso";
            resposta.Id = agendamentoId;
            return Ok(resposta);
        }

        [HttpPost("ApagarAgendamentoBanhoTosa")]
        public IActionResult Deletar([FromBody] AgendamentoBanhoTosaApagarSolicitacao solicitacao)
        {
            AgendamentoBanhoTosaResposta resposta = new AgendamentoBanhoTosaResposta();
            Mensagem mensagem = new Mensagem();

            if (solicitacao == null || string.IsNullOrWhiteSpace(solicitacao.Token) || string.IsNullOrEmpty(solicitacao.AgendamentoId))
            {
                resposta.StatusCode = 401;
                resposta.MensagemRetorno = "Token e dados do Agendamento são obrigatórios";
                return Ok(resposta);
            }

            ShortGuid token = solicitacao.Token;
            mensagem = UsuarioRepository.TryGetByToken(token, out List<Usuario> usuario);
            if (!mensagem.Sucesso || usuario.Count == 0)
            {
                resposta.StatusCode = 401;
                resposta.MensagemRetorno = "Usuário não encontrado ou não autenticado";
                return Ok(resposta);
            }
            ShortGuid agendamentoId = solicitacao.AgendamentoId;

            mensagem = AgendamentosBanhoTosaRepository.TryDelete(agendamentoId);
            if (!mensagem.Sucesso)
            {
                resposta.StatusCode = 500;
                resposta.MensagemRetorno = mensagem.Descricao;
                return Ok(resposta);
            }

            resposta.StatusCode = 200;
            resposta.MensagemRetorno = "Agendamento apagado com sucesso";
            resposta.Id = agendamentoId;
            return Ok(resposta);
        }
        public static Mensagem PreencherResposta(List<AgendamentosBanhoTosa> agendamentos, out List<AgendamentoBanhoTosaDto> listaAgendamentos)
        {
            listaAgendamentos = new List<AgendamentoBanhoTosaDto>();

            try
            {
                listaAgendamentos = agendamentos.Select(a => new AgendamentoBanhoTosaDto
                {
                    AgendamentolId = new ShortGuid(a.Id).ToString(),
                    DataAgendamento = a.DataAgendamento,
                    AnimalId = new ShortGuid(a.AnimalId).ToString(),
                    ModalidadeAgendamento = a.ModalidadeAgendamento,
                    Observacoes = a.Observacoes
                }).ToList();

                return new Mensagem(); 
            }
            catch (Exception ex)
            {
                return new Mensagem(ex.Message, ex);
            }
        }
    }
}

