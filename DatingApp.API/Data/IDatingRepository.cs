using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
        // לאלץ שהפרמטר T יהיה רק קלאסים
        //להוסיף או משתמשים או תמונות באותה פונקציה
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         // כדי לראות אם יש יותר משינוי אחד יחזיר אמת ושקר אם לא היה שינויים או שהשינווים לא נשמרו
         Task<bool> SaveAll();
         Task<IEnumerable<User>> GetUsers();
         Task<User> GetUser(int id);
    }
}