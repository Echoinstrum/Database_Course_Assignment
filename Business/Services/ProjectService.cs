using Business.Models;
using Data.Entities;
using Data.Repositories;
using System.Diagnostics;

namespace Business.Services;
//Got some help from ChatGPT-4o in this file. 
//Where most of the help was about the "mapping" between ProjectRegistrationForm and ProjectEntity.
// specifically ensuring that the data from the registration form is correctly sent into the entity model.
public class ProjectService
{
    private readonly ProjectRepository _projectRepository;
    private readonly CustomerService _customerService;
    public ProjectService(ProjectRepository projectRepository, CustomerService customerService)
    {
        _projectRepository = projectRepository;
        _customerService = customerService;
    }

    public async Task<ProjectModel?> CreateProjectAsync(ProjectRegistrationForm projectRegistrationForm)
    {
        try
        {
            var customerEntity = await _customerService.CreateCustomerIfNotExistAsync(projectRegistrationForm.CustomerName);

            var projectEntity = new ProjectEntity
            {
                Title = projectRegistrationForm.Title,
                Description = projectRegistrationForm.Description,
                StartDate = projectRegistrationForm.StartDate,
                EndDate = projectRegistrationForm.EndDate,
                ProjectManager = projectRegistrationForm.ProjectManager,
                TotalPrice = projectRegistrationForm.TotalPrice,
                Status = projectRegistrationForm.Status ?? "Ej påbörjat",
                Service = projectRegistrationForm.Service,
                CustomerId = customerEntity.Id
            };
            //Calls the GetNextProjectNumberAsync-method. 
            //That counts all the existing projects in the db and creates the next comming project number
            var nextProjectNumber = await _projectRepository.GetNextProjectNumberAsync();
            projectEntity.SetProjectNumber(nextProjectNumber);
            //Sets the generated projectNuber in ProjectEntity through the SetProjectNumber-method
            //Which we use because ProjectNumber has a private set.



            var createdProject = await _projectRepository.CreateProjectAsync(projectEntity);

            if (createdProject != null)
            {
                var customer = await _customerService.GetCustomerByIdAsync(createdProject.CustomerId);

                return new ProjectModel
                {
                    Id = createdProject.Id,
                    ProjectNumber = createdProject.ProjectNumber,
                    Title = createdProject.Title,
                    Description = createdProject.Description,
                    StartDate = createdProject.StartDate,
                    EndDate = createdProject.EndDate,
                    ProjectManager = createdProject.ProjectManager,
                    TotalPrice = createdProject.TotalPrice,
                    Status = createdProject.Status,
                    Service = createdProject.Service,
                    CustomerId = createdProject.CustomerId,
                    CustomerName = customer?.CustomerName ?? "Okänd kund"
                };

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in CreateProjectAsync: {ex.Message}");
            return null;
        }

        return null;
    }

    public async Task<IEnumerable<ProjectModel>> GetAllProjectsAsync()
    {
        var projects = await _projectRepository.GetAllProjectsAsync();

        return projects.Select(project => new ProjectModel
        {
            Id = project.Id,
            ProjectNumber = project.ProjectNumber,
            Title = project.Title,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            ProjectManager = project.ProjectManager,
            Service = project.Service,
            Status = project.Status,
            TotalPrice = project.TotalPrice,
            CustomerId = project.CustomerId,
            CustomerName = project.Customer?.CustomerName ?? "Okänd kund",
        });
    }

    public async Task<ProjectModel?> GetProjectByIdAsync(int projectId)
    {
        var project = await _projectRepository.GetProjectByIdAsync(projectId);
        if(project == null)
        {
            return null;
        }

        var customer = await _customerService.GetCustomerByIdAsync(project.CustomerId);

        return new ProjectModel
        {
            Id = project.Id,
            ProjectNumber = project.ProjectNumber,
            Title = project.Title,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            ProjectManager = project.ProjectManager,
            TotalPrice = project.TotalPrice,
            Status = project.Status,
            Service = project.Service,
            CustomerName = customer?.CustomerName ?? "Okänd kund"
        };
    }

    public async Task<bool> UpdateProjectAsync(ProjectModel updatedProjectModel)
    {
        var existingProject =  await _projectRepository.GetProjectByIdAsync(updatedProjectModel.Id);
        if(existingProject == null)
        {
            Debug.WriteLine("Project not found for update");
            return false;
        }

        existingProject.Title = updatedProjectModel.Title;
        existingProject.Description = updatedProjectModel.Description;
        existingProject.StartDate = updatedProjectModel.StartDate;
        existingProject.EndDate = updatedProjectModel.EndDate;
        existingProject.ProjectManager = updatedProjectModel.ProjectManager;
        existingProject.TotalPrice = updatedProjectModel.TotalPrice;
        existingProject.Status = updatedProjectModel.Status;
        existingProject.Service = updatedProjectModel.Service;

        return await _projectRepository.UpdateProjectAsync(existingProject);
    }

    public async Task<bool> DeleteProjectAsync(int projectId)
    {
        var project = await _projectRepository.GetProjectByIdAsync(projectId);
        if(project == null)
        {
            Debug.WriteLine($"Project with ID {projectId} was not found");
            return false;
        }

        return await _projectRepository.DeleteProjectAsync(projectId);
    }
}
