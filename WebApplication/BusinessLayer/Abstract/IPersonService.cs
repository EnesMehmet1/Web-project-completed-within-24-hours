using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IPersonService
    {
        List<Person> GetListActive();
        List<DeletedPerson> GetListPassive();
        void personAdd(Person person);
        Person personGetById(int id);
        DeletedPerson deletedPersonGetById(int DeletedId);
        Person GetBySha(string sha);
        void personDelete(Person person);
        void personUpdate(Person person);
        void createSha(Person person);
        void postMail(Person person);
        void personDeletedBoxAdd(Person person);
        void PersonClear(DeletedPerson deletedPerson);
        void PersonReverse(DeletedPerson deletedPerson);
    }
}
