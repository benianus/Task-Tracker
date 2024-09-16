using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskTrackerBusinessLogic;
using TaskTrackerDataLayer;

namespace TaskTrackerApiLayer.Controllers
{
    [Route("api/TaskTracker")]
    [ApiController]
    public class TaskTrackerApi : ControllerBase
    {
        [HttpGet("allTasks", Name = "getAllTasks")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<TaskDTO>> getAllTasks()
        {
            List<TaskDTO> tasks = TaskTracker.GetAllTaskList();

            if (tasks == null)
            {
                return NotFound("Tasks Not Found");
            }

            return Ok(tasks);
        }
    }
}
