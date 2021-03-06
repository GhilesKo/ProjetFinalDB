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
                listeRendezVous = _context.RendezVous.ToList()
                

            };



            return View(RendezVousDTO);




        }

        [HttpPost]
        public async Task<IActionResult> Index(RendezVousDTO rdv)
        {


         
            connectionBD = new SqlConnection(_configuration.GetConnectionString("DefaultConnectionString"));
            connectionBD.Open();

            var debut = rdv.dateDebut.ToString("yyyy-MM-dd");
            var fin = rdv.dateFin.ToString("yyyy-MM-dd");
            SqlCommand sqlCommand = connectionBD.CreateCommand();
            sqlCommand.CommandText =$"EXECUTE usp_getRDVphotographe '{debut}','{fin}',{rdv.PhotographeId}";
            SqlDataReader resultat= sqlCommand.ExecuteReader();
                

            while ( resultat.Read())
            {
                rdv.listeRendezVous.Add(new RendezVou
                    (int.Parse(resultat["rendezVousID"].ToString()),
                    DateTime.Parse(resultat["dateRendezVous"].ToString()),
                    resultat["commentaire"].ToString(), 
                    int.Parse(resultat["proprieteID"].ToString()),
                    TimeSpan.Parse(resultat["heureDebut"].ToString()), 
                    resultat["justification"].ToString(),
                    resultat["statutPhoto"].ToString(), 
                    resultat["commentairePhotos"].ToString()));
            }
            resultat.Close();
            connectionBD.Close();

           
            
            return View(rdv);

           




        }
       
        [HttpPost]
        public async Task<IActionResult> ConfirmerRDV(RendezVousDTO rdv, int rdvID) {

            var disponibilité = await _context.Disponibilites.FirstOrDefaultAsync(d=>d.RendezVousId== rdvID);
            if (disponibilité == null)
            {
                return NoContent();
            }
            disponibilité.Statut = "Occupé";
           await _context.SaveChangesAsync();

            return NoContent();
        
        }

        [HttpPost]
        public async Task<IActionResult> RefuserRDV(int rdvID)
        {

            var disponibilité = await _context.Disponibilites.FirstOrDefaultAsync(d => d.RendezVousId == rdvID);
            if (disponibilité == null)
            {
                return NoContent();
            }
        

            var rdv = await _context.RendezVous.FirstOrDefaultAsync(r => r.RendezVousId == rdvID);
            _context.RendezVous.Remove(rdv);
            disponibilité.Statut = "Libre";
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

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
        public IActionResult Create(int? dispoId)
        {
            ViewData["ProprieteId"] = new SelectList(_context.Proprietes, "ProprieteId", "NomProprio");
            ViewData["dispoId"] = dispoId;  
            return View();
        }

        // POST: RendezVous/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RendezVousId,Commentaire,ProprieteId,Justification,StatutPhoto,CommentairePhotos")] RendezVou rendezVou, int dispoId)
        {
            if (ModelState.IsValid)
            {
                var dispo = await _context.Disponibilites.FirstOrDefaultAsync(d => d.DisponibiliteId == dispoId);
                rendezVou.DateRendezVous = dispo.DateDisponibilite;
                rendezVou.HeureDebut = dispo.HeureDebut;
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
