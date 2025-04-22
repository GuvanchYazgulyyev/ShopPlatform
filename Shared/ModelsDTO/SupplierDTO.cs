using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPlatform.Shared.ModelsDTO
{
    public class SupplierDTO
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public String Name { get; set; }
        public String WebUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
