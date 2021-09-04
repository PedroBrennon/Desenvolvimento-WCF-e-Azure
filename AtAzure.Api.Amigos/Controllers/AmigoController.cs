using AtAzure.Api.Amigos.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AtAzure.Api.Amigos.Controllers
{
    public class AmigoController : ApiController
    {
        private SqlConnection _connection;

        private void Connection()
        {
            string cons = ConfigurationManager.ConnectionStrings["dbat"].ToString();
            _connection = new SqlConnection(cons);
        }

        // GET: api/Amigo
        public IEnumerable<AmigoBindingModel> Get()
        {
            Connection();
            List<AmigoBindingModel> amigos = new List<AmigoBindingModel>();

            using (SqlCommand command = new SqlCommand("GetAllAmigos", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        AmigoBindingModel amigo = new AmigoBindingModel()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Foto = Convert.ToString(reader["Foto"]),
                            Nome = Convert.ToString(reader["Nome"]),
                            Sobrenome = Convert.ToString(reader["Sobrenome"]),
                            Email = Convert.ToString(reader["Email"]),
                            Telefone = Convert.ToString(reader["Telefone"]),
                            Aniversario = Convert.ToDateTime(reader["Aniversario"]),
                            Pais = Convert.ToString(reader["Pais"]),
                            Estado = Convert.ToString(reader["Estado"]),
                            Amigo = Convert.ToString(reader["Amigos"]),
                        };
                        amigos.Add(amigo);
                    }
                }
                _connection.Close();
            }

            return amigos;
        }

        // GET: api/Amigo/5
        public AmigoBindingModel Get(int id)
        {
            Connection();
            AmigoBindingModel amigo = new AmigoBindingModel();

            using (SqlCommand command = new SqlCommand("GetAmigoById", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    amigo = new AmigoBindingModel()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Foto = Convert.ToString(reader["Foto"]),
                        Nome = Convert.ToString(reader["Nome"]),
                        Sobrenome = Convert.ToString(reader["Sobrenome"]),
                        Email = Convert.ToString(reader["Email"]),
                        Telefone = Convert.ToString(reader["Telefone"]),
                        Aniversario = Convert.ToDateTime(reader["Aniversario"]),
                        Pais = Convert.ToString(reader["Pais"]),
                        Estado = Convert.ToString(reader["Estado"]),
                        Amigo = Convert.ToString(reader["Amigos"]),
                    };
                }
                _connection.Close();

                return amigo;
            }
        }

        // POST: api/Amigo
        public void Post(AmigoBindingModel amigo)
        {
            Connection();

            using (SqlCommand command = new SqlCommand("InsertAmigo", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Foto", amigo.Foto);
                command.Parameters.AddWithValue("@Nome", amigo.Nome);
                command.Parameters.AddWithValue("@Sobrenome", amigo.Sobrenome);
                command.Parameters.AddWithValue("@Email", amigo.Email);
                command.Parameters.AddWithValue("@Telefone", amigo.Telefone);
                command.Parameters.AddWithValue("@Aniversario", amigo.Aniversario);
                command.Parameters.AddWithValue("@Pais", amigo.Pais);
                command.Parameters.AddWithValue("@Estado", amigo.Estado);

                _connection.Open();
                int execute = command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        // PUT: api/Amigo/5
        public void Put(int id, AmigoBindingModel amigo)
        {
            Connection();

            using (SqlCommand command = new SqlCommand("UpdateAmigo", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Foto", amigo.Foto);
                command.Parameters.AddWithValue("@Nome", amigo.Nome);
                command.Parameters.AddWithValue("@Sobrenome", amigo.Sobrenome);
                command.Parameters.AddWithValue("@Email", amigo.Email);
                command.Parameters.AddWithValue("@Telefone", amigo.Telefone);
                command.Parameters.AddWithValue("@Aniversario", amigo.Aniversario);
                command.Parameters.AddWithValue("@Pais", amigo.Pais);
                command.Parameters.AddWithValue("@Estado", amigo.Estado);

                _connection.Open();
                int execute = command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        // DELETE: api/Amigo/5
        public void Delete(int id)
        {
            Connection();
            using (SqlCommand command = new SqlCommand("DeleteAmigo", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);

                _connection.Open();
                int execute = command.ExecuteNonQuery();
            }
            _connection.Close();
        }
    }
}
