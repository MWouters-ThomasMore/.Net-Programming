using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAPIDemo.DTO.Gebruiker;

namespace WebAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GebruikerController : ControllerBase
    {
        private UserManager<CustomUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private SignInManager<CustomUser> _signInManager;

        public GebruikerController(RoleManager<IdentityRole> roleManager, UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            // Defensive coding
            // Modelstate validatie
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Bestaat de gebruiker? Is de email bevestigd?
            CustomUser? user = await _userManager.FindByEmailAsync(dto.Email);
            if (user != null && !user.EmailConfirmed)
            {
                ModelState.AddModelError("message", "Emailadres is nog niet bevestigd.");
                return BadRequest(ModelState);
            }

            // Sign in user
            var result = await _signInManager.PasswordSignInAsync
                (dto.Email, dto.Password, false, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("message", "Account is locked.");
            }

            if (result.Succeeded)
            {
                // Claims = Who IS a user
                var authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Gender, "Attack helicopter"),
                };

                // Roles = In welke rol(len) zit een gebruiker
                IList<string> userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles != null)
                {
                    foreach (string role in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }

                JwtSecurityToken token = Token.GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            else
            {
                ModelState.AddModelError("message", "Ongeldige loginpoging");
                return Unauthorized(ModelState);
            }
        }

        // Lijst gebruikers
        [Route("Lijst")]
        [HttpGet]
        public async Task<ActionResult> GetLijst()
        {
            return Ok(await _userManager.Users.ToListAsync());
        }

        // Eén gebruiker op basis van id
        [HttpGet("{id}")]
        public async Task<ActionResult> Details(string id)
        {
            CustomUser? gebruiker = await _userManager.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

            if (gebruiker != null)
                return Ok(gebruiker);
            else return Ok(null);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegistratieDTO dto)
        {
            // Defensive coding
            // Input validatie met ModelState
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Bestaat gebruiker al?
            var gebruiker = await _userManager.FindByEmailAsync(dto.Email);
            if (gebruiker != null)
            {
                ModelState.AddModelError("message", "Gebruiker bestaal al!");
                return BadRequest(ModelState);
            }

            // Actual business logic starts here -> Useful to avoid unnecessary indentations.
            // Manual mapping -> Ignore email+phone confirmed
            CustomUser user = new CustomUser
            {
                UserName = dto.Email,
                NormalizedEmail = dto.Email,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            // Register user to DB
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                return Created();
            }
            else
            {
                if (result.Errors.Count() > 0)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("message", error.Description);
                }
                return BadRequest(ModelState);
            }
        }
    }
}