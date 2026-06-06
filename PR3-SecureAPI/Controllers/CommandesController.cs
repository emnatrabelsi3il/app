using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PR3_SecureAPI.Models;

namespace PR3_SecureAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandesController : ControllerBase
    {
        private readonly PosteContext _context;

        public CommandesController(PosteContext context)
        {
            _context = context;
        }

        // WPF admin : créer une commande pour un seul poste
        [Authorize]
        [HttpPost("poste/{posteId}/refresh")]
        public async Task<IActionResult> RefreshOnePoste(int posteId)
        {
            var poste = await _context.Poste.FindAsync(posteId);

            if (poste == null)
            {
                return NotFound("Poste introuvable.");
            }

            var commande = new CommandePoste
            {
                PosteId = poste.Id,
                MacAdress = poste.MacAdress,
                TypeCommande = "RefreshInfo",
                Portee = "Single",
                Statut = "Pending",
                DateCreation = DateTime.Now
            };

            _context.CommandesPostes.Add(commande);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Commande créée pour le poste sélectionné.",
                commande.Id,
                poste.Numero,
                poste.MacAdress
            });
        }

        // WPF admin : créer une commande pour tous les postes
        [Authorize]
        [HttpPost("global/refresh")]
        public async Task<IActionResult> RefreshAllPostes()
        {
            var postes = await _context.Poste.ToListAsync();

            if (!postes.Any())
            {
                return NotFound("Aucun poste trouvé.");
            }

            foreach (var poste in postes)
            {
                var commande = new CommandePoste
                {
                    PosteId = poste.Id,
                    MacAdress = poste.MacAdress,
                    TypeCommande = "RefreshInfo",
                    Portee = "Global",
                    Statut = "Pending",
                    DateCreation = DateTime.Now
                };

                _context.CommandesPostes.Add(commande);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Commandes globales créées avec succès.",
                nombrePostes = postes.Count
            });
        }

        // AgentService : récupérer les commandes en attente pour sa MAC
        [AllowAnonymous]
        [HttpGet("pending/{macAdress}")]
        public async Task<IActionResult> GetPendingCommands(string macAdress)
        {
            var commandes = await _context.CommandesPostes
                .Where(c => c.MacAdress == macAdress && c.Statut == "Pending")
                .OrderBy(c => c.DateCreation)
                .ToListAsync();

            return Ok(commandes);
        }

        // AgentService : marquer une commande comme terminée
        [AllowAnonymous]
        [HttpPost("{id}/complete")]
        public async Task<IActionResult> CompleteCommand(int id, [FromBody] CompleteCommandeDto dto)
        {
            var commande = await _context.CommandesPostes.FindAsync(id);

            if (commande == null)
            {
                return NotFound("Commande introuvable.");
            }

            commande.Statut = dto.Success ? "Done" : "Failed";
            commande.Resultat = dto.Resultat;
            commande.DateExecution = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Commande mise à jour.",
                commande.Id,
                commande.Statut,
                commande.Resultat
            });
        }

        // WPF admin : voir l'historique des commandes
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCommandes()
        {
            var commandes = await _context.CommandesPostes
                .OrderByDescending(c => c.DateCreation)
                .ToListAsync();

            return Ok(commandes);
        }
    }
}