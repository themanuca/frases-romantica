using App.Interfaces;
using Domain.Entities.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace App.Services
{
    public class FraseRomanticaServices : IFraseRomanticaServices
    {
        private readonly IFraseRepository _fraseRepository;

        public FraseRomanticaServices(IFraseRepository fraseRepository)
        {
            _fraseRepository = fraseRepository;
        }
        public async Task<int> CriarAsync(FraseRomantica frase)
        {
            if (frase == null)
            {
                 throw new ArgumentException("Objeto Vazio", nameof(frase));
            }

            if (string.IsNullOrWhiteSpace(frase.Texto))
            {
                throw new ArgumentException("Texto vazio");
            }
            try
            {
                var result = await _fraseRepository.CriarAsync(frase);
                if(result <= 0)
                {
                    throw new ArgumentException("Frase não gravada"); 
                }
                return result;
            }
            catch (Exception ex)
            { 
                throw new ArgumentException("Ocorreu um erro interno ao criar a frase.", ex);
            }
        }

        public async Task<FraseRomantica?> ObterPorIdAsync(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentNullException("Identificação vazia");
            }
            try
            {
                var result = await _fraseRepository.ObterPorIdAsync(id);
                if(result == null)
                {
                    throw new ArgumentNullException("Frase não encontrada.");

                }
                return result;

            }catch(Exception ex)
            {
                throw new ArgumentException("Ocorreu um erro interno ao buscar a frase.", ex);
            }
        }

        public async Task<IEnumerable<FraseRomantica>> ObterTodasAsync()
        {
            try
            {
                var result = await _fraseRepository.ObterTodasAsync();
                if (!result.Any())
                {
                    throw new ArgumentNullException("Ainda não há frases salvas.");
                }
                return result;


            }
            catch (Exception ex)
            {
                throw new ArgumentException("Ocorreu um erro interno ao buscar a frase.", ex);
            }
        }
    }
}
