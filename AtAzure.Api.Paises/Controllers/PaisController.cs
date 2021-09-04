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
    public class PaisController : ApiController
    {
        private SqlConnection _connection;

        private void Connection()
        {
            string cons = ConfigurationManager.ConnectionStrings["dbat"].ToString();
            _connection = new SqlConnection(cons);
        }

        // GET: api/Pais
        public IEnumerable<PaisBindingModel> Get()
        {
            Connection();
            List<PaisBindingModel> paises = new List<PaisBindingModel>();

            using (SqlCommand command = new SqlCommand("GetAllPaises", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    PaisBindingModel pais = new PaisBindingModel()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Foto = Convert.ToString(reader["Foto"]),
                        Nome = Convert.ToString(reader["Nome"])
                    };
                    paises.Add(pais);
                }
                _connection.Close();
            }

            return paises;
        }

        // GET: api/Pais/5
        public PaisBindingModel Get(int id)
        {
            Connection();
            PaisBindingModel pais = new PaisBindingModel();

            using (SqlCommand command = new SqlCommand("GetPaisById", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    pais = new PaisBindingModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Foto = Convert.ToString(reader["Foto"]),
                        Nome = Convert.ToString(reader["Nome"])
                    };
                }
                _connection.Close();

                return pais;
            }
        }

        // POST: api/Pais
        public void Post(PaisBindingModel pais)
        {
            Connection();

            using (SqlCommand command = new SqlCommand("InsertPais", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Foto", pais.Foto);
                command.Parameters.AddWithValue("@Nome", pais.Nome);

                _connection.Open();
                int execute = command.ExecuteNonQuery();


            }
            _connection.Close();
        }

        // PUT: api/Pais/5
        public void Put(int id, PaisBindingModel pais)
        {
            Connection();

            using (SqlCommand command = new SqlCommand("UpdatePais", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Foto", pais.Foto);
                command.Parameters.AddWithValue("@Nome", pais.Nome);
                
                _connection.Open();
                int execute = command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        // DELETE: api/Pais/5
        public void Delete(int id)
        {
            Connection();
            using (SqlCommand command = new SqlCommand("DeletePais", _connection))
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
