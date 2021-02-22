using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace NotesOTG_Server.Services.Interfaces
{
    public interface IService<TEntity>
    {
        Task<List<TEntity>> GetAll();

        Task<TEntity> FindById(int id);

        void Remove(TEntity entity);

        Task<TEntity> Add(TEntity entity);

        Task<bool> Save();
        
    }
}