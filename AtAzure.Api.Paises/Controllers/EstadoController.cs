using AtAzure.Api.Paises.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AtAzure.Api.Paises.Controllers
{
    public class EstadoController : ApiController
    {
        private SqlConnection _connection;

        private void Connection()
        {
            string cons = ConfigurationManager.ConnectionStrings["dbat"].ToString();
            _connection = new SqlConnection(cons);
        }

        // GET: api/Estado
        public IEnumerable<EstadoBindingModel> Get()
        {
            Connection();
            List<EstadoBindingModel> estados = new List<EstadoBindingModel>();

            using (SqlCommand command = new SqlCommand("GetAllEstados", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    EstadoBindingModel estado = new EstadoBindingModel()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Foto = Convert.ToString(reader["Foto"]),
                        Nome = Convert.ToString(reader["Nome"]),
                        paisId = Convert.ToInt32(reader["Pais"]),
                    };
                    estados.Add(estado);
                }
                _connection.Close();
            }

            return estados;
        }

        // GET: api/Estado/5
        public EstadoBindingModel Get(int id)
        {
            Connection();
            EstadoBindingModel estado = new EstadoBindingModel();

            using (SqlCommand command = new SqlCommand("GetEstadoById", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    estado = new EstadoBindingModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Foto = Convert.ToString(reader["Foto"]),
                        Nome = Convert.ToString(reader["Nome"]),
                        paisId = Convert.ToInt32(reader["Pais"])
                    };
                }
                _connection.Close();

                return estado;
            }
        }

        // POST: api/Estado
        public void Post(EstadoBindingModel estado)
        {
            Connection();

            using (SqlCommand command = new SqlCommand("InsertEstado", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Foto", estado.Foto);
                command.Parameters.AddWithValue("@Nome", estado.Nome);
                command.Parameters.AddWithValue("@Pais", estado.paisId);

                _connection.Open();
                int execute = command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        // PUT: api/Estado/5
        public void Put(int id, EstadoBindingModel estado)
        {
            Connection();

            using (SqlCommand command = new SqlCommand("UpdateEstado", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Foto", estado.Foto);
                command.Parameters.AddWithValue("@Nome", estado.Nome);
                command.Parameters.AddWithValue("@Pais", estado.paisId);

                _connection.Open();
                int execute = command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        // DELETE: api/Estado/5
        public void Delete(int id)
        {
            Connection();
            using (SqlCommand command = new SqlCommand("DeleteEstado", _connection))
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
