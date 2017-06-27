using prjSeuMelhorAmicao.Models.ConexaoBD;
using prjSeuMelhorAmicao.Models.DAL.Contrato;
using prjSeuMelhorAmicao.Models.Entidade;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace prjSeuMelhorAmicao.Models.DAL
{
    public class ClienteDAO : IDAO<Cliente>
    {

        public Cliente Buscar(int id)
        {
            var conex = new ConectionFactory();
            string sp = "spBuscarCliente";

            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@Id", id)
            };
            var dt = conex.ExecutaSpDataTable(sp, parametros);
            var ongs = new List<Ong>();

            return ConvertTable(dt).FirstOrDefault();

        }

        public void Delete(Cliente obj)
        {
            var conex = new ConectionFactory();
            string sp = "spDeleteCliente";

            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@id", obj.Id)
            };
            conex.ExecutaNonQuerySP(sp, parametros);
        }

        public void Insert(Cliente obj)
        {
            var conex = new ConectionFactory();
            string sp = "spInsertCliente";

            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@nome", obj.Nome),
                new SqlParameter("@email", obj.Email),
                new SqlParameter("@senha", obj.Senha),
                new SqlParameter("@genero", obj.Genero),
                new SqlParameter("@dataNascimento", obj.DataNascimento)
            };

            obj.Id = (int)conex.ExecutaScalarSP(sp, parametros);
        }

        public List<Cliente> Listar(string pesquisa)
        {
            throw new NotImplementedException();
        }

        public void Salvar(Cliente obj)
        {
            if (obj.ClienteId == 0)
                Insert(obj);
            else
                Update(obj);
        }

        public void Update(Cliente obj)
        {
            var conex = new ConectionFactory();
            string sp = "spUpdateCliente";

            //Criando lista de parâmetros e inserindo um a um
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@id", obj.Id),
                new SqlParameter("@nome", obj.Nome),
                new SqlParameter("@email", obj.Email),
                new SqlParameter("@senha", obj.Senha),
                new SqlParameter("@genero", obj.Genero),
                new SqlParameter("@dataNascimento", obj.DataNascimento)
            };
            conex.ExecutaNonQuerySP(sp, parametros);
        }


        public void FavoritarAnimal(Cliente cliente)
        {
            var conex = new ConectionFactory();
            string sp = "spFavoritar";

            foreach (Animal animal in cliente.Animal)
            {
                var parametros = new List<SqlParameter>
                {
                    new SqlParameter("@idUsuario", cliente.Id),
                    new SqlParameter("@IdAnimal", animal.Id),
                };

                conex.ExecutaNonQuerySP(sp, parametros);
            }
        }

        public void DesfavoritarAnimal(Cliente cliente)
        {
            var conex = new ConectionFactory();
            string sp = "spDesFavoritar";

            foreach (Animal animal in cliente.Animal)
            {
                var parametros = new List<SqlParameter>
                {
                    new SqlParameter("@idUsuario", cliente.Id),
                    new SqlParameter("@IdAnimal", animal.Id),
                };

                conex.ExecutaNonQuerySP(sp, parametros);
            }
        }



        
        private List<Cliente> ConvertTable(DataTable table)
        {

            if (table.Rows.Count < 1)
                throw new Exception("Cliente não encontrado");


            List<Cliente> listOng = new List<Cliente>();

            foreach (DataRow row in table.Rows)
            {
                listOng.Add(new Cliente()
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nome = row["Nome"].ToString(),
                    Email = row["Email"].ToString(),
                    Senha = row["Senha"].ToString(),
                    Genero = row["Genero"].ToString(),
                    DataNascimento= Convert.ToDateTime(row["DataNascimneto"])


                });
            }
            return listOng;
        }


    }
}