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

    public interface ICapDonViHanhChinhRepository : IRepository<CapDonViHanhChinhModel>
    {

    }

    #endregion
    public class CapDonViHanhChinhRepository : Repository<CapDonViHanhChinhModel>, ICapDonViHanhChinhRepository
    {
        protected override string ConnectionString => "QLCC_ConnectionString";
        protected override string TableName => "[dbo].[DM_capDonViHanhChinh]";
    }
}
