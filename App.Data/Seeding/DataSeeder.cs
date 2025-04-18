using App.Data.Contexts;
using App.Data.Entities;
using App.Shared.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Data.Seeding;

public static class DataSeeder
{
    public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var baseUrl = config.GetValue<string>("App:ApiUrl");

        HashingHelper.CreatePasswordHash("1234", out byte[] passwordHash, out byte[] passwordSalt);


        if (!context.AboutMes.Any())
        {
            context.AboutMes.Add(new AboutMe
            {
                Id = 1,
                Introduciton = "Hello, I'm a software developer.",
                ImageUrl1 = $"{baseUrl}/images/pp-2.png",
                ImageUrl2 = $"{baseUrl}/images/pp-1.png",
                Cv = $"{baseUrl}/images/Emre-Inan-Cv-Eng.pdf",
                Title = "Junior Backend Developer"
            });
            await context.SaveChangesAsync();
        }

        if (!context.BlogPosts.Any())
        {
            context.BlogPosts.AddRange(
                new BlogPost
                {
                    Id = new Guid("24900544-58fc-4be7-9ab1-18f088510da4"),
                    AuthorId = new Guid("7c117612-fb38-48c9-9908-97b433a3f92b"),
                    Title = "Introduction to ASP.NET Core",
                    Content = "ASP.NET Core is a cross-platform, high-performance framework for building modern, cloud-based, internet-connected applications. It allows developers to create web apps, services, and APIs with ease.",
                    ImageUrl = $"{baseUrl}/images/image_1.jpg",
                    CreatedAt = DateTime.UtcNow,
                },
            new BlogPost
            {
                Id = new Guid("7aa98865-285c-4c67-a96a-8fcd30855234"),
                AuthorId = new Guid("7c117612-fb38-48c9-9908-97b433a3f92b"),
                Title = "Understanding Entity Framework Core",
                Content = "Entity Framework Core is a lightweight and extensible version of Entity Framework. It is an open-source object-relational mapper for .NET applications.",
                ImageUrl = $"{baseUrl}/images/image_2.jpg",
                CreatedAt = DateTime.UtcNow,
            },
            new BlogPost
            {
                Id = new Guid("b860336c-64af-497d-9bbd-ed95fd752e9f"),
                AuthorId = new Guid("7c117612-fb38-48c9-9908-97b433a3f92b"),
                Title = "Building RESTful APIs with ASP.NET Core",
                Content = "In this blog post, we will explore how to build RESTful APIs using ASP.NET Core. We will cover routing, controllers, and data handling.",
                ImageUrl = $"{baseUrl}/images/image_3.jpg",
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }

        if (!context.Comments.Any())
        {
            context.Comments.AddRange(
                new Comment
                {
                    Id = 1,
                    Content = "Great post!",
                    IsApproved = true,
                    PostId = new Guid("24900544-58fc-4be7-9ab1-18f088510da4"),
                    UserId = new Guid("a8647b9a-fbf3-4867-ba8f-5fc8239bff12"),
                    CreatedAt = DateTime.UtcNow
                },
            new Comment
            {
                Id = 2,
                Content = "Thanks for sharing!",
                IsApproved = true,
                PostId = new Guid("7aa98865-285c-4c67-a96a-8fcd30855234"),
                UserId = new Guid("44063c52-807a-40bf-a65c-a06d5ac3ee26"),
                CreatedAt = DateTime.UtcNow
            },
            new Comment
            {
                Id = 3,
                Content = "Awesome!",
                IsApproved = true,
                PostId = new Guid("b860336c-64af-497d-9bbd-ed95fd752e9f"),
                UserId = new Guid("a8647b9a-fbf3-4867-ba8f-5fc8239bff12"),
                CreatedAt = DateTime.UtcNow
            },
            new Comment
            {
                Id = 4,
                Content = "Nice work!",
                IsApproved = true,
                PostId = new Guid("24900544-58fc-4be7-9ab1-18f088510da4"),
                UserId = new Guid("44063c52-807a-40bf-a65c-a06d5ac3ee26"),
                CreatedAt = DateTime.UtcNow
            },
            new Comment
            {
                Id = 5,
                Content = "Keep it up!",
                IsApproved = true,
                PostId = new Guid("7aa98865-285c-4c67-a96a-8fcd30855234"),
                UserId = new Guid("a8647b9a-fbf3-4867-ba8f-5fc8239bff12"),
                CreatedAt = DateTime.UtcNow
            },
            new Comment
            {
                Id = 6,
                Content = "Well done!",
                IsApproved = true,
                PostId = new Guid("b860336c-64af-497d-9bbd-ed95fd752e9f"),
                UserId = new Guid("7c117612-fb38-48c9-9908-97b433a3f92b"),
                CreatedAt = DateTime.UtcNow
            },
            new Comment
            {
                Id = 7,
                Content = "I love it!",
                IsApproved = false,
                PostId = new Guid("24900544-58fc-4be7-9ab1-18f088510da4"),
                UserId = new Guid("7c117612-fb38-48c9-9908-97b433a3f92b"),
                CreatedAt = DateTime.UtcNow
            },
            new Comment
            {
                Id = 8,
                Content = "This is great!",
                IsApproved = false,
                PostId = new Guid("7aa98865-285c-4c67-a96a-8fcd30855234"),
                UserId = new Guid("44063c52-807a-40bf-a65c-a06d5ac3ee26"),
                CreatedAt = DateTime.UtcNow
            },
            new Comment
            {
                Id = 9,
                Content = "I like it!",
                IsApproved = false,
                PostId = new Guid("b860336c-64af-497d-9bbd-ed95fd752e9f"),
                UserId = new Guid("7c117612-fb38-48c9-9908-97b433a3f92b"),
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }

        if (!context.ContactMessages.Any())
        {
            context.ContactMessages.AddRange(
            new ContactMessage
            {
                Id = 1,
                Name = "John Doe",
                Email = "user@mail.com",
                Subject = "Hello",
                Message = "Hello, how are you?",
                CreatedAt = DateTime.UtcNow
            },
            new ContactMessage
            {
                Id = 2,
                Name = "Jane Doe",
                Email = "user1@mail.com",
                Subject = "Hello",
                Message = "Hello, how are you?",
                CreatedAt = DateTime.UtcNow
            },
            new ContactMessage
            {
                Id = 3,
                Name = "Emre İnan",
                Email = "emreinannn@gmail.com",
                Subject = "Selam",
                Message = "Lütfen mesajımı oku ve mail gönder!",
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }
    
        if (!context.Educations.Any())
        {
            context.Educations.AddRange(
                new Education
                {
                    Id = 1,
                    School = "Tech University",
                    Degree = "Bachelor of Science",
                    FieldOfStudy = "Computer Science",
                    StartDate = new DateTime(2015, 9, 1),
                    EndDate = new DateTime(2019, 6, 30),
                    CreatedAt = DateTime.UtcNow
                },
            new Education
            {
                Id = 2,
                School = "Online Coding Academy",
                Degree = "Certificate",
                FieldOfStudy = "Web Development",
                StartDate = new DateTime(2020, 1, 15),
                EndDate = new DateTime(2020, 5, 15),
                CreatedAt = DateTime.UtcNow
            },
            new Education
            {
                Id = 3,
                School = "Digital Marketing Institute",
                Degree = "Diploma",
                FieldOfStudy = "Digital Marketing",
                StartDate = new DateTime(2021, 3, 1),
                EndDate = new DateTime(2021, 9, 1),
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }

        if (!context.Experiences.Any())
        {
            context.Experiences.AddRange(
                new Experience
                {
                    Id = 1,
                    Title = "Software Engineer",
                    Company = "Tech Innovations Inc.",
                    StartDate = new DateTime(2020, 1, 15),
                    EndDate = new DateTime(2022, 6, 30),
                    Description = "Developed and maintained web applications using ASP.NET Core.",
                    CreatedAt = DateTime.UtcNow
                },
            new Experience
            {
                Id = 2,
                Title = "Frontend Developer",
                Company = "Creative Solutions Ltd.",
                StartDate = new DateTime(2018, 3, 1),
                EndDate = new DateTime(2020, 1, 14),
                Description = "Designed user interfaces and implemented responsive layouts.",
                CreatedAt = DateTime.UtcNow
            },
            new Experience
            {
                Id = 3,
                Title = "Backend Developer",
                Company = "Global Tech Systems",
                StartDate = new DateTime(2017, 5, 10),
                EndDate = null, // Currently working
                Description = "Focused on server-side application logic and database management.",
                CreatedAt = DateTime.UtcNow
            },
            new Experience
            {
                Id = 4,
                Title = "Intern",
                Company = "Future Coders Academy",
                StartDate = new DateTime(2016, 6, 1),
                EndDate = new DateTime(2016, 9, 1),
                Description = "Assisted in developing small-scale projects and learning programming practices.",
                CreatedAt = DateTime.UtcNow
            },
            new Experience
            {
                Id = 5,
                Title = "Project Manager",
                Company = "Innovatech Group",
                StartDate = new DateTime(2022, 7, 1),
                EndDate = null, // Currently working
                Description = "Managing project timelines and collaborating with development teams.",
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }

        if (!context.PersonalInfos.Any())
        {
            context.PersonalInfos.AddRange(
                new PersonalInfo
                {
                    Id = 1,
                    FirstName = "Emre",
                    LastName = "İnan",
                    PhoneNumber = "+90 553 238 2222",
                    Email = "emreinannn@gmail.com",
                    BirthDate = new DateTime(1993, 8, 9),
                    Address = "İstanbul TR"
                });
            await context.SaveChangesAsync();
        }

        if (!context.Projects.Any())
        {
            context.Projects.AddRange(
                new Project
                {
                    Id = 1,
                    Title = "Web Development",
                    Description = "Creating responsive and dynamic websites.",
                    ImageUrl = $"{baseUrl}//images/project-3.jpg",
                    CreatedAt = DateTime.UtcNow
                },
            new Project
            {
                Id = 2,
                Title = "Mobile App Development",
                Description = "Building mobile applications for iOS and Android.",
                ImageUrl = $"{baseUrl}/images/project-4.jpg",
                CreatedAt = DateTime.UtcNow
            },
            new Project
            {
                Id = 3,
                Title = "Machine Learning",
                Description = "Developing machine learning models for data analysis.",
                ImageUrl = $"{baseUrl}/images/project-5.jpg",
                CreatedAt = DateTime.UtcNow
            },
            new Project
            {
                Id = 4,
                Title = "Cloud Computing",
                Description = "Utilizing cloud services for scalable applications.",
                ImageUrl = $"{baseUrl}/images/project-1.jpg",
                CreatedAt = DateTime.UtcNow
            },
            new Project
            {
                Id = 5,
                Title = "Digital Marketing",
                Description = "Strategies for promoting products online.",
                ImageUrl = $"{baseUrl}/images/project-2.jpg",
                CreatedAt = DateTime.UtcNow
            },
            new Project
            {
                Id = 6,
                Title = "Cybersecurity",
                Description = "Protecting systems and networks from cyber threats.",
                ImageUrl = $"{baseUrl}/images/project-6.jpg",
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }

        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    CreatedAt = DateTime.UtcNow
                },
            new Role
            {
                Id = 2,
                Name = "Commenter",
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }

        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User
                {
                    Id = new Guid("7c117612-fb38-48c9-9908-97b433a3f92b"),
                    UserName = "Admin",
                    Email = "admin@mail.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    RoleId = 1,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    ProfilePhotoUrl = $"{baseUrl}/images/pp-1.png"
                },
            new User
            {
                Id = new Guid("a8647b9a-fbf3-4867-ba8f-5fc8239bff12"),
                UserName = "John",
                Email = "commenter@mail.com",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RoleId = 2,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                ProfilePhotoUrl = $"{baseUrl}/images/person_1.jpg"
            },
            new User
            {
                Id = new Guid("44063c52-807a-40bf-a65c-a06d5ac3ee26"),
                UserName = "Mike",
                Email = "commenter2@mail.com",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RoleId = 2,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                ProfilePhotoUrl = $"{baseUrl}//images/person_2.jpg"
            });
            await context.SaveChangesAsync();
        }
    }
}
