using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext dbcontext;
        //private Dictionary<string, GenericRepository<BaseEntity>> repositories;
        private Hashtable repositories;
        //hashtable(non generic collection) can't be used in one application in which boxing and unboxing takes place
        //same as dictionary but it is not generic
        public UnitOfWork(StoreContext dbcontext)
        {
            this.dbcontext = dbcontext;
            repositories = new Hashtable();

        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;

            if (!repositories.ContainsKey(key))
            {
                var repository = new GenericRepository<TEntity>(dbcontext);

                repositories.Add(key, repository);
            }

            return repositories[key] as IGenericRepository<TEntity>;
        }

        //it is used to create repositories for each entity

        public async Task<int> CompleteAsync()
            => await dbcontext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await dbcontext.DisposeAsync();

    }
}
