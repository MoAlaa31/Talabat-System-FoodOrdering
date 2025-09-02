using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public interface ISpecification<T> where T : BaseEntity
    {
        //interface specifies the specification object sent to the method that builds the query
        //property signature
        public Expression<Func<T, bool>>? Criteria { get; set; }     //P=>P.Id==1
        //the variable that caries the value for the "Where"
        public List<Expression<Func<T, object>>> Includes { get; set; }    //P=>P.Brand
        //the variable that caries the value for the "Include"     

        public Expression<Func<T, object>>? OrderBy { get; set; }
        public Expression<Func<T, object>>? OrderByDescending { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }
    }
}
