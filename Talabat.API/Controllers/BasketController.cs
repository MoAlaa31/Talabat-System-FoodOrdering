using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Dtos;
using Talabat.API.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(
            IBasketRepository basketRepository,
            IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedbasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var createdOrUpdated = await _basketRepository.UpdateBasketAsync(mappedbasket);
            if (createdOrUpdated is null)
                return BadRequest(new ApiResponse(400));
            return Ok(createdOrUpdated);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
            return Ok(new ApiResponse(200, "Basket Deleted Successfully"));
        }
    }
}
