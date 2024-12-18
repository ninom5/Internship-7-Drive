﻿using Drive.Data.Entities;
using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace Drive.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly DriveDbContext _context;

        public UserService(DriveDbContext context)
        {
            _context = context;
        }

        public bool EmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public Status Create(string name, string surname, string email, string password, byte[] hashedPassword)
        {
            try
            {
                var newUser = new User
                {
                    Name = name,
                    Surname = surname,
                    Email = email,
                    Password = password,
                    HashedPassword = hashedPassword,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                return Status.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Status.Failed;
            }
        }
    }
}