using Stize.Domain;
using Stize.DotNet.Specification;

namespace Stize.ApiTemplate.Domain.Specifications
{
    public class EntityByIdSpecification<T> : Specification<T>
        where T :IObject<int>
    {
        public EntityByIdSpecification(int id) : base(x => x.Id == id)
        {
        }
    }
}
