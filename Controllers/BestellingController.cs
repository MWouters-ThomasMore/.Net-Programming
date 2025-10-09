using AutoMapper;
using WebAPIDemo.DTO.Bestellingen;

namespace WebAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BestellingController : ControllerBase
    {
        private IUnitOfWork _uow;
        private IMapper _mapper;

        public BestellingController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BestellingDTO>>> AlleBestellingenOphalen()
        {
            List<Bestelling> modellen = await _uow.BestellingRepo.GetAllIncludingProducts();

            if (modellen == null || modellen.Count == 0)
            {
                return NotFound();
            }

            List<BestellingDTO> dtos = _mapper.Map<List<BestellingDTO>>(modellen);

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BestellingDTO>> BestellingOphalen(int id)
        {
            Bestelling model = await _uow.BestellingRepo.GetBestellingIncludingProducts(id);

            if (model == null)
            {
                return NotFound();
            }

            BestellingDTO dto = _mapper.Map<BestellingDTO>(model);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<Bestelling>> BestellingToevoegen(AddBestellingDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Bestelling model = _mapper.Map<Bestelling>(dto);

            await _uow.BestellingRepo.Add(model);
            try
            {
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateException dbError)
            {
                return BadRequest(dbError);
            }

            return CreatedAtAction(nameof(BestellingToevoegen), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> BestellingWijzigen(int id, UpdateBestellingDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("De opgegeven id's komen niet overeen.");
            }

            Bestelling model = _mapper.Map<Bestelling>(dto);
            await _uow.BestellingRepo.Update(model);

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                List<Bestelling> bestellingen = await _uow.BestellingRepo.GetAll();
                if (bestellingen.Any(x => x.Id == id))
                {
                    return NotFound("Er is geen Bestelling met dit id gevonden");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> BestellingVerwijderen(int id)
        {
            Bestelling? bestelling = await _uow.BestellingRepo.GetObject(id);
            if (bestelling == null)
            {
                return NotFound("De bestelling met deze id is niet gevonden.");
            }

            await _uow.BestellingRepo.Delete(bestelling);
            await _uow.SaveChangesAsync();

            return Ok($"Bestelling met id {id} is verwijderd");
        }
    }
}