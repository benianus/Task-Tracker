using Microsoft.AspNetCore.Mvc;
using TaskTrackerBusinessLogic;
using TaskTrackerDataLayer;

namespace TaskTrackerApiLayer.Controllers
{
    [Route("api/TaskTracker")]
    [ApiController]
    public class TaskTrackerApi : ControllerBase
    {
        [HttpGet("AllTasks", Name = "GetAllTasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<TaskDTO>>> getAllTasks()
        {
            List<TaskDTO> tasks = await TaskTracker.GetAllTaskList();

            if (tasks == null)
            {
                return NotFound("Tasks Not Found");
            }

            return Ok(tasks);
        }
        [HttpGet("TasksDone", Name = "GetTasksDone")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async  Task<ActionResult<List<TaskDTO>>> GetTasksDone()
        {
            List<TaskDTO> tasks = await TaskTracker.GetTasksDone();

            if (tasks == null)
            {
                return NotFound("Tasks not found");
            }

            return Ok(tasks);
        }
        [HttpGet("TasksInProgress", Name = "GetTasksInProgress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<TaskDTO>>> GetTaskInProgress()
        {
            List<TaskDTO> tasks = await TaskTracker.GetTaskInProgress();

            if (tasks == null)
            {
                return NotFound("Tasks not found");
            }

            return Ok(tasks);
        }
        [HttpGet("TaskNotDone", Name = "GetTasksNotDoe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<TaskDTO>>> GetTaskNotDone()
        {
            List<TaskDTO> tasks = await TaskTracker.GetTasksNotDone();

            if (tasks == null)
            {
                return NotFound("Tasks not found");
            }

            return Ok(tasks);
        }
        [HttpGet("TaskBy{taskId}", Name = "GetTaskById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskDTO>> GetTaskById(int taskId)
        {
            if (taskId < 1)
            {
                return BadRequest("Id less then 1, Bad request");
            }

            TaskTracker task = await TaskTracker.FindTaskById(taskId);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            return Ok(task.DTO);
        }
        [HttpPost("Task")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskDTO>> AddNewTask(TaskDTO newDto)
        {
            if (newDto.TaskDescription == string.Empty || newDto.TaskStatus < 0)
            {
                return BadRequest("Bad Request");
            }

            TaskTracker task = new TaskTracker(newDto);

            if (task == null)
            {
                return NotFound("Object empty");
            }

            if (await task.Save())
            {
                newDto.TaskID = task.TaskID;
                // add new id
                return CreatedAtRoute("GetTaskById", new { taskId = newDto.TaskID }, newDto);

            }

            return StatusCode(500, "Internal server errro");
        }
        [HttpPut("TaskBy{taskId}", Name = "UpdateTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskDTO>> UpdateTask(int taskId, TaskDTO Dto)
        {
            if (taskId <= 0 || Dto.TaskDescription == string.Empty || Dto.TaskStatus <= 0)
            {
                return BadRequest("Bad Request");
            }

            TaskTracker task = await TaskTracker.FindTaskById(taskId);

            if (task == null)
            {
                return NotFound("Task Not Found");
            }

            task.TaskID = taskId;
            task.TaskDescription = Dto.TaskDescription;
            task.TaskStatus = Dto.TaskStatus;
            task.CreatedAt = Dto.CreatedAt;
            task.UpdatedAt = Dto.UpdatedAt;

            if (await task.Save())
            {
                return Ok("Task Updated successfully");
            }

            return StatusCode(500, "Internal server errro");
        }
        [HttpDelete("{taskId}", Name = "DeleteTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> DeleteTask(int taskId)
        {
            if (taskId < 0)
            {
                return BadRequest($"Id {taskId} less than 1, Bad request");
            }

            TaskTracker taskDeleted = await TaskTracker.FindTaskById(taskId);

            if (taskDeleted == null)
            {
                return NotFound("Task not found");
            }

            if (await TaskTracker.DeleteTask(taskId))
            {
                return Ok("Task Deleted successfully");
            }

            return BadRequest("Bad request");
        }
        [HttpPut("markInProgress/{taskId}", Name = "MarkTaskInProgress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> MarkTaskInProgress(int taskId)
        {
            if (taskId < 0)
            {
                return BadRequest($"Id {taskId} less than 1, Bad request");
            }

            TaskTracker taskDeleted = await TaskTracker.FindTaskById(taskId);

            if (taskDeleted == null)
            {
                return NotFound("Task not found");
            }

            if (await TaskTracker.MarkTaskInProgress(taskId))
            {
                return Ok("Task marked in progress successfully");
            }

            return BadRequest("Bad request");
        }
        [HttpPut("markDone/{taskId}", Name = "MarkTaskDone")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> MarkTaskDone(int taskId)
        {
            if (taskId < 0)
            {
                return BadRequest($"Id {taskId} less than 1, Bad request");
            }

            TaskTracker taskDeleted = await TaskTracker.FindTaskById(taskId);

            if (taskDeleted == null)
            {
                return NotFound("Task not found");
            }

            if (await TaskTracker.MarkTaskDone(taskId))
            {
                return Ok("Task marked done successfully");
            }

            return BadRequest("Bad request");
        }
    }
}
