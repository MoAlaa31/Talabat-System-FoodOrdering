using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Errors;
using Talabat.Core.Entities;
using Talabat.Core;

namespace Talabat.API.Filters
{
    public class ExistingIdFilter<T> : IAsyncActionFilter where T : BaseEntity
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExistingIdFilter(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("id", out var idObj) && idObj is int id && id > 0)
            {
                var entity = await _unitOfWork.Repository<T>().GetAsync(id);
                if (entity == null)
                {
                    context.Result = new NotFoundObjectResult(new ApiResponse(StatusCodes.Status404NotFound, $"{typeof(T).Name} with ID {id} not found."));
                    return;
                }
            }
            else
            {
                context.Result = new BadRequestObjectResult(new ApiResponse(StatusCodes.Status400BadRequest, "Invalid ID format."));
                return;
            }

            await next();
        }
    }
}
