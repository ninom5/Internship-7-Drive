using Drive.Data.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using File = Drive.Data.Entities.Models.File;
using Drive.Data.Enums;

namespace Drive.Data.Seed
{
    public static class DatabaseSeeder
    {
        public static void InitialSeed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasData(new List<User>
                {
                    new User
                    {
                        Id = 1,
                        Name = "Ante",
                        Surname = "Antic",
                        Email = "ante@gmail.com",
                        HashedPassword = Hash("ante123"),
                        CreatedAt = DateTime.Now
                    },
                    new User
                    {
                        Id = 2,
                        Name = "Mate",
                        Surname = "Matic",
                        Email = "mate@gmail.com",
                        HashedPassword = Hash("mate123"),
                        CreatedAt = DateTime.Now
                    },
                    new User
                    {
                        Id = 3,
                        Name = "Jon",
                        Surname = "Jones",
                        Email = "jonjones@gmail.com",
                        HashedPassword = Hash("jonjones123"),
                        CreatedAt = DateTime.Now
                    },
                    new User
                    {
                        Id = 4,
                        Name = "Steph",
                        Surname = "Curry",
                        Email = "curry@gmail.com",
                        HashedPassword = Hash("curry123"),
                        CreatedAt = DateTime.Now
                    }
                });



            modelBuilder.Entity<Folder>()
                .HasData(new List<Folder>
                {
                    new Folder
                    {
                        Id = 1,
                        Name = "Root Folder",
                        OwnerId = 1,
                        ParentFolderId = null,
                        CreatedAt = DateTime.Now
                    },
                    new Folder
                    {
                        Id = 2,
                        Name = "Root Folder",
                        OwnerId = 2,
                        ParentFolderId = null,
                        CreatedAt = DateTime.Now
                    },
                    new Folder
                    {
                        Id = 3,
                        Name = "Root Folder",
                        OwnerId = 3,
                        ParentFolderId = null,
                        CreatedAt = DateTime.Now
                    },
                    new Folder
                    {
                        Id = 4,
                        Name = "Root Folder",
                        OwnerId = 4,
                        ParentFolderId = null,
                        CreatedAt = DateTime.Now
                    },
                    new Folder
                    {
                        Id = 5,
                        Name = "Projects",
                        OwnerId = 1,
                        ParentFolderId = 1,
                        CreatedAt = DateTime.Now
                    },
                    new Folder
                    {
                        Id = 6,
                        Name = "Database",
                        OwnerId = 1,
                        ParentFolderId = 5,
                        CreatedAt = DateTime.Now
                    },
                    new Folder
                    {
                        Id = 7,
                        Name = "College",
                        OwnerId = 1,
                        ParentFolderId = 1,
                        CreatedAt = DateTime.Now
                    },
                    new Folder
                    {
                        Id = 8,
                        Name = "FESB",
                        OwnerId = 2,
                        ParentFolderId = 2,
                        CreatedAt = DateTime.Now
                    },
                    new Folder
                    {
                        Id = 9,
                        Name = "Training",
                        OwnerId = 3,
                        ParentFolderId = 3,
                        CreatedAt = DateTime.Now
                    },
                    new Folder
                    {
                        Id = 10,
                        Name = "Basketball Training",
                        OwnerId = 4,
                        ParentFolderId = 4,
                        CreatedAt = DateTime.Now
                    }
                });


            modelBuilder.Entity<File>()
                .HasData(new List<File>
                {
                    new File
                    {
                        Id = 1,
                        Name = "insert.sql",
                        Content = "insert into Users(id) Values(1)",
                        FolderId = 1,
                        OwnerId = 1,
                        CreatedAt = DateTime.Now
                    },
                    new File
                    {
                        Id = 2,
                        Name = "Drive.cs",
                        Content = "public static class{\nint n = 1;\n}",
                        FolderId = 5,
                        OwnerId = 1,
                        CreatedAt = DateTime.Now
                    },
                    new File
                    {
                        Id = 3,
                        Name = "FitnessCenter",
                        Content = "Create table",
                        FolderId = 6,
                        OwnerId = 1,
                        CreatedAt = DateTime.Now
                    },
                    new File
                    {
                        Id = 4,
                        Name = "Restaurant",
                        Content = "Create table",
                        FolderId = 6,
                        OwnerId = 1,
                        CreatedAt = DateTime.Now
                    },
                    new File
                    {
                        Id = 5,
                        Name = "Algorithms",
                        Content = "Bubble sort",
                        FolderId = 7,
                        OwnerId = 1,
                        CreatedAt = DateTime.Now
                    },
                    new File
                    {
                        Id = 6,
                        Name = "Strukture podataka",
                        Content = "tree node",
                        FolderId = 8,
                        OwnerId = 2,
                        CreatedAt = DateTime.Now
                    },
                    new File
                    {
                        Id = 7,
                        Name = "Training of strength",
                        Content = "Bench 3x15 - 100kg\n Deadlifts 2x10",
                        FolderId = 9,
                        OwnerId = 3,
                        CreatedAt = DateTime.Now
                    },
                    new File
                    {
                        Id = 8,
                        Name = "Cardio",
                        Content = "30min",
                        FolderId = 9,
                        OwnerId = 3,
                        CreatedAt = DateTime.Now
                    }
                });


            modelBuilder.Entity<SharedItem>()
                .HasData(new List<SharedItem>
                {
                    new SharedItem
                    {
                        Id = 1,
                        ItemType = DataType.Folder,
                        ItemId = 5,
                        SharedWithId = 2,
                        SharedById = 1,
                        SharedAt = DateTime.Now
                    },
                    new SharedItem
                    {
                        Id = 2,
                        ItemType = DataType.File,
                        ItemId = 3,
                        SharedWithId = 2,
                        SharedById = 1,
                        SharedAt = DateTime.Now
                    }
                });


            modelBuilder.Entity<Comment>()
                .HasData(new List<Comment>
                {
                    new Comment
                    {
                        Id = 1,
                        FileId = 1,
                        UserId = 1,
                        Content = "lose",
                        CreatedAt = DateTime.Now
                    },
                    new Comment
                    {
                        Id = 2,
                        FileId = 3,
                        UserId = 2,
                        Content = "A?",
                        CreatedAt = DateTime.Now
                    }
                });
        }
        private static byte[] Hash(string password)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                var passwordHash = Encoding.UTF8.GetBytes(password);

                return sha512.ComputeHash(passwordHash);
            }
        }
    }
}
