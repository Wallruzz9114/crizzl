using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crizzl.Domain.Entities;
using Crizzl.Infrastructure.Contexts;
using Crizzl.Infrastructure.Helpers;
using Newtonsoft.Json;

namespace Crizzl.Infrastructure.Seed
{
    public static class DataSeeder
    {
        public static void Seed(DatabaseContext databaseContext)
        {
            if (!databaseContext.Users.Any())
            {
                var usersData = File.ReadAllText("../Crizzl.Infrastructure/Seed/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(usersData);

                foreach (var user in users)
                {
                    PasswordHasher.GeneratePasswordHash("Qwertyuiop123!", out byte[] passwordHash, out byte[] passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower();

                    databaseContext.Users.Add(user);
                }

                databaseContext.SaveChanges();
            }
        }
    }
}