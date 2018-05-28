using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi2.Models
{
    public class MyRepository
    {
        private List<Person> Persons = new List<Person>()
        {
            new Person() {Id = 1, Name = "Reza", Family = "Abolhasani"},
            new Person() {Id = 2, Name = "Reza1", Family = "Abolhasani1"},
            new Person() {Id = 3, Name = "Reza2", Family = "Abolhasani2"},
        };
        private List<User> Users = new List<User>()
        {
            new User() {Id = 1, UserName = "Reza", Password = "Abolhasani"},
            new User() {Id = 2, UserName = "user1", Password = "pass1"},
            new User() {Id = 3, UserName = "user2", Password = "pass2"},
        };

        private string CasheKeyPersons = "MyDbPersons";
        private string CasheKeyUsers = "MyDbUsers";
        public MyRepository()
        {
            var ctx = HttpContext.Current;
            if (ctx!=null)
            {
                if (ctx.Cache[CasheKeyPersons] == null)
                {
                    ctx.Cache[CasheKeyPersons] = Persons;
                }
                if (ctx.Cache[CasheKeyUsers] == null)
                {
                    ctx.Cache[CasheKeyUsers] = Users;
                }
            }
        }

        public List<Person> GetAllPersons()
        {
            var ctx = HttpContext.Current;
            if (ctx!=null)
            {
                return (List<Person>) ctx.Cache[CasheKeyPersons];
            }
            return new List<Person>();
        }

        public List<User> GetAllUsers()
        {
            var ctx = HttpContext.Current;
            if (ctx!=null)
            {
                return (List<User>) ctx.Cache[CasheKeyUsers];
            }
            return new List<User>();
        }

        public bool SavePerson(Person person)
        {
            var ctx = HttpContext.Current;
            if (ctx != null)
            {
                try
                {
                    var persons = (List<Person>)ctx.Cache[CasheKeyPersons];
                    persons.Add(person);
                    ctx.Cache[CasheKeyPersons] = persons;
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
            return false;
        }

    }
}