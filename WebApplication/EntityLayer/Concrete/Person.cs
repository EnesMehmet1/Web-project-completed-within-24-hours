using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Person
    {
        [Key]
        public int personId { get; set; }
        public string personName { get; set; }
        public string personSurname { get; set; }
        public string personEMail { get; set; }
        public string personPassword { get; set; }
        public string PersonProfileImage { get; set; }
        public string PersonShaNo { get; set; }
        public bool PersonState { get; set; }
        public DateTime personRegisterDate { get; set; }
        public ICollection<DeletedPerson> DeletedPersons { get; set; }
    }
}
