using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Data.Repositories;

public class ProjectRepository(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task<ProjectEntity?> CreateProjectAsync(ProjectEntity projectEntity)
    {
        try
        {
            // Got help from ChatGPT with the "throw new Exception part", i was gonna do a if-statement checking if each of them were null
            // But i managed to get into some problems then, i believe since i was using FindAsync, and int(Id) never is null. 
            // Also not sure about if it is actually correct to but the Customer, Status, User and Product entities as public in the ProjectEntity. But when i searched around for it, it seemed to be quite normal to do that. 
            projectEntity.Customer = await _context.Customers.FindAsync(projectEntity.CustomerId) ?? throw new Exception($"Customer with ID {projectEntity.CustomerId} were not found");
            projectEntity.Status = await _context.StatusTypes.FindAsync(projectEntity.StatusTypeId) ?? throw new Exception($"Status with ID {projectEntity.StatusTypeId} were not found");
            projectEntity.User = await _context.Users.FindAsync(projectEntity.UserId) ?? throw new Exception($"User with ID {projectEntity.UserId} were not found");
            projectEntity.Product = await _context.Products.FindAsync(projectEntity.ProductId) ?? throw new Exception($"Product with ID {projectEntity.ProductId} were not found");

            await _context.Projects.AddAsync(projectEntity);
            await _context.SaveChangesAsync();
            return projectEntity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in CreateProjectAsync: {ex}");
            return null;
        }
    }

    public async Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync()
    {
        return await _context.Projects
            .Include(p => p.Customer)
            .Include(p => p.Status)
            .Include(p => p.User)
            .Include(p => p.Product)
            .ToListAsync();
    } 

    public async Task<bool> UpdateProjectAsync(ProjectEntity updatedProjectEntity)
    {
        try
        {
            var existingProjectEntity = await _context.Projects.FindAsync(updatedProjectEntity.Id);
            if (existingProjectEntity == null)
            {
                return false;
            }

            existingProjectEntity.Customer = await _context.Customers.FindAsync(updatedProjectEntity.CustomerId) ?? throw new Exception("Customer were not found");
            existingProjectEntity.Status = await _context.StatusTypes.FindAsync(updatedProjectEntity.StatusTypeId) ?? throw new Exception("Status were not found");
            existingProjectEntity.User = await _context.Users.FindAsync(updatedProjectEntity.UserId) ?? throw new Exception("User were not found");
            existingProjectEntity.Product = await _context.Products.FindAsync(updatedProjectEntity.ProductId) ?? throw new Exception("Product were not found");

            _context.Entry(existingProjectEntity).CurrentValues.SetValues(updatedProjectEntity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Errr in UpdateProjectAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        try
        {
            var projectEntity = await _context.Projects.FindAsync(id);
            if(projectEntity == null)
            {
                return false;
            }

            _context.Projects.Remove(projectEntity)
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in DeleteProjectAsync: {ex.Message}");
            return false;
        }
    }
}
