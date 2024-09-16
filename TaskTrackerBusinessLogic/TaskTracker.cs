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
        public static List<TaskDTO> GetAllTaskList()
        {
            return TaskTrackerData.GetAllTaskList();
        }
        public static List<TaskDTO> GetTasksDone()
        {
            return TaskTrackerData.GetTasksDone();
        }
        public static List<TaskDTO> GetTaskInProgress()
        {
            return TaskTrackerData.GetTaskInProgress();
        }
        public static List<TaskDTO> GetTasksNotDone()
        {
            return TaskTrackerData.GetTasksNotDone();
        }
        public static TaskTracker FindTaskById(int TaskId)
        {
            TaskDTO DTO = TaskTrackerData.GetTaskByID(TaskId);

            if (DTO != null)
            {
                return new TaskTracker(DTO, enMode.Update);
            }
            else
            {
                return null;
            }
        }
        private bool _AddNewTask()
        {
            this.TaskID = TaskTrackerData.AddNewTask(this.DTO);
            return TaskID != 0;
        }
        private bool _UpdateTask()
        {
            return TaskTrackerData.UpdateTask(this.TaskID, this.TaskDescription, this.TaskStatus, this.CreatedAt, this.UpdatedAt);
        }
        //public static bool DeleteTask()
        //{
        //    return TaskTrackerData.DeleteTask();
        //}
        //public bool Save()
        //{
        //    switch(Mode)
        //    {
        //        case enMode.AddNew:
        //            Mode = enMode.Update;
        //            return _AddNewTask();
        //        case enMode.Update:
        //            return _UpdateTask();
        //    }

        //    return false;
        //}
    }
}
