using System.ComponentModel.DataAnnotations;
using Talabat.Core;
using Talabat.Core.Entities;

namespace Talabat.API.Attributes
{
    public class ExistingIdAttribute<T> : ValidationAttribute where T : BaseEntity
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int id)
            {
                // check if the id > 0
                if (id > 0)
                {
                    var unitOfWork = validationContext.GetService<IUnitOfWork>();
                    var entityExisiting = unitOfWork.Repository<T>().GetAsync(id).Result;

                    if (entityExisiting != null)
                        return ValidationResult.Success;
                }
            }
            return new ValidationResult($"InValid {typeof(T).Name} Id");
        }
    }
}
