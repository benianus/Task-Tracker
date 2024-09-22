using System;
using TaskTrackerDataLayer;

namespace TaskTrackerBusinessLogic
{
    public class TaskTracker
    {
        
        public enum enMode
        {
            AddNew,
            Update
        }
        public enum enTaskStatus
        {
            NotDone = 1,
            InProgress = 2,
            Done = 3
        }
        
        public enTaskStatus TStatus = enTaskStatus.InProgress;
        public enMode Mode = enMode.AddNew;
        public TaskDTO DTO { get { return new TaskDTO(this.TaskID, this.TaskDescription, this.TaskStatus, this.CreatedAt, this.UpdatedAt); } }
        public int TaskID { get; set; }
        public string TaskDescription { get; set; }
        public byte TaskStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public TaskTracker()
        {
            TaskID = 0;
            TaskDescription = string.Empty;
            TaskStatus = 1;
            CreatedAt = DateTime.MinValue;
            UpdatedAt = DateTime.MinValue;  
        }

        public TaskTracker(TaskDTO DTO, enMode mode = enMode.AddNew)
        {
            TaskID = DTO.TaskID;
            TaskDescription = DTO.TaskDescription;
            TaskStatus = DTO.TaskStatus;
            CreatedAt = DTO.CreatedAt;
            UpdatedAt = DTO.UpdatedAt;

            Mode = mode;
        }
        public static async Task<List<TaskDTO>> GetAllTaskList()
        {
            return await TaskTrackerData.GetAllTaskList();
        }
        public static async Task<List<TaskDTO>> GetTasksDone()
        {
            return await TaskTrackerData.GetTaskByStatus((int)enTaskStatus.Done);
        }
        public static async Task<List<TaskDTO>> GetTaskInProgress()
        {
            return await TaskTrackerData.GetTaskByStatus((int)enTaskStatus.InProgress);
        }
        public static async Task<List<TaskDTO>> GetTasksNotDone()
        {
            return await TaskTrackerData.GetTaskByStatus((int)enTaskStatus.NotDone);
        }
        public static async Task<TaskTracker> FindTaskById(int TaskId)
        {
            TaskDTO DTO = await TaskTrackerData.GetTaskByID(TaskId);

            if (DTO != null)
            {
                return new TaskTracker(DTO, enMode.Update);
            }
            else
            {
                return null;
            }
        }
        private async Task<bool> _AddNewTask()
        {
            this.TaskID = await TaskTrackerData.AddNewTask(this.DTO);
            return TaskID != 0;
        }
        private async Task<bool> _UpdateTask()
        {
            return await TaskTrackerData.UpdateTask(this.DTO);
        }
        public static async Task<bool> DeleteTask(int TaskId)
        {
            return await TaskTrackerData.DeleteTask(TaskId);
        }
        public static async Task<bool> MarkTaskInProgress(int taskId)
        {
            return await TaskTrackerData.MarkTaskInProgress(taskId);
        }
        public static async Task<bool> MarkTaskDone(int taskId)
        {
            return await TaskTrackerData.MarkTaskDone(taskId);
        }
        public async Task<bool> Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    Mode = enMode.Update;
                    return await _AddNewTask();
                case enMode.Update:
                    return await _UpdateTask();
            }

            return false;
        }
    }
}
