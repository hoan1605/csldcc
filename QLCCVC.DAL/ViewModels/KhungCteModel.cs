using QLCCVC.DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCCVC.DAL.ViewModels
{
    public class KhungCteModel : KhungModel
    {
        public int Level { get; set; }
        public string TreePath { get; set; }
    }
}
