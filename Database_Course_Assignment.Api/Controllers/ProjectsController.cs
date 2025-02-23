using Business.Models;
using Business.Services;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Database_Course_Assignment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectsController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectModel>> CreateProject(ProjectRegistrationForm projectRegistrationForm)
        {
            var newProject = await _projectService.CreateProjectAsync(projectRegistrationForm);
            if(newProject == null)
            {
                return BadRequest("Error in HttpPost: Could not create project - please check input data.");
            }
            return CreatedAtAction(
                nameof(GetProjectById),
                new { id = newProject.Id },
                newProject);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectModel>>> GetProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            // Had some help from ChatGPT-4o with this line || projects.Any() - What we do here is
            // checking if projects is null(not existing) or if the list is empty(exists, but nothing is in it)
            if (projects == null || !projects.Any()) 
            {
                return Ok(new List<ProjectModel>());
            }
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProjectById(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if(project == null)
            {
                return NotFound($"Project with ID: {id} were not found");
            }
            return Ok(project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, ProjectModel projectModel)
        {
            if (id != projectModel.Id)
            {
                return BadRequest("Project id not matching");
            }

            var updatedProject = await _projectService.UpdateProjectAsync(projectModel);
            if (!updatedProject)
            {
                return BadRequest("Project wasn't found");
            }

            return Ok(updatedProject);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var deletedProject = await _projectService.DeleteProjectAsync(id);
            if (!deletedProject)
            {
                return BadRequest("Project wasn't found");
            }

            return Ok(deletedProject);
        }
    }
}
