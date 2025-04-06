using Dapper;
using Domain.Entities.Models;
using Infra.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repository
{
    public class FraseRepository
    {
        private readonly DBContext _context;
        public FraseRepository(DBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<FraseRomantica>> ObterTodasAsync()
        {
            var sql = "SELECT * FROM FrasesRomanticas ORDER BY Id DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<FraseRomantica>(sql);
        }

        public async Task<FraseRomantica?> ObterPorIdAsync(int id)
        {
            var sql = "SELECT * FROM FrasesRomanticas WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<FraseRomantica>(sql, new { Id = id });
        }

        public async Task<int> CriarAsync(FraseRomantica frase)
        {
            var sql = @"INSERT INTO FrasesRomanticas (Texto, Tema, Curtidas, CriadoEm) 
                    VALUES (@Texto, @Tema, @Curtidas, @CriadoEm);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, frase);
        }
    }
}
