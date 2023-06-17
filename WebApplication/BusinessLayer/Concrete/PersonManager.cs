using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;

namespace BusinessLayer.Concrete
{
    public class PersonManager : IPersonService
    {
        IPersonDal persondal;
        // silme işlemindeki DeletedPerson tablosu için extra bir Manager sınıfına ihtiyacımız yok.
        IDeletedPersonDal deletedPersonDal= new EfDeletedPersonDal(); // bu kısım ile person veritabındaki veri false olurken deltepedPerson tablosuna ilişkili veri eklenecek(geri döünüşüm kutumuz).
        Context c = new Context();
        public PersonManager(IPersonDal persondal)
        {
            this.persondal = persondal;
        }

        public Person personGetById(int id)
        {
            return persondal.Get(x => x.personId == id);
        }
        public DeletedPerson deletedPersonGetById(int deletedId)
        {
            return deletedPersonDal.Get(x => x.deletedPersonId == deletedId);
        }  
        public Person GetBySha(string sha) // üye sorgulaması için
        {
            return persondal.Get(x => x.PersonShaNo == sha);
        }

        public List<Person> GetListActive()
        {
            return persondal.List(x=>x.PersonState==true);  // aktif olanları getiriyor.
        }
        // Çöp kutusu
        public List<DeletedPerson> GetListPassive() // pasif olanları getiriyor. Burada istersek Person tablosundan çekerek de yapabilirdik.
        {
            return deletedPersonDal.List();
        }

        public void personAdd(Person person)     //aynı maille kayıtlı kullanıcı var mı kotnrolü yapılacak
        {
            if(c.Persons.Where(x => x.personEMail == person.personEMail).FirstOrDefault() == null) { 
            //c.Persons.Where(x=>x.eMail==person.eMail) // veri erişim katmanında yapılacak
            person.personRegisterDate = DateTime.Parse(DateTime.Now.ToShortDateString()); //kullanıcı kayıt tarihi
            person.PersonState = true;
            createSha(person);
            persondal.Insert(person);
            postMail(person);
            }
            else
            {

            }
        }

        public void personDelete(Person person) //Silme işlemini direkt yapmak yerine state bool değişkeni tanımlayıp değerini False yapıyorum.
        {
            person.PersonState = false;
            persondal.Update(person);           //Bu sayede kullanıcıyı sistemden çıkarıyoruz ancak kayılarımızda yine de oluyor. silinenler veritabanı tablosunda silinme işlemi bilgileri
            personDeletedBoxAdd(person);

        }                                       //görüp isteresek tamamen silebiliyoruz ya da geri döndürebiliyoruz.


        //çöp kutusundan da silmek istersek yapmamız gereken, ana veritabanındaki false yaptığımız veriyi de, çöp kutusundaki veriyi de silmemiz gerekiyor.
        //silme işleminde bu yöntemi kullanmak yerine ana tablodan silip farklı tabloya yeni veri girişi şeklindede eklenerek tablonun şişmesi engellenebilir.
        public void PersonClear(DeletedPerson deletedPerson)
        {
            deletedPersonDal.Delete(deletedPerson);
            persondal.Delete(persondal.Get(x => x.personId == deletedPerson.personId));    //veri tamamen silindi!!!!!!           
        }

        public void PersonReverse(DeletedPerson deletedPerson)
        {
            Person p = persondal.Get(x => x.personId == deletedPerson.personId);
            p.PersonState = true;
            persondal.Update(p);                                               // durum aktif olarak güncellendi
            deletedPersonDal.Delete(deletedPerson);                            // silinenler tablosundan silindi 
        }

        public void personUpdate(Person person)
        {
            persondal.Update(person);
        }

        public void createSha(Person person)
        {
            SHA256 mysha256 = SHA256.Create();                        //Maile gönderilmek üzere sha oluşturuluyor
            string source = person.personPassword;                    //bu sha ile kullanıcı sorgulama yapabilecek
            using (SHA256 sha1Hash = SHA256.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                person.PersonShaNo = hash;
            }

            person.PersonState = true;  // Kullanıcının aktif kullanıcılar arasında yer alabilmesi için durumunu aktif yaptım.
        }
        public void postMail(Person person)
        {
            MailMessage message = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential("ersankuneri1981@gmail.com", "ijeometkeyfeftue");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            message.To.Add(person.personEMail);
            message.From = new MailAddress("yildirimenes401@gmail.com");                                        //buradaki mail değiştirilecek(test maili)
            message.Subject = "Sorgulama Numaranız";
            message.Body = "Sn. kullanıcımız kaydınız gerçekleştirilmiştir. kaydınızı sorgulama işlemini \n" + person.PersonShaNo + " numrası ile yapabilirsiniz.\n " +
                "Şifreniz sorgulama numaranızı yazmanızın ardından gözükecektir.";
            client.Send(message);
        }

        public void personDeletedBoxAdd(Person person)
        {
            DeletedPerson deletedPerson = new DeletedPerson();
            deletedPerson.personId = person.personId;
            deletedPerson.deletedPersonDeletedDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            deletedPerson.AdminName = "Admin"; // Admin kaydı ekleyerek burada silme işlemini hangi adminin yaptığını görebiliriz.
            deletedPersonDal.Insert(deletedPerson);
        }
    }
}
