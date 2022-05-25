using System;
using System.Collections.Generic;

#nullable disable

namespace AppProjetSessionDB.Models
{
    public partial class Disponibilite
    {
        public int DisponibiliteId { get; set; }
        public DateTime DateDisponibilite { get; set; }
        public int PhotographeId { get; set; }
        public TimeSpan HeureDebut { get; set; }
        public int? RendezVousId { get; set; }
        public string Statut { get; set; }

        public virtual Photographe Photographe { get; set; }
        public virtual RendezVou RendezVous { get; set; }

        public Disponibilite()
        {

        }

        public Disponibilite(int disponibiliteId, DateTime dateDisponibilite, int photographeId, TimeSpan heureDebut, int? rendezVousId, string? statut)
        {
            DisponibiliteId = disponibiliteId;
            DateDisponibilite = dateDisponibilite;
            PhotographeId = photographeId;
            HeureDebut = heureDebut;
            RendezVousId = rendezVousId;
            Statut = statut;
        }
    }
}
