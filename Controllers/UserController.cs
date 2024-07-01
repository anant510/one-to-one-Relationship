using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using relationshipOnetoone.context;
using relationshipOnetoone.Dto;
using relationshipOnetoone.models;

namespace relationshipOnetoone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.UserProfile)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    UserProfile = new UserProfileDto
                    {
                        UserProfileId = u.UserProfile.UserProfileId,
                        FullName = u.UserProfile.FullName
                    }
                })
                .ToListAsync();

            return Ok(users);
        }


        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                UserProfile = new UserProfileDto
                {
                    UserProfileId = user.UserProfile.UserProfileId,
                    FullName = user.UserProfile.FullName
                }
            };

            return userDto;
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                UserProfile = new UserProfile
                {
                    FullName = userDto.UserProfile.FullName
                }
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            userDto.UserId = user.UserId;
            userDto.UserProfile.UserProfileId = user.UserProfile.UserProfileId;

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, userDto);
        }


        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
        {
            if (id != userDto.UserId)
            {
                return BadRequest();
            }

            var user = await _context.Users
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            user.Username = userDto.Username;
            user.UserProfile.FullName = userDto.UserProfile.FullName;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
