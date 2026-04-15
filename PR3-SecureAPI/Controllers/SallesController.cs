using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PR3_SecureAPI.Models;
using System.Text.RegularExpressions;

namespace PR3_SecureAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SallesController : ControllerBase
    {
        private readonly SalleContext _context;

        public SallesController(SalleContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Salle>>> GetClasseItems()
        {
            return await _context.Salle.ToListAsync();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Salle>> GetClasseItem(int id)
        {
            var classeItem = await _context.Salle.FindAsync(id);

            if (classeItem == null)
            {
                return NotFound();
            }

            return classeItem;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutClasseItem(int id, Salle classeItem)
        {
            if (id != classeItem.Id)
            {
                return BadRequest("Id invalide.");
            }

            if (classeItem.EtablissementId <= 0)
            {
                return BadRequest("Veuillez sélectionner un établissement valide.");
            }

            if (!NumeroSalleEstValide(classeItem.Numero))
            {
                return BadRequest("Le numéro de salle doit contenir uniquement des chiffres.");
            }

            classeItem.Numero = FormaterNumeroSalle(classeItem.Numero);

            bool existe = await SalleExisteDansMemeEtablissement(
                classeItem.Numero,
                classeItem.EtablissementId,
                classeItem.Id
            );

            if (existe)
            {
                return Conflict("Cette salle existe déjà dans le même établissement.");
            }

            _context.Entry(classeItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClasseItemExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Salle>> PostClasseItem(Salle classeItem)
        {
            if (classeItem == null)
            {
                return BadRequest("Données invalides.");
            }

            if (classeItem.EtablissementId <= 0)
            {
                return BadRequest("Veuillez sélectionner un établissement valide.");
            }

            if (!NumeroSalleEstValide(classeItem.Numero))
            {
                return BadRequest("Le numéro de salle doit contenir uniquement des chiffres.");
            }

            classeItem.Numero = FormaterNumeroSalle(classeItem.Numero);

            bool existe = await SalleExisteDansMemeEtablissement(
                classeItem.Numero,
                classeItem.EtablissementId
            );

            if (existe)
            {
                return Conflict("Cette salle existe déjà dans le même établissement.");
            }

            _context.Salle.Add(classeItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClasseItem", new { id = classeItem.Id }, classeItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClasseItem(int id)
        {
            var classeItem = await _context.Salle.FindAsync(id);
            if (classeItem == null)
            {
                return NotFound();
            }

            _context.Salle.Remove(classeItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClasseItemExists(int id)
        {
            return _context.Salle.Any(e => e.Id == id);
        }

        private bool NumeroSalleEstValide(string? numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                return false;

            numero = numero.Trim();

            return Regex.IsMatch(numero, @"^\d+$");
        }

        private string FormaterNumeroSalle(string numero)
        {
            return $"Salle {numero.Trim()}";
        }

        private async Task<bool> SalleExisteDansMemeEtablissement(string numeroFormate, int etablissementId, int? salleIdExclue = null)
        {
            return await _context.Salle.AnyAsync(s =>
                s.EtablissementId == etablissementId &&
                s.Numero.ToLower() == numeroFormate.ToLower() &&
                (!salleIdExclue.HasValue || s.Id != salleIdExclue.Value));
        }
    }
}