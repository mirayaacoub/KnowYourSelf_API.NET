using KnowYourSelf_API.Data;
using KnowYourSelf_API.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace KnowYourSelf_API.Services
{
    public class AuthenticateService
    {
        private readonly ApplicationDbContext _db;

        public AuthenticateService(ApplicationDbContext db)
        {
            _db = db;
        }

        public (int Status, string Message, User User) Authenticate(string email, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return (401, "Cannot login with these credentials!", null);
            }

            using (var sha256 = SHA256.Create())
            {
                var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));

                if (user.Password != hashedPassword)
                {
                    return (401, "Cannot login with these credentials!", null);
                }
            }

            return (200, "Successful", user);
        }
    }
}
