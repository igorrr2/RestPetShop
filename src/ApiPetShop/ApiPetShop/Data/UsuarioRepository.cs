using ApiPetShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Util.MensagemRetorno;

namespace ApiPetShop.Repositories
{
    public class UsuarioRepository
    {

        public static Mensagem TryAdd(Usuario usuario)
        {
            try
            {
                using var conexao = new MySqlConnection(Connection.PetShopConnectionString);
                conexao.Open();

                using var cmd = new MySqlCommand("Usuario_Add", conexao);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@p_Id", usuario.Id);
                cmd.Parameters.AddWithValue("@p_Nome", usuario.Nome);
                cmd.Parameters.AddWithValue("@p_Login", usuario.Login);
                cmd.Parameters.AddWithValue("@p_Senha", usuario.Senha);
                cmd.Parameters.AddWithValue("@p_Token", (object?)usuario.Token ?? DBNull.Value);

                cmd.ExecuteNonQuery();

                return new Mensagem();
            }
            catch (Exception ex)
            {
                return new Mensagem(ex.Message, ex);
            }
        }

        public static Mensagem TryGet(out List<Usuario> usuarios, Guid id)
        {
            usuarios = new List<Usuario>();
            try
            {
                using var conexao = new MySqlConnection(Connection.PetShopConnectionString);
                conexao.Open();

                using var cmd = new MySqlCommand("Usuario_Get", conexao);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@p_Id", id.ToString() ?? (object)DBNull.Value);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    usuarios.Add(new Usuario
                    {
                        Id = Guid.Parse(reader["Id"].ToString()),
                        Nome = reader["Nome"].ToString(),
                        Login = reader["Login"].ToString(),
                        Senha = reader["Senha"].ToString(),
                        Token = reader["Token"]?.ToString()
                    });
                }

                return new Mensagem();
            }
            catch (Exception ex)
            {
                return new Mensagem(ex.Message, ex);
            }
        }

        public static Mensagem TryUpdate(Usuario usuario)
        {
            try
            {
                using var conexao = new MySqlConnection(Connection.PetShopConnectionString);
                conexao.Open();

                using var cmd = new MySqlCommand("Usuario_Update", conexao);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@p_Id", usuario.Id.ToString());
                cmd.Parameters.AddWithValue("@p_Nome", usuario.Nome ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_Login", usuario.Login ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_Senha", usuario.Senha ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@p_Token", usuario.Token ?? (object)DBNull.Value);

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

                using var cmd = new MySqlCommand("Usuario_Delete", conexao);
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

        public static Mensagem TryGetByLogin(string login, out List<Usuario> usuarios)
        {
            usuarios = new List<Usuario>();
            try
            {
                using var conexao = new MySqlConnection(Connection.PetShopConnectionString);
                conexao.Open();

                using var cmd = new MySqlCommand("SELECT * FROM Usuarios_View WHERE Login = @Login", conexao);
                cmd.Parameters.AddWithValue("@Login", login);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    usuarios.Add(new Usuario
                    {
                        Id = Guid.Parse(reader["Id"].ToString()),
                        Nome = reader["Nome"].ToString(),
                        Login = reader["Login"].ToString(),
                        Senha = reader["Senha"].ToString(),
                        Token = reader["Token"]?.ToString()
                    });
                }

                if (usuarios.Count == 0)
                    return new Mensagem("Usuário não encontrado no banco de dados");

                return new Mensagem();
            }
            catch (Exception ex)
            {
                return new Mensagem(ex.Message, ex);
            }
        }

        public static Mensagem TryGetByToken(Guid token, out List<Usuario> usuarios)
        {
            usuarios = new List<Usuario>();
            try
            {
                using var conexao = new MySqlConnection(Connection.PetShopConnectionString);
                conexao.Open();

                using var cmd = new MySqlCommand("SELECT * FROM Usuarios_View WHERE Token = @Token", conexao);
                cmd.Parameters.AddWithValue("@Token", token.ToString());

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    usuarios.Add(new Usuario
                    {
                        Id = Guid.Parse(reader["Id"].ToString()),
                        Nome = reader["Nome"].ToString(),
                        Login = reader["Login"].ToString(),
                        Senha = reader["Senha"].ToString(),
                        Token = reader["Token"]?.ToString()
                    });
                }

                return new Mensagem();
            }
            catch (Exception ex)
            {
                return new Mensagem(ex.Message, ex);
            }
        }

        public static Mensagem TryObterTodosUsuarios(out List<Usuario> usuarios)
        {
            usuarios = new List<Usuario>();
            try
            {
                using var conexao = new MySqlConnection(Connection.PetShopConnectionString);
                conexao.Open();

                using var cmd = new MySqlCommand("SELECT * FROM Usuarios_View", conexao);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    usuarios.Add(new Usuario
                    {
                        Id = Guid.Parse(reader["Id"].ToString()),
                        Nome = reader["Nome"].ToString(),
                        Login = reader["Login"].ToString(),
                        Senha = reader["Senha"].ToString(),
                        Token = reader["Token"]?.ToString()
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
