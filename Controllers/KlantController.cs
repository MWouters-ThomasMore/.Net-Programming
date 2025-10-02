using AutoMapper;
using WebAPIDemo.DTO.Bestellingen;
using WebAPIDemo.DTO.Klant;

namespace WebAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KlantController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private IMapper _mapper;

        public KlantController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // GET: api/Klant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KlantBestellingenDTO>>> GetKlantenMetBestellingen()
        {
            List<Klant> klanten =  await _uow.KlantRepo.GetKlantenMetBestellingen();

            // Mapping
            List<KlantBestellingenDTO> dtos = _mapper.Map<List<KlantBestellingenDTO>>(klanten);

            return Ok(dtos);
        }

        // GET: api/Klant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KlantDTO>> GetKlant(int id)
        {
            Klant model = await _uow.KlantRepo.GetObject(id);

            if (model == null)
            {
                return NotFound();
            }

            KlantDTO dto = _mapper.Map<KlantDTO>(model);

            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<Klant>> klantToevoegen(AddKlantDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Klant model = _mapper.Map<Klant>(dto);
            model.AangemaaktDatum = DateTime.Now;

            await _uow.KlantRepo.Add(model);
            try
            {
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateException dbError)
            {
                return BadRequest(dbError);
            }

            return CreatedAtAction("klantToevoegen", new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> KlantWijzigen(int id, UpdateKlantDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("De opgegeven id's komen niet overeen.");
            }

            Klant model = _mapper.Map<Klant>(dto);
            await _uow.KlantRepo.Update(model);

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                List<Klant> klanten = await _uow.KlantRepo.GetAll();
                if (klanten.Any(x => x.Id == id))
                {
                    return NotFound("Er is geen klant met dit id gevonden");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> KlantVerwijderen(int id)
        {
            Klant? klant = await _uow.KlantRepo.GetObject(id);
            if (klant == null)
            {
                return NotFound("De klant met deze id is niet gevonden.");
            }

            await _uow.KlantRepo.Delete(klant);
            await _uow.SaveChangesAsync();

            return Ok($"klant met id {id} is verwijderd");
        }
    }
}