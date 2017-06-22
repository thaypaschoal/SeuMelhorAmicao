using prjSeuMelhorAmicao.Models.ConexaoBD;
using prjSeuMelhorAmicao.Models.Entidade;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace prjSeuMelhorAmicao.Models.DAL
{
    public class UsuarioDAO
    {
        public Usuario Login(LoginViewModel model)
        {
            var conex = new ConectionFactory();
            string sp = "spLogin";

            //Criando lista de parâmetros e inserindo um a um
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@email", model.Email),
                new SqlParameter("@senha", model.Senha),
            };

            DataTable table = conex.ExecutaSpDataTable(sp, parametros);


            if (table == null || table.Rows.Count == 0) return new Usuario();

            return new Usuario()
            {
                Id = int.Parse(table.Rows[0]["Id"].ToString()),
                Nome = table.Rows[0]["Nome"].ToString(),
                Email = table.Rows[0]["Email"].ToString(),
                Perfil = new Perfil() { Tipo = table.Rows[0]["Tipo"].ToString() }
            };

        }

     

    }
}