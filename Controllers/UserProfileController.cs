using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using relationshipOnetoone.context;
using relationshipOnetoone.Dto;
using relationshipOnetoone.models;

namespace relationshipOnetoone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserProfileController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/userprofiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetUserProfiles()
        {
            var userProfiles = await _context.UserProfiles
                .Select(up => new UserProfileDto
                {
                    UserProfileId = up.UserProfileId,
                    FullName = up.FullName,
                    UserId = up.UserId
                })
                .ToListAsync();

            return Ok(userProfiles);
        }

        // GET: api/userprofiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile(int id)
        {
            var userProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(up => up.UserProfileId == id);

            if (userProfile == null)
            {
                return NotFound();
            }

            var userProfileDto = new UserProfileDto
            {
                UserProfileId = userProfile.UserProfileId,
                FullName = userProfile.FullName,
                UserId = userProfile.UserId
            };

            return Ok(userProfileDto);
        }

        // POST: api/userprofiles
        [HttpPost]
        public async Task<ActionResult<UserProfileDto>> CreateUserProfile([FromBody] UserProfileDto userProfileDto)
        {
            try
            {
                var userProfile = new UserProfile
                {
                    FullName = userProfileDto.FullName,
                    UserId = userProfileDto.UserId  // Use the UserId provided by the user
                };

                _context.UserProfiles.Add(userProfile);
                await _context.SaveChangesAsync();

                userProfileDto.UserProfileId = userProfile.UserProfileId;

                return CreatedAtAction(nameof(GetUserProfile), new { id = userProfile.UserProfileId }, userProfileDto);
            }
            catch (DbUpdateException ex)
            {
                // Log the exception or inspect ex.InnerException
                Console.WriteLine($"DbUpdateException occurred: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, "Database error occurred while saving data.");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }




        // PUT: api/userprofiles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserProfile(int id, UserProfileDto userProfileDto)
        {
            if (id != userProfileDto.UserProfileId)
            {
                return BadRequest();
            }

            var userProfile = await _context.UserProfiles.FindAsync(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            userProfile.FullName = userProfileDto.FullName;
            userProfile.UserId = userProfileDto.UserId;

            _context.Entry(userProfile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserProfileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/userprofiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserProfile(int id)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            _context.UserProfiles.Remove(userProfile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserProfileExists(int id)
        {
            return _context.UserProfiles.Any(up => up.UserProfileId == id);
        }
    }
}
