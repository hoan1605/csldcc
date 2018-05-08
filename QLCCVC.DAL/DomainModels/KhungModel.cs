using QLCCVC.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCCVC.DAL.DomainModels
{
    [Table("khung")]
    public partial class KhungModel : EntityBase
    {
        public virtual string parent_ID { get; set; }
        public virtual string DM_capDonViHanhChinh_ID { get; set; }
        public virtual string DM_loaiToChuc_ID { get; set; }
        public virtual string DM_linhVuc_ID { get; set; }
        public virtual string tenKhung { get; set; }
        public virtual string ghiChu { get; set; }
        public virtual int? STT { get; set; }
        public virtual bool? hieuLuc { get; set; }
    }
}
