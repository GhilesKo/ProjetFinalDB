using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppProjetSessionDB.Models;
using AppProjetSessionDB.Models.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace AppProjetSessionDB.Controllers
{
    public class RendezVousController : Controller
    {
        private readonly H22_4D5_Projet_sessionContext _context;
        private IConfiguration _configuration;
        private SqlConnection connectionBD;
        public RendezVousController(H22_4D5_Projet_sessionContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: RendezVous
        public async Task<IActionResult> Index()
        {

            var Lesphotographes = _context.Photographes.ToList();
            var RendezVousDTO = new RendezVousDTO
            {
                photographes = Lesphotographes,
                listeRendezVous = new List<RendezVou>()


            };
            return View(RendezVousDTO);




        }

        [HttpPost]
        public async Task<IActionResult> Index(RendezVousDTO rdv)
        {


         
            connectionBD = new SqlConnection(_configuration.GetConnectionString("DefaultConnectionString"));
            connectionBD.Open();

            SqlCommand sqlCommand = connectionBD.CreateCommand();
            sqlCommand.CommandText =$"EXECUTE usp_getRDVphotographe '{rdv.dateDebut}','{rdv.dateFin}',{rdv.PhotographeId}";
            SqlDataReader resultat= sqlCommand.ExecuteReader();

            while ( resultat.Read())
            {
                rdv.listeRendezVous.Add(new RendezVou(int.Parse(resultat["rendezVousID"].ToString()), DateTime.Parse(resultat["dateRendezVous"].ToString()), resultat["commentaire"].ToString(), int.Parse(resultat["proprieteID"].ToString()), TimeSpan.Parse(resultat["heureDebut"].ToString()), resultat["justification"].ToString(), resultat["statutPhoto"].ToString(), resultat["commentairePhoto"].ToString()));
            }

            connectionBD.Close()
            
            return View(rdv);

           




        }

        // GET: RendezVous/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rendezVou = await _context.RendezVous
                .Include(r => r.Propriete)
                .FirstOrDefaultAsync(m => m.RendezVousId == id);
            if (rendezVou == null)
            {
                return NotFound();
            }

            return View(rendezVou);
        }

        // GET: RendezVous/Create
        public IActionResult Create()
        {
            ViewData["ProprieteId"] = new SelectList(_context.Proprietes, "ProprieteId", "Adresse");
            return View();
        }

        // POST: RendezVous/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RendezVousId,DateRendezVous,Commentaire,ProprieteId,HeureDebut,Justification,StatutPhoto,CommentairePhotos")] RendezVou rendezVou)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rendezVou);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProprieteId"] = new SelectList(_context.Proprietes, "ProprieteId", "Adresse", rendezVou.ProprieteId);
            return View(rendezVou);
        }

        // GET: RendezVous/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rendezVou = await _context.RendezVous.FindAsync(id);
            if (rendezVou == null)
            {
                return NotFound();
            }
            ViewData["ProprieteId"] = new SelectList(_context.Proprietes, "ProprieteId", "Adresse", rendezVou.ProprieteId);
            return View(rendezVou);
        }

        // POST: RendezVous/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RendezVousId,DateRendezVous,Commentaire,ProprieteId,HeureDebut,Justification,StatutPhoto,CommentairePhotos")] RendezVou rendezVou)
        {
            if (id != rendezVou.RendezVousId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rendezVou);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RendezVouExists(rendezVou.RendezVousId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProprieteId"] = new SelectList(_context.Proprietes, "ProprieteId", "Adresse", rendezVou.ProprieteId);
            return View(rendezVou);
        }

        // GET: RendezVous/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rendezVou = await _context.RendezVous
                .Include(r => r.Propriete)
                .FirstOrDefaultAsync(m => m.RendezVousId == id);
            if (rendezVou == null)
            {
                return NotFound();
            }

            return View(rendezVou);
        }

        // POST: RendezVous/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rendezVou = await _context.RendezVous.FindAsync(id);
            _context.RendezVous.Remove(rendezVou);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RendezVouExists(int id)
        {
            return _context.RendezVous.Any(e => e.RendezVousId == id);
        }
    }
}
