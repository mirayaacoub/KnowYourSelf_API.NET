using KnowYourSelf_API.Data;
using Microsoft.AspNetCore.Mvc;
namespace KnowYourSelf_API.Controllers
{
    [Route("api/KnowYourSelfAPI/v1/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly ApplicationDbContext _db;
        public BaseController(ApplicationDbContext db)
        {
            _db = db;
        }
    }
}
