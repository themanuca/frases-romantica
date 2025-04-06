using Domain.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Interfaces
{
    public interface IFraseRomanticaServices
    {
        Task<IEnumerable<FraseRomantica>> ObterTodasAsync();
        Task<FraseRomantica?> ObterPorIdAsync(int id);
        Task<int> CriarAsync(FraseRomantica frase);
    }
}
