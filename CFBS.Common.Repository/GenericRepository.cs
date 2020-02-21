using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CFBS.Common.Repository
{
    public class GenericRepository<TContext, TEntity, TDTO> : IGenericRepository<TEntity, TDTO> 
                                                            where TContext : DbContext 
                                                            where TEntity : Entity
                                                            where TDTO : class
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly IMapper _mapper;

        public GenericRepository(TContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _mapper = mapper;
        }

        public async Task<ICollection<TDTO>> Get(Expression<Func<TEntity, bool>> filter = null,
                                                 string includeProperties = "")
        {
            IQueryable<TEntity> entities = _dbSet;

            if (filter != null)
            {
                entities = entities.Where(filter);
            }

            foreach (string includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                entities = entities.Include(includeProperty);
            }

            IQueryable<TDTO> dtos = _mapper.ProjectTo<TDTO>(entities);

            return await dtos.ToListAsync();
        }

        public async Task<TDTO> GetByID(int id)
        {
            TEntity foundEntity = await _dbSet.FindAsync(id);
            return _mapper.Map<TDTO>(foundEntity);
        }

        public async Task<TDTO> Create([NotNull]TDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            TEntity entity = _mapper.Map<TEntity>(dto);

            entity.ID = null;

            await _dbSet.AddAsync(entity);

            await _context.SaveChangesAsync();

            IEnumerable<NavigationEntry> navigationEntries = _context.Entry(entity).Navigations;

            foreach (NavigationEntry navigationEntry in navigationEntries)
            {
                if (!navigationEntry.IsLoaded) await navigationEntry.LoadAsync();
            }

            return _mapper.Map<TDTO>(entity);
        }

        public async Task Delete(int id)
        {
            TEntity entityToDelete = await _dbSet.FindAsync(id);

            if (entityToDelete == null) throw new ArgumentException(nameof(id));

            _context.Update(entityToDelete);

            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, TDTO dto)
        {
            TEntity entityToUpdate = await _dbSet.FindAsync(id);

            if (entityToUpdate == null) throw new ArgumentException(nameof(id));

            _mapper.Map(dto, entityToUpdate);

            _context.Update(entityToUpdate);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> EntityExists(int id)
        {
            return await _dbSet.AsNoTracking().AnyAsync(entity => entity.ID == id);
        }
    }
}
