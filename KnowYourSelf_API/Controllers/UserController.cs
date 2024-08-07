using KnowYourSelf_API.Data;
using Microsoft.AspNetCore.Mvc;
using KnowYourSelf_API.Models;
using Microsoft.EntityFrameworkCore;
using KnowYourSelf_API.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace KnowYourSelf_API.Controllers
{
    [ApiController]
    public class UserController : BaseController
    {
        //public UserController(ApplicationDbContext db) : base(db) { }
        private readonly AuthenticateService _authenticateService;
        private readonly RegisterService _registerService;
        private readonly IConfiguration _configuration;

        public UserController(ApplicationDbContext db, AuthenticateService authenticateService, RegisterService registerService, IConfiguration configuration) : base(db)
        {
            _authenticateService = authenticateService;
            _registerService = registerService;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = _db.Users.ToList();

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        // endpoint to get user per id
        [HttpGet("{userId:int}", Name = "GetUser")] // explicitely provide the type
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> GetUser(int userId)
        {
            if (userId == 0)
            {
                return BadRequest();
            }
            //var user = _db.Users.FirstOrDefault(u => u.UserId == userId);
            var user = _db.Users
              .Include(u => u.Patient) 
              .Include(u => u.Therapist) 
              .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("{userId:int}", Name = "UpdateUserImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateUserImage(int userId, [FromBody] UpdateUserDTO userDTO)
        {

            if (userDTO == null || userId != userDTO.UserId)
            {
                return BadRequest();
            }

            var user = _db.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            user.UserImageUrl = userDTO.UserImageUrl;

            _db.Users.Update(user);
            _db.SaveChanges();
            return Ok(user);
        }

        // endpoint to delete user
        [HttpDelete("{userId:int}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteUser(int userId) //IActionResult do not hacve to specify return type; ActionResult should specify result type
        {
            if (userId == 0)
            {
                return BadRequest();
            }

            var user = _db.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            if (user.Role.ToLower().Equals("patient"))
            {
                var patient = _db.Patients.FirstOrDefault(p => p.UserId == userId);
                _db.Patients.Remove(patient);
            }
            else if (user.Role.ToLower().Equals("therapist"))
            {
                var therapist = _db.Therapists.FirstOrDefault(th => th.UserId == userId);
                Console.WriteLine(user.Role);
                _db.Therapists.Remove(therapist);
            }

            _db.Users.Remove(user);
            _db.SaveChanges();
            return NoContent(); // or success 200
        }

        [HttpPost("login", Name = "Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            var result = _authenticateService.Authenticate(loginDTO.Email, loginDTO.Password);

            if (result.Status == 200)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("user_id", result.User.UserId.ToString()) }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { Message = result.Message, User = result.User, Token = tokenString });
            }

            return StatusCode(result.Status, new { Message = result.Message });
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Register([FromBody] CreateUserDTO createUserDTO)
        {
            var result = _registerService.RegisterUser(createUserDTO);

            if (result.Status == 201)
            {
                return CreatedAtRoute("GetUser", new { userId = createUserDTO.UserId }, new { message = result.Message, token = result.Token });
            }
            if (result.Status == 409)
            {
                return Conflict(new { message = result.Message });
            }
            return BadRequest(new { message = result.Message });
        }

    }

}
