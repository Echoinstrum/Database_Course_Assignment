using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class StatusTypeRepository(DataContext context)
{
    private readonly DataContext _context = context;

    //READ

    public async Task<IEnumerable<StatusTypeEntity>> GetAllAsync()
    {
        return await _context.StatusTypes.ToListAsync();
    }

    public async Task<StatusTypeEntity?> GetByIdAsync(int id)
    {
        return await _context.StatusTypes.FindAsync(id);
    }
}
