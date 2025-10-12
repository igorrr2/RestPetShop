using ApiPetShopLibrary.BanhoTosa;
using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Util.MensagemRetorno;

namespace ApiPetShop.Data
{
    public class AgendamentosBanhoTosaRepository
    {
        public static Mensagem TryAdd(AgendamentosBanhoTosa agendamento)
        {
            try
            {
                using var conexao = new MySqlConnection(Connection.PetShopConnectionString);
                conexao.Open();

                using var cmd = new MySqlCommand("AgendamentosBanhoTosa_Add", conexao);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@p_Id", agendamento.Id);
                cmd.Parameters.AddWithValue("@p_DataAgendamento", agendamento.DataAgendamento);
                cmd.Parameters.AddWithValue("@p_AnimalId", agendamento.AnimalId);
                cmd.Parameters.AddWithValue("@p_ModalidadeAgendamento", agendamento.ModalidadeAgendamento);
                cmd.Parameters.AddWithValue("@p_Observacoes", (object?)agendamento.Observacoes ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_UsuarioId", agendamento.UsuarioId);

                cmd.ExecuteNonQuery();
                return new Mensagem();
            }
            catch (Exception ex)
            {
                return new Mensagem(ex.Message, ex);
            }
        }

        public static Mensagem TryGet(out List<AgendamentosBanhoTosa> agendamentos, Guid? id = null)
        {
            agendamentos = new List<AgendamentosBanhoTosa>();

            try
            {
                using var conexao = new MySqlConnection(Connection.PetShopConnectionString);
                conexao.Open();

                using var cmd = new MySqlCommand("AgendamentosBanhoTosa_Get", conexao);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@p_Id", (object?)id ?? DBNull.Value);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    agendamentos.Add(new AgendamentosBanhoTosa
                    {
                        Id = Guid.Parse(reader["Id"].ToString()),
                        DataAgendamento = Convert.ToDateTime(reader["DataAgendamento"]),
                        AnimalId = Guid.Parse(reader["AnimalId"].ToString()),
                        ModalidadeAgendamento = reader["ModalidadeAgendamento"].ToString(),
                        Observacoes = reader["Observacoes"].ToString(),
                        UsuarioId = Guid.Parse(reader["UsuarioId"].ToString())
                    });
                }

                return new Mensagem();
            }
            catch (Exception ex)
            {
                return new Mensagem(ex.Message, ex);
            }
        }

        public static Mensagem TryUpdate(AgendamentosBanhoTosa agendamento)
        {
            try
            {
                using var conexao = new MySqlConnection(Connection.PetShopConnectionString);
                conexao.Open();

                using var cmd = new MySqlCommand("AgendamentosBanhoTosa_Update", conexao);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@p_Id", agendamento.Id);
                cmd.Parameters.AddWithValue("@p_DataAgendamento", agendamento.DataAgendamento);
                cmd.Parameters.AddWithValue("@p_ModalidadeAgendamento", agendamento.ModalidadeAgendamento);
                cmd.Parameters.AddWithValue("@p_Observacoes", (object?)agendamento.Observacoes ?? DBNull.Value);

                cmd.ExecuteNonQuery();
                return new Mensagem();
            }
            catch (Exception ex)
            {
                return new Mensagem(ex.Message, ex);
            }
        }

        public static Mensagem TryDelete(Guid id)
        {
            try
            {
                using var conexao = new MySqlConnection(Connection.PetShopConnectionString);
                conexao.Open();

                using var cmd = new MySqlCommand("AgendamentosBanhoTosa_Delete", conexao);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_Id", id);
                cmd.ExecuteNonQuery();

                return new Mensagem();
            }
            catch (Exception ex)
            {
                return new Mensagem(ex.Message, ex);
            }
        }

        public static Mensagem TryDeleteByAnimalId(Guid animalId)
        {
            try
            {
                using var conexao = new MySqlConnection(Connection.PetShopConnectionString);
                conexao.Open();

                string sql = "DELETE FROM AgendamentosBanhoTosa WHERE AnimalId = @Id";

                using var cmd = new MySqlCommand(sql, conexao);
                cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = animalId.ToString();

                int linhasAfetadas = cmd.ExecuteNonQuery();

                return new Mensagem();
            }
            catch (Exception ex)
            {
                return new Mensagem(ex.Message, ex);
            }
        }

        public static Mensagem TryGetByUsuarioId(Guid usuarioId, out List<AgendamentosBanhoTosa> agendamentos)
        {
            agendamentos = new List<AgendamentosBanhoTosa>();

            try
            {
                using var conexao = new MySqlConnection(Connection.PetShopConnectionString);
                conexao.Open();

                using var cmd = new MySqlCommand(
                    "SELECT * FROM AgendamentosBanhoTosa_View WHERE UsuarioId = @UsuarioId", conexao);
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    agendamentos.Add(new AgendamentosBanhoTosa
                    {
                        Id = Guid.Parse(reader["Id"].ToString()),
                        DataAgendamento = Convert.ToDateTime(reader["DataAgendamento"]),
                        AnimalId = Guid.Parse(reader["AnimalId"].ToString()),
                        ModalidadeAgendamento = reader["ModalidadeAgendamento"].ToString(),
                        Observacoes = reader["Observacoes"].ToString(),
                        UsuarioId = Guid.Parse(reader["UsuarioId"].ToString())
                    });
                }

                return new Mensagem();
            }
            catch (Exception ex)
            {
                return new Mensagem(ex.Message, ex);
            }
        }
    }
}
