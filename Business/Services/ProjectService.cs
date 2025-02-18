using Data.Entities;
using Data.Repositories;

namespace Business.Services;

public class ProjectService
{
    private readonly ProjectRepository _projectRepository;

    public ProjectService(ProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<ProjectEntity?> CreateProjectAsync(ProjectEntity projectEntity)
    {
        //Continous help from chatgpt-4o on the "Generating a project number "P-" for each project.
        //Here i call the SetProjectNumber method from the ProjectEntity. 
        //And set it, using the generated projectEntity's Id
        projectEntity.SetProjectNumber(GenerateProjectNumber(projectEntity.Id));
        return await _projectRepository.CreateProjectAsync(projectEntity);
    }

    public async Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync()
    {
        return await _projectRepository.GetAllProjectsAsync();
    }

    public async Task<bool> UpdateProjectAsync(ProjectEntity updatedProjectEntity)
    {
        return await _projectRepository.UpdateProjectAsync(updatedProjectEntity);
    }

    public async Task<bool> DeleteProjectAsync(int projectId)
    {
        return await _projectRepository.DeleteProjectAsync(projectId);
    }

    private string GenerateProjectNumber(int id)
    {
        return $"P-{id}";
    }
}
