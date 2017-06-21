using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;


namespace prjSeuMelhorAmicao.Models.ConexaoBD
{
    public class ConectionFactory
    {
        private SqlConnection conn;
        private SqlTransaction tran;

        public ConectionFactory()
        {
            var conexao = WebConfigurationManager.ConnectionStrings["conexaoTCC"].ConnectionString;
            AbreConexao(conexao);
        }
        public ConectionFactory(string conex)
        {
            AbreConexao(conex);
        }

        private void AbreConexao(string conex)
        {
            conn = new SqlConnection
            {
                ConnectionString = conex
            };

            conn.Open();
        }
        private void FechaConexao()
        {
            if (tran != null)
                throw new Exception(@"A conexão está em transação. Finaize-a antes");
            conn.Close();
            conn.Dispose();
            conn = null;
        }

        public void BeginTran()
        {
            tran = conn.BeginTransaction();
        }

        public void Commit()
        {
            tran.Commit();
            tran = null;
        }

        public void RollBack()
        {
            tran.Rollback();
            tran = null;
        }

        public void ExecutaNonQuerySP(string storedProcedure)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandText = storedProcedure,
                CommandType = System.Data.CommandType.StoredProcedure,
                Connection = conn
            };

            if (tran != null)
                cmd.Transaction = tran;

            cmd.ExecuteNonQuery();
        }

        public object ExecutaNonQuerySP(string storedProcedure, List<SqlParameter> parametros)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandText = storedProcedure,
                CommandType = CommandType.StoredProcedure,
                Connection = conn
            };

            foreach (var parametro in parametros)
                cmd.Parameters.Add(parametro);

            if (tran != null)
                cmd.Transaction = tran;

            return cmd.ExecuteNonQuery();
        }

        public object ExecutaScalarSP(string storedProcedure)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandText = storedProcedure,
                CommandType = CommandType.StoredProcedure,
                Connection = conn
            };

            if (tran != null)
                cmd.Transaction = tran;

            return cmd.ExecuteScalar();
        }

        public object ExecutaScalarSP(string storedProcedure, List<SqlParameter> parametros)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandText = storedProcedure,
                CommandType = CommandType.StoredProcedure,
                Connection = conn
            };

            foreach (var parametro in parametros)
                cmd.Parameters.Add(parametro);

            if (tran != null)
                cmd.Transaction = tran;

            return cmd.ExecuteScalar();
        }

        public DataTable ExecutaSpDataTable(string storedProcedure)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandText = storedProcedure,
                CommandType = CommandType.StoredProcedure,
                Connection = conn
            };

            if (tran != null)
                cmd.Transaction = tran;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds.Tables[0];
        }

        public DataTable ExecutaSpDataTable(string storedProcedure, List<SqlParameter> parametros)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandText = storedProcedure,
                CommandType = CommandType.StoredProcedure,
                Connection = conn
            };

            foreach (var parametro in parametros)
                cmd.Parameters.Add(parametro);

            if (tran != null)
                cmd.Transaction = tran;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            return ds;
        }
    }
}