using ApiPetShop.Models;
using ApiPetShopLibrary.Animal;
using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Util.MensagemRetorno;

namespace ApiPetShop.Repositories
{
    public class AnimalRepository
    {
        
        public static Mensagem TryAdd(Animal animal)
        {
            try
            {
                using var conexao = new MySqlConnection(Connection.PetShopConnectionString);
                conexao.Open();

                using var cmd = new MySqlCommand("Animal_Add", conexao);
                cmd.CommandType = CommandType.StoredProcedure;

                // GUID convertido para string
                cmd.Parameters.AddWithValue("@p_Id", animal.Id.ToString());
                cmd.Parameters.AddWithValue("@p_NomeAnimal", animal.NomeAnimal ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_NomeTutor", animal.NomeTutor ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_Raca", animal.Raca ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_Sexo", animal.Sexo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_DataNascimento", animal.DataNascimento);
                cmd.Parameters.AddWithValue("@p_Observacoes", animal.Observacoes ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_NumeroTelefoneTutor", animal.NumeroTelefoneTutor ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_UsuarioId", animal.UsuarioId.ToString());

                cmd.ExecuteNonQuery();
                return new Mensagem();
            }
            catch (Exception ex)
            {
                return new Mensagem(ex.Message, ex);
            }
        }

        public static Mensagem TryGet(out List<Animal> animais, Guid? id = null)
        {
            animais = new List<Animal>();

            try
            {
                using var conexao = new MySqlConnection(Connection.PetShopConnectionString);
                conexao.Open();

                using var cmd = new MySqlCommand("Animal_Get", conexao);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_Id", id?.ToString() ?? (object)DBNull.Value);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    animais.Add(new Animal
                    {
                        Id = Guid.Parse(reader["Id"].ToString()),
                        UsuarioId = Guid.Parse(reader["UsuarioId"].ToString()),
                        NomeAnimal = reader["NomeAnimal"].ToString(),
                        NomeTutor = reader["NomeTutor"].ToString(),
                        Raca = reader["Raca"].ToString(),
                        Sexo = reader["Sexo"].ToString(),
                        DataNascimento = Convert.ToDateTime(reader["DataNascimento"]),
                        Observacoes = reader["Observacoes"]?.ToString(),
                        NumeroTelefoneTutor = reader["NumeroTelefoneTutor"]?.ToString()
                    });
                }

                return new Mensagem();
            }
            catch (Exception ex)
            {
                return new Mensagem(ex.Message, ex);
            }
        }

        public static Mensagem TryUpdate(Animal animal)
        {
            try
            {
                using var conexao = new MySqlConnection(Connection.PetShopConnectionString);
                conexao.Open();

                using var cmd = new MySqlCommand("Animal_Update", conexao);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@p_Id", animal.Id.ToString());
                cmd.Parameters.AddWithValue("@p_NomeAnimal", animal.NomeAnimal ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_NomeTutor", animal.NomeTutor ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_Raca", animal.Raca ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_Sexo", animal.Sexo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_DataNascimento", animal.DataNascimento);
                cmd.Parameters.AddWithValue("@p_Observacoes", animal.Observacoes ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_NumeroTelefoneTutor", animal.NumeroTelefoneTutor ?? (object)DBNull.Value);

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

                using var cmd = new MySqlCommand("Animal_Delete", conexao);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_Id", id.ToString());
                cmd.ExecuteNonQuery();

                return new Mensagem();
            }
            catch (Exception ex)
            {
                return new Mensagem(ex.Message, ex);
            }
        }

        public static Mensagem TryGetByUsuarioId(Guid usuarioId, out List<Animal> animais)
        {
            animais = new List<Animal>();

            try
            {
                using var conexao = new MySqlConnection(Connection.PetShopConnectionString);
                conexao.Open();

                using var cmd = new MySqlCommand("SELECT * FROM Animais_View WHERE UsuarioId = @UsuarioId", conexao);
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId.ToString());

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    animais.Add(new Animal
                    {
                        Id = Guid.Parse(reader["Id"].ToString()),
                        NomeAnimal = reader["NomeAnimal"].ToString(),
                        NomeTutor = reader["NomeTutor"].ToString(),
                        Raca = reader["Raca"].ToString(),
                        Sexo = reader["Sexo"].ToString(),
                        DataNascimento = Convert.ToDateTime(reader["DataNascimento"]),
                        Observacoes = reader["Observacoes"]?.ToString(),
                        NumeroTelefoneTutor = reader["NumeroTelefoneTutor"]?.ToString()
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
