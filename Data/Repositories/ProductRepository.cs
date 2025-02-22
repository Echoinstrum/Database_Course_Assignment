//using Data.Contexts;
//using Data.Entities;
//using Microsoft.EntityFrameworkCore;
//using System.Diagnostics;

//namespace Data.Repositories;

//public class ProductRepository(DataContext context)
//{
//    private readonly DataContext _context = context;

//    //CREATE
//    public async Task<ProductEntity> CreateProductAsync(ProductEntity entity)
//    {
//        try
//        {
//            await _context.Products.AddAsync(entity);
//            await _context.SaveChangesAsync();
//            return entity;
//        }
//        catch (Exception ex)
//        {
//            Debug.WriteLine(ex.Message);
//            return null!;
//        }
//    }


//    //READ
//     public async Task<IEnumerable<ProductEntity>> GetAllAsync()
//    {
//        return await _context.Products.ToListAsync();
//    }


//    //UPDATE

//    public async Task<bool> UpdateAsync(ProductEntity updatedEntity)
//    {
//        if( updatedEntity == null)
//        {
//            return false!;
//        }

//        try
//        {
//            var existingEntity = await _context.Products.FindAsync(updatedEntity.Id);
//            if (existingEntity == null)
//            {
//                return false;
//            }
//            _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
//            await _context.SaveChangesAsync();
//            return true;
//        }
//        catch (Exception ex)
//        {
//            Debug.WriteLine(ex.Message);
//            return false;
//        }  
//    }

//    //DELETE product by id

//    public async Task<bool> DeleteByIdAsync(int id)
//    {
//            try
//            {
//                var existingEntity = await _context.Products.FindAsync(id);
//                if(existingEntity == null)
//                {
//                    return false;
//                }
//                _context.Products.Remove(existingEntity);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch(Exception ex)
//            {
//                Debug.WriteLine(ex.Message);
//                return false;
//            }
//    }
//}
