using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Talabat.API.Errors;
using Talabat.Repository.Data;

namespace Talabat.API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext dbcontext;

        public BuggyController(StoreContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = dbcontext.Products.Find(100);
            if (product == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(product);
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var product = dbcontext.Products.Find(100);
            var productToReturn = product.ToString();      //This will throw a null reference exception

            return Ok(productToReturn);
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]     //Get: api/buggy/badrequest/five
        public ActionResult GetBadRequest(int id)   //validation error
        {
            return Ok();
        }

        [HttpGet("Unauthorized")]
        public ActionResult GetUnauthorized()
        {
            return Unauthorized(new ApiResponse(401));
        }
    }
}
