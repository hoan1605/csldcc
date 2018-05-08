using QLCCVC.DAL.DomainModels;
using QLCCVC.Infrastructure;
using QLCCVC.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCCVC.DAL
{
    #region Interface

    public interface ILinhVucRepository : IRepository<LinhVucModel> { }

    #endregion
    public class LinhVucRepository : Repository<LinhVucModel>, ILinhVucRepository
    {
        protected override string ConnectionString => "QLCC_ConnectionString";
        protected override string TableName => "[dbo].[DM_linhVuc]";
    }
}
