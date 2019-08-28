using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FutbolSys.Domain
{
    public class Match
    {
        [Key]
        public int MatchId { get; set; }

        [Display(Name = "Fecha")]
        public int DateId { get; set; }

        [Display(Name = "Día del Juego")]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }

        [Display(Name = "Local")]
        public int LocalId { get; set; }

        [Display(Name = "Visitante")]
        public int VisitorId { get; set; }

        [Display(Name = "Goles del Local")]
        public int? LocalGoals { get; set; }

        [Display(Name = "Goles del Visitante")]
        public int? VisitorGoals { get; set; }

        [Display(Name = "Estado")]
        public int StatusId { get; set; }

        [Display(Name = "Grupo")]
        public int TournamentGroupId { get; set; }

        public virtual Date Date { get; set; }

        public virtual Team Local { get; set; }

        public virtual Team Visitor { get; set; }

        public virtual Status Status { get; set; }

        public virtual TournamentGroup TournamentGroup { get; set; }

        public virtual ICollection<Prediction> Predictions { get; set; }
    }
}
