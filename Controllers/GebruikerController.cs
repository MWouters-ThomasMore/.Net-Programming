using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAPIDemo.Data.Resources;
using WebAPIDemo.DTO.Gebruiker;

namespace WebAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = StringResources.Role_Admin)]
    //[Authorize(Roles = "admin")]
    public class GebruikerController : ControllerBase
    {
        private UserManager<CustomUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private SignInManager<CustomUser> _signInManager;
        private IMapper _mapper;

        public GebruikerController(RoleManager<IdentityRole> roleManager, UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGebruiker(string id, GebruikerWijzigenDto dto)
        {
            if (dto == null || id != dto.Id)
            {
                return BadRequest(ModelState);
            }

            CustomUser? user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return NotFound(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Update model met nieuwe data uit DTO
            user = _mapper.Map<CustomUser>(dto);

            // Wachtwoord wijzigen
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("error", error.Description);
                    }

                    return BadRequest(ModelState);
                }
            }

            // Update user
            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return Ok("De gebruiker is gewijzigd");
            }
            else
            {
                if (updateResult.Errors.Any())
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError("message", error.Description);
                    }
                }
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (string id)
        {
            CustomUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(ModelState);
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok("De gebruiker is succesvol verwijderd.");
            }
            else
            {
                if (result.Errors.Any())
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("message", error.Description);
                    }
                }
                return BadRequest(ModelState);
            }
        }

        [HttpPost("EditPermission")]
        public async Task<IActionResult> EditPermission(EditPermissionDTO dto)
        {
            // Defensive coding -> Bestaat de gebruiker & de rol?
            CustomUser? gebruiker = await _userManager.FindByEmailAsync(dto.Email);
            if (gebruiker == null)
            {
                ModelState.AddModelError("error", "gebruiker bestaat niet");
                return BadRequest(ModelState);
            }

            IdentityRole? rol = await _roleManager.FindByNameAsync(dto.RolNaam);
            if (rol == null)
            {
                ModelState.AddModelError("error", "rol bestaat niet");
                return BadRequest(ModelState);
            }

            IdentityResult result = null;

            if (dto.Operation == EditOperation.Grant)
            {
                result = await _userManager.AddToRoleAsync(gebruiker, rol.Name);
            }
            else if (dto.Operation == EditOperation.Remove)
            {
                result = await _userManager.RemoveFromRoleAsync(gebruiker, rol.Name);
            }

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("GetAlleUsersMetRollen")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAlleUsersMetRollen()
        {
            List<CustomUser> users = await _userManager.Users.ToListAsync();

            // We zetten om naar dto omdat we nooit met modellen met de user willen communiceren
            List<GebruikerMetRollenDTO> dtos = _mapper.Map<List<GebruikerMetRollenDTO>>(users);

            // Haal rollen op
            for (int i = 0; i < dtos.Count; i++)
            {
                GebruikerMetRollenDTO dto = dtos[i];
                dto.Roles = await _userManager.GetRolesAsync(users[i]);
            }

            return Ok(dtos);
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
                // Voeg standaard rol toe bij het registreren.
                //await _userManager.AddToRoleAsync(user, "user");
                await _userManager.AddToRoleAsync(user, StringResources.Role_User);

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