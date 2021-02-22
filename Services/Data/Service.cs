using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotesOTG_Server.Models.Contexts;
using NotesOTG_Server.Services.Interfaces;

namespace NotesOTG_Server.Services
{
    public class Service<TEntity> : IService<TEntity> where TEntity : class
    {

        protected readonly DbSet<TEntity> entity;
        private readonly DatabaseContext context;

        protected Service(DatabaseContext context)
        {
            this.context = context;
            entity = context.Set<TEntity>();
        }


        public async Task<List<TEntity>> GetAll()
        {
            return await entity.ToListAsync();
        }

        public async Task<TEntity> FindById(int id)
        {
            return await entity.FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            this.entity.Remove(entity);
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            var e = await this.entity.AddAsync(entity);
            return e.Entity;
        }

        public async Task<bool> Save()
        {
            return await context.SaveChangesAsync() == 1;
        }
    }
}