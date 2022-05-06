using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppProjetSessionDB.Models.DTO
{
    public class RendezVousDTO
    {

        [DataType(DataType.Date)]
        public DateTime dateDebut { get; set; }

        [DataType(DataType.Date)]
        public DateTime dateFin { get; set; }

  
        public int PhotographeId { get; set; }

        public List<Photographe> photographes { get; set; }

        public List<RendezVou> listeRendezVous { get; set; }

    }
}
