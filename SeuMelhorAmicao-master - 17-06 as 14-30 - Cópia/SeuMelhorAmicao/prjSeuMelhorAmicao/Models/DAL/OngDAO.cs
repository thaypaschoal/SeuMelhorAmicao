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
    public class OngDAO : IDAO<Ong>
    {
        public Ong Buscar(int id)
        {
            var conex = new ConectionFactory();
            string sp = "spBuscarOng";

            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@Id", id)
            };
            var dt = conex.ExecutaSpDataTable(sp, parametros);
            var ongs = new List<Ong>();


            return ConvertTable(dt).FirstOrDefault();
        }

        public void Delete(Ong obj)
        {
            var conex = new ConectionFactory();
            string sp = "spDeleteOng";

            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@Id", obj.Id)
            };
            conex.ExecutaNonQuerySP(sp, parametros);
        } 

        public void Insert(Ong obj)
        {
            var conex = new ConectionFactory();
            string sp = "spInsertOng";

            //Criando lista de parâmetros e inserindo um a um
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@nome", obj.Nome),
                new SqlParameter("@telefone", obj.Telefone),
                new SqlParameter("@endereco", obj.Endereco),
                new SqlParameter("@email", obj.Email),
                new SqlParameter("@senha", obj.Senha),
                new SqlParameter("@biografia", obj.Biografia),
                new SqlParameter("@foto", obj.Foto)
            };
            obj.Id = (int)conex.ExecutaScalarSP(sp, parametros);
        }

        public List<Ong> Listar(string pesquisa)
        {
            var conex = new ConectionFactory();
            string sp = "spListaOng";

            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@pesquisa", pesquisa)
            };
            var dt = conex.ExecutaSpDataTable(sp, parametros);

            return ConvertTable(dt);

        }

        public void Salvar(Ong obj)
        {
            if (obj.Id == 0)
                Insert(obj);
            else
                Update(obj);
        }

        public void Update(Ong obj)
        {
            var conex = new ConectionFactory();
            string sp = "spUpdateOng";

            //Criando lista de parâmetros e inserindo um a um
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@id", obj.Id),
                new SqlParameter("@nome", obj.Nome),
                new SqlParameter("@telefone", obj.Telefone),
                new SqlParameter("@endereco", obj.Endereco),
                new SqlParameter("@email", obj.Email),
                new SqlParameter("@senha", obj.Senha),
                new SqlParameter("@biografia", obj.Biografia),
                new SqlParameter("@foto", obj.Foto)
            };
            conex.ExecutaNonQuerySP(sp, parametros);
        }

        private List<Ong> ConvertTable(DataTable table)
        {

            if (table.Rows.Count < 1)
                throw new Exception("ONG não encontrada");


            List<Ong> listOng = new List<Ong>();

            foreach (DataRow row in table.Rows)
            {
                listOng.Add(new Ong()
                {

                    Id = Convert.ToInt32(row["Id"]),
                    Nome = row["Nome"].ToString(),
                    Email = row["Email"].ToString(),
                    Biografia = row["Biografia"].ToString(),
                    Telefone = row["Telefone"].ToString(),
                    Endereco = row["Endereco"].ToString(),
                    Foto = (byte[])row["Foto"]
                });
            }
            return listOng;
        }

    }
}