using AutoMapper;
using WebAPIDemo.DTO.Orderlijn;

namespace WebAPIDemo.Controllers
{
    public class OrderlijnController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public OrderlijnController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // GET: api/Orderlijn
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderlijnDTO>>> GetOrderlijnen()
        {
            List<Orderlijn> modellen = await _uow.OrderlijnRepo.GetAll();
            return _mapper.Map<List<OrderlijnDTO>>(modellen);
        }

        // GET: api/Orderlijn/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderlijnDTO>> GetOrderlijn(int id)
        {
            Orderlijn model = await _uow.OrderlijnRepo.GetObject(id);

            if (model == null)
            {
                return NotFound();
            }

            OrderlijnDTO dto = _mapper.Map<OrderlijnDTO>(model);

            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<Orderlijn>> OrderlijnToevoegen(AddOrderLijnDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Orderlijn model = _mapper.Map<Orderlijn>(dto);
            await _uow.OrderlijnRepo.Add(model);
            try
            {
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateException dbError)
            {
                return BadRequest(dbError);
            }

            return CreatedAtAction("OrderlijnToevoegen", new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> OrderlijnWijzigen(int id, UpdateOrderLijnDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("De opgegeven id's komen niet overeen.");
            }

            Orderlijn model = _mapper.Map<Orderlijn>(dto);
            await _uow.OrderlijnRepo.Update(model);

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                List<Orderlijn> Orderlijnen = await _uow.OrderlijnRepo.GetAll();
                if (Orderlijnen.Any(x => x.Id == id))
                {
                    return NotFound("Er is geen Orderlijn met dit id gevonden");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> OrderlijnVerwijderen(int id)
        {
            Orderlijn? Orderlijn = await _uow.OrderlijnRepo.GetObject(id);
            if (Orderlijn == null)
            {
                return NotFound("De Orderlijn met deze id is niet gevonden.");
            }

            await _uow.OrderlijnRepo.Delete(Orderlijn);
            await _uow.SaveChangesAsync();

            return Ok($"Orderlijn met id {id} is verwijderd");
        }
    }
}