using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Предпочтения
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferenceController
        : ControllerBase
    {

        private readonly IRepository<Preference> _repository;

        public PreferenceController(IRepository<Preference> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// Получить все предпочтения
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PrefernceResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }
    }
}