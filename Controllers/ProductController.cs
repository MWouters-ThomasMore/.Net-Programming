using AutoMapper;
using WebAPIDemo.DTO.Product;

namespace WebAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IUnitOfWork _uow;
        private IMapper _mapper;

        public ProductController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<ProductDTO>>> Get()
        {
            List<Product> producten = await _uow.ProductRepo.GetAll();

            if (producten.Count > 0)
            {
                List<ProductDTO> dtos = _mapper.Map<List<ProductDTO>>(producten);
                return Ok(dtos);
            }
            else
            {
                return NotFound("Er zijn geen resultaten gevonden");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            Product model = await _uow.ProductRepo.GetObject(id);

            if (model == null)
            {
                return NotFound($"Product {id} kan niet worden gevonden in de database");
            }

            ProductDTO dto = _mapper.Map<ProductDTO>(model);
            return Ok(dto);
        }

        [HttpGet("Search")]
        public async Task<ActionResult<List<ProductDTO>>> Search(string zoekwaarde)
        {
            List<Product> modellen = await _uow.ProductRepo.SearchByNameAsync(zoekwaarde);

            if (modellen == null)
            {
                return NotFound($"Er zijn geen producten in de database waar {zoekwaarde} voorkomt in de naam");
            }

            List<ProductDTO> dtos = _mapper.Map<List<ProductDTO>>(modellen);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> ProductToevoegen(AddProductDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Manuele mapping -> BAD practice, fout-en veranderinggevoelig. Veel herhalend werk
            //Product model = new Product();
            //model.Beschrijving = dto.Beschrijving;
            //model.Naam = dto.Naam;
            //model.Prijs = dto.Price;

            Product model = _mapper.Map<Product>(dto);

            await _uow.ProductRepo.Add(model);
            try
            {
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateException dbError)
            {
                return BadRequest(dbError);
            }
            return CreatedAtAction("GetProduct", new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ProductWijzigen(int id, UpdateProductDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("De opgegeven id's komen niet overeen.");
            }

            Product model = _mapper.Map<Product>(dto);
            await _uow.ProductRepo.Update(model);

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                List<Product> producten = await _uow.ProductRepo.GetAll();
                if (producten.Any(x => x.Id == id))
                {
                    return NotFound("Er is geen product met dit id gevonden");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ProductVerwijderen (int id)
        {
            List<Product> producten = await _uow.ProductRepo.GetAll();
            if (producten == null)
            {
                return NotFound("De tabel producten bestaat niet.");
            }
            Product? product = await _uow.ProductRepo.GetObject(id);
            if (product == null)
            {
                return NotFound("Het product met deze id is niet gevonden.");
            }

            await _uow.ProductRepo.Delete(product);
            await _uow.SaveChangesAsync();

            return Ok($"Product met id {id} is verwijderd");
        }
    }
}