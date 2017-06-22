using prjSeuMelhorAmicao.Models.ConexaoBD;
using prjSeuMelhorAmicao.Models.DAL.Contrato;
using prjSeuMelhorAmicao.Models.Entidade;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace prjSeuMelhorAmicao.Models.DAL
{
    public class AnimalDAO : IDAO<Animal>
    {

        public void Salvar(Animal obj)
        {
            if (obj.Id == 0)
                Insert(obj);
            else
                Update(obj);
        }

        public void Insert(Animal obj)
        {
            var conex = new ConectionFactory();
            string sp = "spInsertAnimal";

            //Criando lista de parâmetros e inserindo um a um
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@nome", obj.Nome),
                new SqlParameter("@especie", obj.Especie),
                new SqlParameter("@genero", obj.Genero),
                new SqlParameter("@foto", obj.Foto),
                new SqlParameter("@descricao", obj.Descricao),
                new SqlParameter("@OngID", obj.ONGId),
            };
            obj.Id = (int)conex.ExecutaScalarSP(sp, parametros);
        }

        public void Delete(Animal obj)
        {
            var conex = new ConectionFactory();
            string sp = "spDeleteAnimal";

            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@idAnimal", obj.Id)
            };
            conex.ExecutaNonQuerySP(sp, parametros);
        }

        public void Update(Animal obj)
        {
            var conex = new ConectionFactory();
            string sp = "spUpdateAnimal";

            //Criando lista de parâmetros e inserindo um a um
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@id", obj.Id),
                new SqlParameter("@nomeAnimal", obj.Nome),
                new SqlParameter("@especie", obj.Especie),
                new SqlParameter("@genero", obj.Genero),
                new SqlParameter("@foto", obj.Foto),
                new SqlParameter("@descricaoAnimal", obj.Descricao),
                new SqlParameter("@dataEntrada", obj.DataEntrada)
            };
            conex.ExecutaNonQuerySP(sp, parametros);
        }


        public Animal Buscar(int id)
        {
            var conex = new ConectionFactory();
            string sp = "spBuscarAnimal";

            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@Id", id)
            };
            var dt = conex.ExecutaSpDataTable(sp, parametros);

            return ConvertTable(dt).FirstOrDefault();
        }

        public List<Animal> Listar(string pesquisa)
        {
            var conex = new ConectionFactory();
            string sp = "spListaAnimnal";

            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@pesquisa", pesquisa)
            };
            var dt = conex.ExecutaSpDataTable(sp, parametros);

            return ConvertTable(dt);
        }

        public List<Animal> ListarAnimalOng(int ongId)
        {
            var conex = new ConectionFactory();
            string sp = "spListaAnimalOng";

            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@OngId", ongId)
            };
            var dt = conex.ExecutaSpDataTable(sp, parametros);

            return ConvertTable(dt);
        }

        private List<Animal> ConvertTable(DataTable table)
        {
            List<Animal> listAnimal = new List<Animal>();

            foreach (DataRow item in table.Rows)
            {
                listAnimal.Add(new Animal()
                {
                    Id = Convert.ToInt32(item["Id"]),
                    Nome = item["Nome"].ToString(),
                    Genero = item["Genero"].ToString(),
                    DataEntrada = DateTime.Parse(item["DataEntrada"].ToString()),
                    Especie = item["Especie"].ToString(),
                    Descricao = item["Descricao"].ToString(),
                    Foto = (byte[])item["Foto"]
                });
            }
            return listAnimal;
        }
    }
}