using CarWorkshop.Models;

namespace CarWorkshop.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDBContext>();

                context.Database.EnsureCreated();

                if (!context.Employees.Any())
                {
                    context.Employees.AddRange(new List<Employee>()
                    {
                        new Employee()
                        {
                            Name = "Steve",
                            Surname = "Jobs",
                            Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f5/Steve_Jobs_Headshot_2010-CROP2.jpg/1200px-Steve_Jobs_Headshot_2010-CROP2.jpg",
                            Login = "Steve13",
                            Password = "12345678",
                            Salary = 300.50,
                            BirthDate = new DateTime(1989, 5, 19),
                            Gender = "Male",
                            WorkingSince = new DateTime(2024, 5, 10),
                            IsAdmin = true
                         },
                        new Employee()
                        {
                            Name = "Elon",
                            Surname = "Musk",
                            Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/99/Elon_Musk_Colorado_2022_%28cropped2%29.jpg/640px-Elon_Musk_Colorado_2022_%28cropped2%29.jpg",
                            Login = "Elon13",
                            Password = "12345678",
                            Salary = 100.50,
                            BirthDate = new DateTime(1970, 5, 16),
                            Gender = "Male",
                            WorkingSince = new DateTime(2024, 5, 11),
                            IsAdmin = false
                        },
                        new Employee()
                        {
                            Name = "Bill",
                            Surname = "Gates",
                            Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Bill_Gates_-_2023_-_P062021-967902_%28cropped%29.jpg/800px-Bill_Gates_-_2023_-_P062021-967902_%28cropped%29.jpg",
                            Login = "Bill13",
                            Password = "12345678",
                            Salary = 180.50,
                            BirthDate = new DateTime(1964, 5, 14),
                            Gender = "Male",
                            WorkingSince = new DateTime(2024, 5, 13),
                            IsAdmin = false
                        },
                        new Employee()
                        {
                            Name = "Jeff",
                            Surname = "Bezos",
                            Image = "https://upload.wikimedia.org/wikipedia/commons/0/03/Jeff_Bezos_visits_LAAFB_SMC_%283908618%29_%28cropped%29.jpeg",
                            Login = "Jeff13",
                            Password = "12345678",
                            Salary = 150.50,
                            BirthDate = new DateTime(1966, 5, 4),
                            Gender = "Male",
                            WorkingSince = new DateTime(2024, 5, 14),
                            IsAdmin = false
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.Tickets.Any())
                {
                    context.Tickets.AddRange(new List<Ticket>()
                    {
                        new Ticket()
                        {
                            Car = new Car()
                            {
                                Brand = "Tesla",
                                Model = "Model 3",
                                RegId = 4556
                            },
                            Description = "The engine does not start",
                            IsDone = false,
                            IsClosed = false,
                            DateTimeSlots = new List<DateTimeSlot>
                            {
                                new DateTimeSlot
                                {
                                    From = new DateTime(2024, 5, 18, 13, 30, 0),
                                    Till = new DateTime(2024, 5, 24, 18, 0, 0)
                                }
                            }
                        },
                        new Ticket()
                        {
                            Car = new Car()
                            {
                                Brand = "BMV",
                                Model = "i8",
                                RegId = 7789
                            },
                            Description = "The engine does not start",
                            IsDone = false,
                            IsClosed = false,
                            DateTimeSlots = new List<DateTimeSlot>
                            {
                                new DateTimeSlot
                                {
                                    From = new DateTime(2024, 5, 21, 11, 0, 0),
                                    Till = new DateTime(2024, 5, 26, 18, 0, 0)
                                }
                            }
                        }
                    });
                    context.SaveChanges();
                }
            }
        }

    }
}
