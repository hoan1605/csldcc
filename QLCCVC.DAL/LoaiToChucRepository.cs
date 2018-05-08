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

    public interface ILoaiToChucRepository : IRepository<LoaiToChucModel>
    {

    }

    #endregion
    public class LoaiToChucRepository : Repository<LoaiToChucModel>, ILoaiToChucRepository
    {
        protected override string ConnectionString => "QLCC_ConnectionString";
        protected override string TableName => "[dbo].[DM_loaiToChuc]";
    }
}
