using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Exceptions;
using Otus.Teaching.PromoCodeFactory.Core.UseCases;
using Otus.Teaching.PromoCodeFactory.Domain.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Партнеры
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PartnersController
        : ControllerBase
    {
        private readonly PartnersCase _partnersManager;

        public PartnersController(PartnersCase partnersManager)
        {
            _partnersManager = partnersManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<PartnerResponse>>> GetPartnersAsync()
        {
            try
            {

                var response = await _partnersManager.GetPartnersAsync();
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PartnerResponse>> GetPartnerByIdAsync(Guid id)
        {
            try
            {

                var response = await _partnersManager.GetPartnerByIdAsync(id);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }


        [HttpGet("{id}/limits/{limitId}")]
        public async Task<IActionResult> GetPartnerLimitAsync(Guid id, Guid limitId)
        {
            try
            {

                var response = await _partnersManager.GetPartnerLimitAsync(id, limitId);

                if (response == null)
                    return NoContent();

                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }

        [HttpPost("{id}/limits")]
        public async Task<IActionResult> SetPartnerPromoCodeLimitAsync(Guid id, SetPartnerPromoCodeLimitRequest request)
        {
            try
            {

                var (partnerId, newLimitId) = await _partnersManager.SetPartnerPromoCodeLimitAsync(id, request);
                return await GetPartnerLimitAsync(partnerId, newLimitId);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }

        }

        [HttpPost("{id}/canceledLimits")]
        public async Task<IActionResult> CancelPartnerPromoCodeLimitAsync(Guid id)
        {
            try
            {

                await _partnersManager.CancelPartnerPromoCodeLimitAsync(id);

                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }

        }
    }
}
