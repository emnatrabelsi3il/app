using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PR3_API.Models;
using Microsoft.AspNetCore.Identity.Data;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using Microsoft.Extensions.Logging;


namespace PR3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly UtilisateurContext _context;

        private readonly ILogger<UtilisateursController> _logger;


        public UtilisateursController(UtilisateurContext context, ILogger<UtilisateursController> logger)
        {
            _context = context;
            _logger = logger;
    }

        // GET: api/Utilisateurs
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateur()
        {
            _logger.LogInformation("Getting weather forecast");

            return await _context.Utilisateur.ToListAsync();
        }

        // GET: api/Utilisateurs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilisateur>> GetUtilisateur(int id)
        {
            var utilisateur = await _context.Utilisateur.FindAsync(id);

            if (utilisateur == null)
            {
                return NotFound();
            }

            return utilisateur;
        }

        // PUT: api/Utilisateurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilisateur(int id, Utilisateur utilisateur)
        {
            if (id != utilisateur.Id)
            {
                return BadRequest();
            }
            utilisateur.MotDePasse = HashString(utilisateur.MotDePasse);
            _context.Entry(utilisateur).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilisateurExists(id))
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

        // POST: api/Utilisateurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /**[HttpPost]
        public async Task<ActionResult<Utilisateur>> PostUtilisateur(Utilisateur utilisateur)
        {
            _context.Utilisateur.Add(utilisateur);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUtilisateur", new { id = utilisateur.Id }, utilisateur);
        }**/

        [AllowAnonymous]
        [HttpPost("Exist")]
        public async Task<IActionResult> ExistUser([FromBody] LoginRequest loginRequest)
        {
            loginRequest.MotDePasse = HashString(loginRequest.MotDePasse);
            var utilisateur = await _context.Utilisateur
                .FirstOrDefaultAsync(u => u.Login == loginRequest.Login && u.MotDePasse == loginRequest.MotDePasse);

            if (utilisateur == null)
            {
                _logger.LogError("L'utilisateur n'existe pas");
                return NotFound();
            }
           string token = GenerateToken("test@test.fr");
           var obj = new { token };
           return Ok(obj);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Utilisateur>> PostEtablissement(Utilisateur utilisateur)
        {
            utilisateur.MotDePasse = HashString(utilisateur.MotDePasse);
            _context.Utilisateur.Add(utilisateur);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUtilisateur", new { id = utilisateur.Id }, utilisateur);
        }

        // DELETE: api/Utilisateurs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilisateur(int id)
        {
            var utilisateur = await _context.Utilisateur.FindAsync(id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            _context.Utilisateur.Remove(utilisateur);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UtilisateurExists(int id)
        {
            return _context.Utilisateur.Any(e => e.Id == id);
        }

        public class LoginRequest
        {
            public string Login { get; set; }
            public string MotDePasse { get; set; }
        }

        static string HashString(string text)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private string GenerateToken(string user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("monSuperSecretmonSuperSecretmonSuperSecretmonSuperSecret"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> { new Claim("username", user) };

            var notBefore = DateTime.UtcNow;
            var expires = notBefore.AddHours(1);

            var token = new JwtSecurityToken(
                issuer: "3iL",
                audience: "API Test",
                claims: claims,
                notBefore: notBefore,
                expires: expires,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    


    }
}
