using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        // כאן נכתוב מתודה שתכניס את הנתונים שבקובץ גייסון לתוך הדאטאבייס
        public static void SeedUsers(DataContext context)
        {
            // כאן בודקים שאין בטבלת משתמשים בתוך הדאטא בייס שום נתונים כדי שלא יכנס הרבה פעמיים לדאטאבייס כל פעם שנריץ
            if(!context.Users.Any())
            {
                var userData=System.IO.File.ReadAllText("Data/UserSeedData.json");
                // convert the text to User Object
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in users)
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("password", out passwordHash, out passwordSalt);

                    user.PasswordHash=passwordHash;
                    user.PasswordSalt=passwordSalt;
                    user.Username=user.Username.ToLower();
                    context.Users.Add(user);
                }
                context.SaveChanges();
            }
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac=new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt=hmac.Key;
                passwordHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
    }
}