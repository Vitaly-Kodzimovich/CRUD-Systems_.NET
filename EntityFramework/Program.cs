using Microsoft.EntityFrameworkCore;


namespace EntityFramework_CRUD
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
    }


    public class LocalDatabase : DbContext
    {
        public DbSet<User> Users => Set<User>();

        public void CreateDatabase() 
        {
            Database.EnsureCreated();
        }

        public void DeleteDatabase()
        {
            Database.EnsureDeleted();
        }

        public void DeleteAllData()
        {
            DeleteDatabase();
            CreateDatabase();
        }

        public bool CheckDatabaseAvailability()
        {
            bool isAvailable = Database.CanConnect();
            return isAvailable;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }
    }


    public static class CRUD_Panel
    {
        public static void Create(LocalDatabase database, User user)
        {
            database.Users.Add(user);
            database.SaveChanges();
        }
        public static void Create(LocalDatabase database, User[] users)
        {
            database.Users.AddRange(users);
            database.SaveChanges();
        }
        public static void Create(LocalDatabase database, List<User> users)
        {
            database.Users.AddRange(users);
            database.SaveChanges();
        }


        public static void ReadElements(LocalDatabase database)
        {
            var users = database.Users.ToList();
            foreach (User u in users)
            {
                Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
            }
        }


        public static void UpdateDatabase(LocalDatabase database)
        {
            database.SaveChanges();
        }


        public static void Delete(LocalDatabase database, User user)
        {
            database.Users.Remove(user);
            database.SaveChanges();
        }
        public static void Delete(LocalDatabase database, User[] users)
        {
            database.RemoveRange(users);
            database.SaveChanges();
        }
        public static void Delete(LocalDatabase database, List<User> users)
        {
            database.RemoveRange(users);
            database.SaveChanges();
        }
    }



    class Program
    {
        static void Main()
        {
            using (LocalDatabase db = new LocalDatabase())
            {
                db.CreateDatabase();

                User alice =   new User { Name = "Alice", Age = 1 };
                User bob   =   new User { Name = "Bob",   Age = 2};
                User carl  =   new User { Name = "Carl",  Age = 3};
                User doris =   new User { Name = "Doris", Age = 4};

                List<User> boys = new List<User>();
                boys.Add(bob);
                boys.Add(carl);
                
                CRUD_Panel.Create(db,alice);
                CRUD_Panel.Create(db,boys);
                CRUD_Panel.Create(db,doris);
                CRUD_Panel.ReadElements(db);

                Console.WriteLine();
                CRUD_Panel.Delete(db, carl);
                CRUD_Panel.ReadElements(db);

                Console.WriteLine();
                CRUD_Panel.Create(db,carl);
                CRUD_Panel.Delete(db,boys);
                CRUD_Panel.ReadElements(db);

                db.DeleteDatabase();
            }
        }
    }
}

