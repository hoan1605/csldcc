using Dapper;
using QLCCVC.DAL.DomainModels;
using QLCCVC.DAL.Models;
using QLCCVC.DAL.ViewModels;
using QLCCVC.Infrastructure;
using QLCCVC.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCCVC.DAL
{
    #region Interface
    public interface IKhungRepository: IRepository<KhungModel>
    {
        IEnumerable<KhungCteModel> FindAllCte(long parentId, string searchString, string filterDonVi, string filterToChuc, string filterLinhVuc);
    }
    #endregion
    public class KhungRepository : Repository<KhungModel>, IKhungRepository
    {
        protected override string ConnectionString => "QLCC_ConnectionString";

        protected override string TableName => "[dbo].[khung]";

        public IEnumerable<KhungCteModel> FindAllCte(long parentId, string searchString, string filterDonVi, string filterToChuc, string filterLinhVuc)
        {
            IEnumerable<KhungCteModel> items;

            using (var cn = Connection)
            {
                cn.Open();
                items = cn.Query<KhungCteModel>("Khung_GetAll_Cte",
                        new
                        {
                            ParentId = parentId,
                            SearchString = searchString,
                            FilterDonVi = filterDonVi,
                            FilterToChuc = filterToChuc,
                            FilterLinhVuc = filterLinhVuc
                        },
                        commandType: CommandType.StoredProcedure);
            }
            return items;
        }
    }
}
