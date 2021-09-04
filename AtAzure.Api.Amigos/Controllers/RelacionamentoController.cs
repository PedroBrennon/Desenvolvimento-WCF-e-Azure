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
    public class RelacionamentoController : ApiController
    {
        private SqlConnection _connection;

        private void Connection()
        {
            string cons = ConfigurationManager.ConnectionStrings["dbat"].ToString();
            _connection = new SqlConnection(cons);
        }

        // GET: api/Relacionamento/5
        public List<RelacionamentoBindingModel> Get(int id)
        {
            Connection();
            List<RelacionamentoBindingModel> amigo = new List<RelacionamentoBindingModel>();

            using (SqlCommand command = new SqlCommand("GetRelacionamentoById", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdAmigo", id);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var rel = new RelacionamentoBindingModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        IdAmigo1 = Convert.ToInt32(reader["IdAmigo1"]),
                        IdAmigo2 = Convert.ToInt32(reader["IdAmigo2"]),
                        Foto = Convert.ToString(reader["Foto"]),
                        Nome = Convert.ToString(reader["Nome"]),
                        Sobrenome = Convert.ToString(reader["Sobrenome"])
                    };
                    amigo.Add(rel);
                }
                _connection.Close();

                return amigo;
            }
        }

        // POST: api/Relacionamento
        public void Post(RelacionamentoBindingModel amigo)
        {
            Connection();

            using (SqlCommand command = new SqlCommand("InsertRelacionamentoAmigos", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdAmigo1", amigo.IdAmigo1);
                command.Parameters.AddWithValue("@IdAmigo2", amigo.IdAmigo2);

                _connection.Open();
                int execute = command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        // DELETE: api/Relacionamento/5
        public void Delete(int Id)
        {
            Connection();
            using (SqlCommand command = new SqlCommand("DeleteRelacionamento", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", Id);

                _connection.Open();
                int execute = command.ExecuteNonQuery();
            }
            _connection.Close();
        }
    }
}
