

namespace WebAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private WebAPIDemoContext _context;

        public ProductController(WebAPIDemoContext context)
        {
            _context = context;
        }

        // GET: api/<ProductController>
        [HttpGet("GetAll")]
        public ActionResult<List<Product>> Get()
        {
            var producten = _context.Producten.ToList();
            if (producten.Count > 0) {
                return Ok(producten);
            }
            else
            {
                return NotFound("Er zijn geen resultaten gevonden");
            }
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            Product product = _context.Producten.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound($"Product {id} kan niet worden gevonden in de database");
            }
            return Ok(product);            
        }

        [HttpGet("Search")]
        public ActionResult<List<Product>> Search(string zoekwaarde)
        {
            var product = _context.Producten.Where(x=>x.Naam.Contains(zoekwaarde)).OrderBy(x=>x.Naam);
            if (product == null)
            {
                return NotFound($"Er zijn geen producten in de database waar {zoekwaarde} voorkomt in de naam");
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> ProductToevoegen(Product product)
        {
            //Validatie
            if (_context.Producten == null)
                return NotFound("De tabel Producten bestaat niet in de database.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Product toevoegen aan de DbSet
            await _context.Producten.AddAsync(product);
            try
            {
                //Product wegschrijven naar de database
                _context.SaveChanges();
            }
            catch (DbUpdateException dbError) 
            {
                return BadRequest(dbError);
            }
            return CreatedAtAction("GetProduct", new {id = product.Id}, product);
        }

        [HttpPut("{id}")]
        public ActionResult ProductWijzigen(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("De opgegeven id's komen niet overeen.");
            }
            //Hier kan nog andere validatie komen.

            _context.Producten.Update(product);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Producten.Any(x => x.Id == id))
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
        public async Task<IActionResult> ProductVerwijdern(int id)
        {
            if (_context.Producten == null)
            {
                return NotFound("De tabel producten bestaat niet.");
            }
            Product product = await _context.Producten.FirstOrDefaultAsync(x=>x.Id == id);
            if (product == null)
            {
                return NotFound("Het product met deze id is niet gevonden.");
            }

            _context.Producten.Remove(product);
            _context.SaveChanges();

            return Ok($"Product met id {id} is verwijderd");
        }
    }
}
