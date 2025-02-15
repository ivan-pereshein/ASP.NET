using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Repositories;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private readonly IRepository<PromoCode> _promocodeRepository;

        public PromocodesController(IRepository<PromoCode> promocodeRepository)
        {
            _promocodeRepository = promocodeRepository;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            IEnumerable<PromoCode> list = await _promocodeRepository.GetAllAsync();

            List<PromoCodeShortResponse> listResponse = list.Select(p => new PromoCodeShortResponse
            {
                Id = p.Id,
                Code = p.Code,
                PartnerName = p.PartnerName,
                ServiceInfo = p.ServiceInfo,
                BeginDate = p.BeginDate.ToLongDateString(),
                EndDate = p.EndDate.ToLongDateString()
            }).ToList();

            return Ok(listResponse);
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            // TODO: Создать промокод и выдать его клиентам с указанным предпочтением
            throw new NotImplementedException();
        }
    }
}