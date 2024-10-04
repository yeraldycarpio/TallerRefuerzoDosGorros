using Microsoft.EntityFrameworkCore;
using TallerRefuerzoDosGorros.Models.EN;

namespace TallerRefuerzoDosGorros.Models.DAL
{
    public class GorrosDAL
    {
        readonly GDbContext _context;

        public GorrosDAL(GDbContext gcontext)
        {
            _context = gcontext;
        }

        //Crear
        public async Task<int> Create(Gorro gorros)
        {
            _context.Add(gorros);
            return await _context.SaveChangesAsync();
        }

        //Obtener por ID
        public async Task<Gorro> GetById(int id)
        {
            var gorro = await _context.Gorros.FirstOrDefaultAsync(s => s.Id == id);
            return gorro != null ? gorro : new Gorro();
        }

        //Editar
        public async Task<int> Edit(Gorro gorros)
        {
            int result = 0;
            var gorroUpdate = await GetById(gorros.Id);
            if (gorroUpdate.Id != 0)
            {
                gorroUpdate.nombre = gorros.nombre;
                gorroUpdate.color = gorros.color;
                gorroUpdate.material = gorros.material;
                gorroUpdate.precio = gorros.precio;
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        //Eliminar
        public async Task<int> Delete(int id)
        {
            int result = 0;
            var gorroDelete = await GetById(id);
            if (gorroDelete.Id != 0)
            {
                _context.Gorros.Remove(gorroDelete);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        //Buscar productos con filtros
        private IQueryable<Gorro> Query(Gorro gorros)
        {
            var query = _context.Gorros.AsQueryable();
            if (!string.IsNullOrWhiteSpace(gorros.nombre))
                query = query.Where(s => s.nombre.Contains(gorros.nombre));
            return query;
        }

        //Conteo de resultados
        public async Task<int> CountSearch(Gorro gorros)
        {
            return await Query(gorros).CountAsync();
        }

        public async Task<List<Gorro>> Search(Gorro gorros, int take = 10, int skip = 0)
        {
            take = take == 0 ? 10 : take;
            var query = Query(gorros);
            query = query.OrderByDescending(s => s.Id).Skip(skip).Take(take);
            return await query.ToListAsync();
        }
    }
}
