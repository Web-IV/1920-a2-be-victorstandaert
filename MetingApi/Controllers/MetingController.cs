using Microsoft.AspNetCore.Mvc;
using MetingApi.DTOs;
using MetingApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;

namespace MetingApi.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class MetingController : ControllerBase
    {
        private readonly IMetingRepository _metingRepository;

        public MetingController(IMetingRepository context)
        {
            _metingRepository = context;
        }

        [HttpGet]     
        public IEnumerable<Meting> GetMeting(string resultaatVraag = null) //return op een vraag OF return alles wanneer niks meegegeven
        {
            if (string.IsNullOrEmpty(resultaatVraag))
                return _metingRepository.GetAll();
            return _metingRepository.GetBy(resultaatVraag);
        }

        [HttpGet("{id}")]
        public ActionResult<Meting> GetMeting(int id)
        {
            Meting meting = _metingRepository.GetBy(id);
            if (meting == null) return NotFound();
            return meting;
        }

        [HttpPost]
        public ActionResult<Meting> PostMeting(MetingDTO meting)
        {
            Meting metingToCreate = new Meting() { };
            foreach (var i in meting.Resultaten)
                metingToCreate.AddResultaat(new Resultaat(i.Vraag, i.Amount));
            _metingRepository.Add(metingToCreate);
            _metingRepository.SaveChanges();

            return CreatedAtAction(nameof(GetMeting), new { id = metingToCreate.Id }, metingToCreate);
        }

        [HttpPut("{id}")]
        public IActionResult PutMeting(int id, Meting meting)
        {
            if (id != meting.Id)
            {
                return BadRequest();
            }
            _metingRepository.Update(meting);
            _metingRepository.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMeting(int id)
        {
            Meting meting = _metingRepository.GetBy(id);
            if (meting == null)
            {
                return NotFound();
            }
            _metingRepository.Delete(meting);
            _metingRepository.SaveChanges();
            return NoContent();
        }

        [HttpGet("{id}/resultaten/{resultaatId}")]
        public ActionResult<Resultaat> GetResultaat(int id, int resultaatId)
        {
            if (!_metingRepository.TryGetMeting(id, out var meting))
            {
                return NotFound();
            }
            Resultaat resultaat = meting.GetResultaat(resultaatId);
            if (resultaat == null)
                return NotFound();
            return resultaat;
        }

        [HttpPost("{id}/resultaten")]
        public ActionResult<Resultaat> PostResultaat(int id, ResultaatDTO resultaat)
        {
            if (!_metingRepository.TryGetMeting(id, out var meting))
            {
                return NotFound();
            }
            var resultaatToCreate = new Resultaat(resultaat.Vraag, resultaat.Amount);
            meting.AddResultaat(resultaatToCreate);
            _metingRepository.SaveChanges();
            return CreatedAtAction("Getresultaat", new { id = meting.Id, resultaatId = resultaatToCreate.Id }, resultaatToCreate);
        }
    }
}
