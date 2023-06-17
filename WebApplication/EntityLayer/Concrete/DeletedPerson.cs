using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class DeletedPerson
    {
        [Key]
        public int deletedPersonId { get; set; }
        public int personId { get; set; }
        public virtual Person Person { get; set; }
        public DateTime deletedPersonDeletedDate { get; set; }
        public string AdminName { get; set; }

    }
}
