using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Data.Repositories;


//My ambition was to finnish up more than just the ProjectEntity and CustomerEntity. And actually use the Status, User and Product/Service entities and so on.
//But i won't make it enough in time, that's why the Status, User and Product parts are commented out here and there.
public class ProjectRepository(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task<ProjectEntity?> CreateProjectAsync(ProjectEntity projectEntity)
    {
        try
        {
            // Got help from ChatGPT with the "throw new Exception part" And what it does is 
            // Get a customer from the database based on the CustomerId. And since Id is a primaryKey, it should never be null.
            // But throwing an exception incanse a Customer couldn't be found. 
            projectEntity.Customer = await _context.Customers.FindAsync(projectEntity.CustomerId) ?? throw new Exception($"Customer with ID {projectEntity.CustomerId} were not found");

            //projectEntity.Status = await _context.StatusTypes.FindAsync(projectEntity.StatusTypeId) ?? throw new Exception($"Status with ID {projectEntity.StatusTypeId} were not found");
            //projectEntity.User = await _context.Users.FindAsync(projectEntity.UserId) ?? throw new Exception($"User with ID {projectEntity.UserId} were not found");
            //projectEntity.Product = await _context.Products.FindAsync(projectEntity.ProductId) ?? throw new Exception($"Product with ID {projectEntity.ProductId} were not found");

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

    // Got some help from ChatGPT-4o on the .Include here. Eeven though Lazy loading isn't "active" as default. 
    // Adding include here is to prevent Lazy Loading issues if it would get active.
    public async Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync()
    {
        return await _context.Projects
            .Include(p => p.Customer) // Preventing LazyLoading, ensuring the customer is loaded.
            //.Include(p => p.Status)
            //.Include(p => p.User)
            //.Include(p => p.Product)
            .ToListAsync();
    } 


    public async Task<ProjectEntity?> GetProjectByIdAsync(int projectId)
    {
        return await _context.Projects.FindAsync(projectId);
    }


    public async Task<bool> UpdateProjectAsync(ProjectEntity updatedProjectEntity)
    {
        try
        {
            var existingProjectEntity = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == updatedProjectEntity.Id);

            if (existingProjectEntity == null)
            {
                Debug.WriteLine($"Project with ID {updatedProjectEntity.Id} was not found.");
                return false;
            }

            //Got a little help from ChatGPT-4o here. What it does is -
            //Keeping the old customerId and forgets about the incomming value
            updatedProjectEntity.CustomerId = existingProjectEntity.CustomerId;

            _context.Projects.Update(updatedProjectEntity);
            await _context.SaveChangesAsync();
            Debug.WriteLine("Database updated successfully!");
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in UpdateProjectAsync: {ex.Message}");
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

            _context.Projects.Remove(projectEntity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in DeleteProjectAsync: {ex.Message}");
            return false;
        }
    }

    // Got some help from ChatGPT-4o for this one. Helping me solve the issue i had of generating a ProjectNumber starting at "P-100"
    // So, what this does is it counts all the Projects in the Db. Return the "P-StartingProjectNumber(which is set too 100) + projectCount" so if there are 2 projects, it return 102 and so on.
    // This is then used in the ProjectSErvice

    private const int StartingProjectNumber = 100;
    public async Task<string> GetNextProjectNumberAsync()
    {
        int projectCount = await _context.Projects.CountAsync();
        return $"P-{StartingProjectNumber + projectCount}";
    }
}
