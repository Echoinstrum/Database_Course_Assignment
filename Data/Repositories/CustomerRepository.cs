using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Data.Repositories;

public class CustomerRepository(DataContext context)
{
    private readonly DataContext _context = context;

    // CREATE
    public async Task<CustomerEntity> CreateAsync (CustomerEntity customerEntity)
    {
        try
        {
            await _context.Customers.AddAsync(customerEntity);
            await _context.SaveChangesAsync();
            return customerEntity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in CreateAsync: {ex}");
            return null!;
        }
    }

    // READ
    public async Task<IEnumerable<CustomerEntity>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    // UPDATE
    public async Task<bool> UpdateAsync(CustomerEntity updatedCustomerEntity)
    {
        if(updatedCustomerEntity == null)
        {
            return false!;
        }

        try
        {
            var existingCustomer = await _context.Customers.FindAsync(updatedCustomerEntity.Id);
            if(existingCustomer == null)
            {
                return false;
            }
            _context.Entry(existingCustomer).CurrentValues.SetValues(updatedCustomerEntity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in UpdateAsync: {ex}");
            return false;
        }
    }

    // DELETE
    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var customerEntity = await _context.Customers.FindAsync(id);
            if (customerEntity == null)
            {
                return false;
            }
            _context.Customers.Remove(customerEntity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in DeleteAsync: {ex}");
            return false;
        }
    }
}
