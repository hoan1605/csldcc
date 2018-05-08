using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCCVC.Infrastructure.Domain
{
    public interface IRepository<T> where T : EntityBase
    {
        long Insert(T item);
        T FindById(long id);
        T FindSingle(string query, dynamic param);
        void Update(T item);
        void Delete(T item);
        IEnumerable<T> FindAll();
        IEnumerable<T> Pagination(string orderBy, string fieldList, string filter, int pageIndex, int pageSize, out int totalRecords);
    }
}
