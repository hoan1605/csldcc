using QLCCVC.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCCVC.DAL.DomainModels
{
    [Table("DM_capDonViHanhChinh")]
    public partial class CapDonViHanhChinhModel : EntityBase
    {
        public virtual string DM_capDonViHanhChinh_ID { get; set; }
        public virtual string noiDung { get; set; }
        public virtual bool? hieuLuc { get; set; }
    }
}
