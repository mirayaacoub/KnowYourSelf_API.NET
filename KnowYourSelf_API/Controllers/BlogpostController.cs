using KnowYourSelf_API.Data;
using KnowYourSelf_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KnowYourSelf_API.Controllers
{
    [ApiController]
    public class BlogpostController : BaseController
    {
        public BlogpostController(ApplicationDbContext db) : base(db) { }

        // endpoint to retrieve all exisitng blogposts
        [HttpGet(Name = "GetAllBlogposts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Blogpost>> GetAllBlogposts()
        {
            var blogposts = _db.Blogposts.ToList();

            if (blogposts == null)
            {
                return NotFound();
            }

            return Ok(blogposts);
        }

        // endpoint to get blogposts by therapist id
        [HttpGet("{therapistId:int}", Name = "GetBlogpostsByTherapist")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Blogpost>> GetBlogpostByTherapist(int therapistId)
        {
            if (therapistId == 0)
            {
                return BadRequest();
            }

            var therapist = _db.Therapists.FirstOrDefault(th => th.TherapistId == therapistId);

            if (therapist == null)
            {
                return NotFound();
            }

            var blogposts = _db.Blogposts
                .Where(b => b.TherapistId == therapist.TherapistId)
                .Include(t => t.Therapist)
                .ToList();  
            return Ok(blogposts);
        }

        // endpoint to get blogposts by therapist id
        [HttpGet("{title}", Name = "GetBlogpostsByTitle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Blogpost>> GetBlogpostsByTitle(string title)
        {
            if (title == "")
            {
                return BadRequest();
            }

            var blogpost = _db.Blogposts.FirstOrDefault(b => b.BlogpostTitle == title);

            if (blogpost == null)
            {
                return NotFound();
            }

            var blogposts = _db.Blogposts
                .Where(b => b.BlogpostTitle == blogpost.BlogpostTitle)
                .Include(t => t.Therapist)
                .ToList();
            return Ok(blogposts);
        }

        // endpoint to post a blogpost
        [HttpPost("{therapistId:int}", Name = "CreateBlogpost")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Blogpost> CreateBlogpost(int therapistId, [FromBody] CreateBlogpostDTO createBlogpostDTO)
        {
            if (therapistId == 0)
            {
                return BadRequest();
            }

            var therapist = _db.Therapists.FirstOrDefault(th => th.TherapistId == therapistId);

            if (therapist == null)
            {
                return NotFound();
            }

            Blogpost blogpost = new()
            {
                BlogpostId = createBlogpostDTO.BlogpostId,
                BlogpostTitle = createBlogpostDTO.BlogpostTitle,
                BlogpostContent = createBlogpostDTO.BlogpostContent,
                Category = createBlogpostDTO.Category,
                BlogpostImage = createBlogpostDTO.BlogpostImage,
                TherapistId = therapistId,  
                CreatedAt = DateTime.UtcNow,
            };

            _db.Blogposts.Add(blogpost);
            _db.SaveChanges();

            return CreatedAtRoute("GetBlogPostByTherapist", new { therapistId = blogpost.TherapistId }, therapist);
        }

        // endpoint to update blogpost information
        [HttpPut("{blogId:int}", Name = "UpdateBlogpost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateBlogpost(int blogId, [FromBody] UpdateBlogpostDTO updateBlogpostDTO)
        {

            if (updateBlogpostDTO == null || blogId != updateBlogpostDTO.BlogpostId)
            {
                return BadRequest();
            }

            var blogpost = _db.Blogposts.FirstOrDefault(b => b.BlogpostId == blogId);

            if (blogpost == null)
            {
                return NotFound();
            }

            blogpost.BlogpostTitle = updateBlogpostDTO.BlogpostTitle;
            blogpost.BlogpostContent = updateBlogpostDTO.BlogpostContent;
            blogpost.BlogpostContent = updateBlogpostDTO.BlogpostImage;
            blogpost.UpdatedAt = DateTime.UtcNow;

            _db.Blogposts.Update(blogpost);
            _db.SaveChanges();
            return Ok(blogpost);
        }

        // endpoint to delete blogpost
        [HttpDelete("{blogId:int}", Name = "DeleteBlogpost")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteBlogpost(int blogId)
        { 
            if (blogId == 0)
            {
                return BadRequest();
            }

            var blogpost = _db.Blogposts.FirstOrDefault(b => b.BlogpostId == blogId);

            if (blogpost == null)
            {
                return NotFound();
            }

            _db.Blogposts.Remove(blogpost);
            _db.SaveChanges();
            return NoContent();
        }
    }
}
