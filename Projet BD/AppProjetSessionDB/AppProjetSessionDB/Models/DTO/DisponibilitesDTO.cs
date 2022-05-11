using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppProjetSessionDB.Models.DTO
{
    public class DisponibilitesDTO
    {


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:s}")]
        public DateTime dateDebut { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:s}")]
        public DateTime dateFin { get; set; }


        public int PhotographeId { get; set; }

        public List<Photographe> photographes { get; set; }

        public List<Disponibilite> disponibilites { get; set; }


    }
}
