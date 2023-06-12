using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace MyWebService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MyRestService : IRestService
    {
        private static List<Person> persons;
        private static int id = 4;

        public MyRestService() 
        {
            persons = new List<Person>
            {
                new Person { Id=0, Name = "Piotr", Age = 23, Email = "piotr@mail.com" },
                new Person { Id=1, Name = "Zuzanna", Age = 22, Email = "zuza@mail.com" },
                new Person { Id=2, Name = "Anna", Age = 21, Email = "anna@mail.com" },
                new Person { Id=3, Name = "Barbara", Age = 20, Email = "barbara@mail.com" },
            };
        }

        public string addXml(Person item)
        {
            return addPerson(item);
        }

        public string editXml(Person item)
        {
            return editPerson(item);
        }

        public string deleteXml(string Id)
        {
            return deletePerson(Id);
        }

        public List<Person> getAllXml()
        {
            return persons;
        }

        public Person getByIdXml(string Id)
        {
            int intId = int.Parse(Id);
            int idx = persons.FindIndex(b => b.Id == intId);
            if (idx == -1)
                throw new WebFaultException<string>("404: Not Found", HttpStatusCode.NotFound);
            return persons.ElementAt(idx);
        }

        public List<Person> getByNameXml(string Name)
        {
            List<Person> list = new List<Person>();
            foreach (Person person in persons)
            {
                if (person.Name.ToLower().Contains(Name.ToLower()))
                {
                    list.Add(person);
                }
            }
            return list;
        }

        public int getSizeXml()
        {
            return persons.Count();
        }

        public string addJson(Person item)
        {
            return addPerson(item);
        }

        public string editJson(Person item)
        {
            return editPerson(item);
        }

        public string deleteJson(string Id)
        {
            return deletePerson(Id);
        }

        public List<Person> getAllJson()
        {
            return getAllXml();
        }

        public Person getByIdJson(string Id)
        {
            return getByIdXml(Id);
        }

        public List<Person> getByNameJson(string Name)
        {
            return getByNameXml(Name);
        }

        public int getSizeJson()
        {
            return getSizeXml();
        }

        private string getAuthors()
        {
            return "Zuzanna Sikorska, 260464; Piotr Łazik, 260371";
        }

        public string getAuthorsXml()
        {
            return getAuthors();
        }

        public string getAuthorsJson()
        {
            return getAuthors();
        }

        private string addPerson(Person item)
        {
            if (item == null)
                throw new WebFaultException<string>("400:BadRequest", HttpStatusCode.BadRequest);

            bool personExists = persons.FindIndex(p => p.Name == item.Name && p.Email == item.Email && p.Age == item.Age) != -1;
            if (personExists)
                throw new WebFaultException<string>("409: Conflict", HttpStatusCode.Conflict);

            item.Id = id++;

            persons.Add(item);
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Created;
            return "Dodano nową osobę z ID=" + item.Id;
        }
        private string editPerson(Person item)
        {
            int indexInDB = persons.FindIndex(p => p.Id == item.Id);
            if (indexInDB == -1)
                throw new WebFaultException<string>("404: Not Found", HttpStatusCode.NotFound);
            Person personToChange = persons.ElementAt(indexInDB);
            personToChange.Name = item.Name;
            personToChange.Email = item.Email;
            personToChange.Age = item.Age;
            return "Zmieniono dane osoby o Id=" + item.Id;
        }
        private string deletePerson(string Id)
        {
            int intId = int.Parse(Id);
            int idx = persons.FindIndex(b => b.Id == intId);
            if (idx == -1)
                throw new WebFaultException<string>("404: Not Found", HttpStatusCode.NotFound);
            persons.RemoveAt(idx);
            return "Usunięto osobę o ID=" + Id;
        }
    }
}
