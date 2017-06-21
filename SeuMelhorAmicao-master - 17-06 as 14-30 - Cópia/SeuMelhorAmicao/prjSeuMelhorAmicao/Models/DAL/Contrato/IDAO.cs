using System.Collections.Generic;


namespace prjSeuMelhorAmicao.Models.DAL.Contrato
{
    public interface IDAO<T> where T : class
    {
        void Salvar(T obj);
        void Insert(T obj);
        void Update(T obj);
        void Delete(T obj);
        T Buscar(int id);
        List<T> Listar(string pesquisa);
    }
}
