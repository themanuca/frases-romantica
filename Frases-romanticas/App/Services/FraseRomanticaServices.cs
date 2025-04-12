using App.Interfaces;
using Contract.DTOs;
using Domain.Entities.Models;
using Domain.Interfaces;
using Domain.Messaging;
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
        private readonly IFraseProducer _producer;

        public FraseRomanticaServices(IFraseRepository fraseRepository, IFraseProducer fraseProducer)
        {
            _fraseRepository = fraseRepository;
            _producer = fraseProducer;
        }

        public async Task AtualizarCurtidaFrase(CurtidasFraseDTO curtidasFraseDTO)
        {
            if (curtidasFraseDTO.IdFrase <= 0)
            {
                throw new ArgumentNullException("Identificação vazia");
            }
            try
            {
                var result = await _fraseRepository.ObterPorIdAsync(curtidasFraseDTO.IdFrase);
                if (result == null)
                {
                    throw new ArgumentNullException("Frase não encontrada.");
                }
                if(result.Curtidas >= curtidasFraseDTO.NumeroCurtidas)
                {
                    return;
                }
                var curtidaAtualizada = new FraseRomantica
                {
                    Id = curtidasFraseDTO.IdFrase,
                    Curtidas = curtidasFraseDTO.NumeroCurtidas
                };
                await _fraseRepository.AtualizarCurtidasFrase(curtidaAtualizada);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Ocorreu um erro interno ao buscar a frase.", ex);
            }
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
                _producer.Publicar(frase.Texto);
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
