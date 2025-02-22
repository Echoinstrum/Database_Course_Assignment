//using Data.Contexts;
//using Data.Entities;
//using Microsoft.EntityFrameworkCore;
//using System.Diagnostics;
//using System.Reflection.Metadata;

//namespace Data.Repositories;

//public class UserRepository(DataContext context)
//{
//    private readonly DataContext _context = context;

//    // CREATE
//    public async Task<UserEntity> CreateAsync(UserEntity userEntity)
//    {
//        try
//        {
//            await _context.Users.AddAsync(userEntity);
//            await _context.SaveChangesAsync();
//            return userEntity;
//        }
//        catch (Exception ex)
//        {
//            Debug.WriteLine($"Error in CreateAsync: {ex}");
//            return null!;
//        }
//    }

//    // READ

//    public async Task<IEnumerable<UserEntity>> GetAllAsync()
//    {
//        return await _context.Users.ToListAsync();
//    }

//    // UPDATE

//    public async Task<bool> UpdateAsync(UserEntity updateUserEntity)
//    {
//        if(updateUserEntity == null)
//        {
//            return false!;
//        }
//        try
//        {
//            var existingUser = await _context.Users.FindAsync(updateUserEntity.Id);
//            if (existingUser == null)
//            {
//                return false;
//            }
//            _context.Entry(existingUser).CurrentValues.SetValues(updateUserEntity);
//            await _context.SaveChangesAsync();
//            return true;
//        }
//        catch(Exception ex)
//        {
//            Debug.WriteLine($"Error in UpdateAsync: {ex}");
//            return false;
//        }

//    }

//    //DELETE
//    public async Task<bool> DeleteAsync(int id)
//    {
//        try
//        {
//            var userEntity = await _context.Users.FindAsync(id);
//            if(userEntity == null)
//            {
//                return false;
//            }
//            _context.Users.Remove(userEntity);
//            await _context.SaveChangesAsync();
//            return true;
//        }
//        catch (Exception ex)
//        {
//            Debug.WriteLine($"Error in DeleteAsync: {ex}");
//            return false;
//        }
//    }
//}
