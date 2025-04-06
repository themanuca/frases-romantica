using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Models
{
    public class FraseRomantica
    {
        public int Id { get; set; }
        public string Texto { get; set; } = string.Empty;
        public string Tema { get; set; } = string.Empty;
        public int Curtidas { get; set; } = 0;
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}
