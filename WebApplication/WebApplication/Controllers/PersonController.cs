using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidation.Results;
using System.IO;
using DataAccessLayer.Concrete;

namespace WebApplication.Controllers
{
    public class PersonController : Controller
    {
        PersonManager pm = new PersonManager(new EfPersonDal());
        PersonValidator personValidator = new PersonValidator();
        Context c = new Context();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetPersonList() // aktif liste
        {
            var personValues = pm.GetListActive();
            return View(personValues);
        }

        public ActionResult GetDeletedPersonList() // Çöp kutusu
        {
            var personValues = pm.GetListPassive();
            return View(personValues);
        }
        [HttpGet]
        public ActionResult AddPerson()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddPerson(Person person)
        {
            Context c = new Context();
            if (c.Persons.Where(x=>x.personEMail==person.personEMail).FirstOrDefault()!=null)
            {
                ViewBag.vlE = "Bu mail hesabı ile daha önce kullanıcı eklenmiş!";  //Aksi taktirde 2x kazanıyor
                return View();
            }
            ImageAdd(person);
            ValidationResult results = personValidator.Validate(person);
            if (results.IsValid)
            {
                pm.personAdd(person);
                return RedirectToAction("GetPersonList");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }
        public void ImageAdd(Person person)  // bekliyor
        {
            if (Request.Files.Count > 0)
            {
                string filneName = Path.GetFileName(Request.Files[0].FileName);
                string extension = Path.GetExtension(Request.Files[0].FileName);
                string path = "~/Image/" + filneName + extension;
                Request.Files[0].SaveAs(Server.MapPath(path));
                person.PersonProfileImage = "/Image/" + filneName + extension;
            }
        }

        [HttpGet]
        public ActionResult EditPerson(int id)
        {
            var personValue = pm.personGetById(id);
            return View(personValue);
        }
        [HttpPost]
        public ActionResult EditPerson(Person person)
        {
            ImageAdd(person);
            ValidationResult results = personValidator.Validate(person);
            if (results.IsValid)
            {
                pm.personUpdate(person);
                return RedirectToAction("GetPersonList");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }


        public ActionResult DeletePerson(int id)
        {
            var personValues = pm.personGetById(id);
            pm.personDelete(personValues);
            return RedirectToAction("GetPersonList");
        }


        public ActionResult ClearPerson(int id)// herşeyi siler
        {
            var DeletedersonValues = pm.deletedPersonGetById(id);
            pm.PersonClear(DeletedersonValues);
            return RedirectToAction("GetPersonList");
        }

        public ActionResult ReversePerson(int id)// herşeyi siler
        {
            var DeletedersonValues = pm.deletedPersonGetById(id);
            pm.PersonReverse(DeletedersonValues);
            return RedirectToAction("GetPersonList");
        }


















        [HttpGet]
        public ActionResult ındexAddMember()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ındexAddMember(Person person)
        {
            ValidationResult results = personValidator.Validate(person);
            if (results.IsValid)
            {
                pm.personAdd(person);
                return RedirectToAction("MemberLogin", "Login");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }
    }
}