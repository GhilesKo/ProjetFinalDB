using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppProjetSessionDB.Models.DTO
{
    public class RendezVousDTO
    {


        public DateTime dateDebut { get; set; }
        public DateTime dateFin { get; set; }

        public int PhotographeId { get; set; }

        public List<Photographe> photographes { get; set; }

        public List<RendezVou> listeRendezVous { get; set; }

    }
}
