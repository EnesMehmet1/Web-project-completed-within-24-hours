using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class PersonValidator: AbstractValidator<Person>
    {
        public PersonValidator() {
            RuleFor(x => x.personName).NotEmpty().WithMessage("Üye adını boş geçemezsin!");
            RuleFor(x => x.personName).MinimumLength(3).WithMessage("Üye adı en az üç karakter olmalı!");
            RuleFor(x => x.personName).MaximumLength(30).WithMessage("Üye adı 30 karakterden az olmalı!");
            RuleFor(x => x.personSurname).NotEmpty().WithMessage("Üye Soyadını boş geçemezsin!");
            RuleFor(x => x.personSurname).MinimumLength(2).WithMessage("Üye Soyadı en az iki karakter olmalı!");
            RuleFor(x => x.personSurname).MaximumLength(30).WithMessage("Üye Soyadı 30 karakterden az olmalı!");
            RuleFor(x => x.personEMail).NotEmpty().WithMessage("Üye E-posta adresini boş geçemezsin!");
            RuleFor(x => x.personEMail).MinimumLength(5).WithMessage("Üye E-posta adresi en az beş karakter olmalı!");
            RuleFor(x => x.personEMail).MaximumLength(40).WithMessage("Üye E-posta adresi 40 karakterden az olmalı!");
            RuleFor(x => x.personEMail).EmailAddress().WithMessage("Lütfen geçerli bir E-posta giriniz").When(x=> !string.IsNullOrEmpty(x.personEMail)); // eposta adresi kontrolü
            RuleFor(x => x.personPassword).NotEmpty().WithMessage("Üye parolasını boş geçemezsin!");
            RuleFor(x => x.personPassword).MinimumLength(4).WithMessage("Üye parolası en az dört karakter olmalı!");
            RuleFor(x => x.personPassword).MaximumLength(18).WithMessage("Üye parolası 18 karakterden az olmalı!");
            RuleFor(x => x.PersonShaNo).Length(64).WithMessage("Lütfen 64 hanli numaranızı giriniz");
        }
    }
}
