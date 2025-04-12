using Domain.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IFraseRepository
    {
        Task<IEnumerable<FraseRomantica>> ObterTodasAsync();
        Task<FraseRomantica?> ObterPorIdAsync(int id);
        Task<int> CriarAsync(FraseRomantica frase);
        Task AtualizarCurtidasFrase(FraseRomantica fraseCurtidaAtualizada);
    }
}
