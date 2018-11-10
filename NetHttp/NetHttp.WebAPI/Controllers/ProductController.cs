using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetHttp.WebAPI.Abstractions;
using NetHttp.WebAPI.Models;

namespace NetHttp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IExtremelyDifficultCrudApi<Product, string> _clientFactory;

        public ProductController(IExtremelyDifficultCrudApi<Product, string> clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Select all products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetAll()
        {
            var allProductsRequest = await _clientFactory.GetAll();
            if (allProductsRequest.Content != null)
            {
                return Ok(allProductsRequest.Content);
            }

            return NoContent();
        }

        /// <summary>
        /// Select a single product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}", Name = "GetById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            //Avoiding invalid id to call to api
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var product = await _clientFactory.GetById(id);
            if (product != null && product.IsSuccessStatusCode)
            {
                return Ok(product.Content);
            }

            return NotFound();
        }

        /// <summary>
        /// Creates a single product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Register([FromBody] Product product)
        {
            var createdProduct = await _clientFactory.Create(product);
            return Created(nameof(GetById), createdProduct.Content);
        }

        /// <summary>
        /// Updates a single product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] Product product)
        {
            //Avoiding invalid id to call to api
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var productFromApi = await _clientFactory.Update(id, product);
            if (productFromApi != null && productFromApi.IsSuccessStatusCode)
            {
                return Accepted(productFromApi.Content);
            }

            return NotFound();
        }


        /// <summary>
        /// Removes a single product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            //Avoiding invalid id to call to api
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var product = await _clientFactory.GetById(id);
            if (product is null)
                return NotFound();

            await _clientFactory.Delete(id);
            return Ok();
        }
    }
}