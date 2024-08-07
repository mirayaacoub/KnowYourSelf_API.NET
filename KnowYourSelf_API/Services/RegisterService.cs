using KnowYourSelf_API.Data;
using KnowYourSelf_API.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Security.Claims;

namespace KnowYourSelf_API.Services
{
    public class RegisterService
    {
        private readonly ApplicationDbContext _db;
        private readonly string _jwtSecretKey;

        public RegisterService(ApplicationDbContext db, string jwtSecretKey)
        {
            _db = db;
            _jwtSecretKey = jwtSecretKey;
        }

        public (int Status, string Message, string Token) RegisterUser(CreateUserDTO createUserDTO)
        {
            // Check for email uniqueness
            if (_db.Users.Any(u => u.Email.ToLower() == createUserDTO.Email.ToLower()))
            {
                return (409, "User already exists", null);
            }

            if (createUserDTO == null)
            {
                return (400, "Invalid data", null);
            }

            // Validate the role
            if (createUserDTO.Role != UserRole.Admin.ToString() &&
                createUserDTO.Role != UserRole.Therapist.ToString() &&
                createUserDTO.Role != UserRole.Patient.ToString())
            {
                return (400, "Invalid role. Allowed values are: Admin, Therapist, Patient.", null);
            }

            // Hash the password
            using (var sha256 = SHA256.Create())
            {
                var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(createUserDTO.Password)));

                // Create the User entity
                var user = new User
                {
                    UserId = createUserDTO.UserId,
                    Email = createUserDTO.Email,
                    Password = hashedPassword,
                    IsVerified = createUserDTO.IsVerified,
                    UserImageUrl = createUserDTO.UserImageUrl,
                    Username = createUserDTO.Username,
                    Role = createUserDTO.Role,
                };

                _db.Users.Add(user);

                // Save user first to get UserId
                _db.SaveChanges();
               
                // Create related entity based on the role
                if (createUserDTO.Role == UserRole.Therapist.ToString())
                {
                    user.Therapist = new Therapist { UserId = user.UserId };
                    _db.Therapists.Add(user.Therapist);
                }
                else if (createUserDTO.Role == UserRole.Patient.ToString())
                {
                    user.Patient = new Patient { UserId = user.UserId };
                    _db.Patients.Add(user.Patient);
                }

                _db.SaveChanges();

                // Generate JWT Token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSecretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.UserId.ToString()) }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return (201, "User created successfully", tokenString);
            }
        }
    }
}
