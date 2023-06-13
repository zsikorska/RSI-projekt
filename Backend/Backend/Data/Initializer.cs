using Backend.Models;
using BlogManager.Data;
using System;
using System.Diagnostics;
using System.Linq;

namespace Backend.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Persons.Any())
            {
                return;   
            }

            var persons = new Person[]
            {
                new Person { Name = "Piotr", Height = 180, Email = "piotr@mail.com", Birthdate=DateOnly.Parse("2000-01-01") },
                new Person { Name = "Zuzanna", Height = 170, Email = "zuza@mail.com", Birthdate=DateOnly.Parse("2000-01-01") },
                new Person { Name = "Anna", Height = 165, Email = "anna@mail.com", Birthdate=DateOnly.Parse("2000-01-01")},
                new Person { Name = "Barbara", Height = 190, Email = "barbara@mail.com", Birthdate=DateOnly.Parse("2000-01-01") },
            };
            foreach (Person s in persons)
            {
                context.Persons.Add(s);
            }
            context.SaveChanges();
        }
    }
}