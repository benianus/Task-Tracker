using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace TaskTrackerDataLayer
{
    public class TaskDTO
    {
        public TaskDTO(int taskID, string taskDescription, byte taskStatus, DateTime createdAt, DateTime updatedAt)
        {
            TaskID = taskID;
            TaskDescription = taskDescription;
            TaskStatus = taskStatus;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int TaskID { get; set; }
        public string TaskDescription { get; set; }
        public byte TaskStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class TaskTrackerData
    {
        public static string ConnectionString = "Server = .; Database = TaskTracker; User id = sa; Password = 123456; " +
            "Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";
        public static List<TaskDTO> GetAllTaskList()
        {
            List<TaskDTO> AllTasks = new List<TaskDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_GetAllTasks", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AllTasks.Add(new TaskDTO(
                                    reader.GetInt32(reader.GetOrdinal("TaskId")),
                                    reader.GetString(reader.GetOrdinal("Description")),
                                    reader.GetByte(reader.GetOrdinal("Status")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                                    ));
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return AllTasks;    
        }
        public static List<TaskDTO> GetTasksDone()
        {
            List<TaskDTO> tasksDone = new List<TaskDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_GetTasksDone", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tasksDone.Add(new TaskDTO(
                                    reader.GetInt32(reader.GetOrdinal("TaskId")),
                                    reader.GetString(reader.GetOrdinal("Description")),
                                    reader.GetByte(reader.GetOrdinal("Status")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                                    ));
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return tasksDone;
        }
        public static List<TaskDTO> GetTaskInProgress()
        {
            List<TaskDTO> tasksInProgress = new List<TaskDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_GetTasksInProgress", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tasksInProgress.Add(new TaskDTO(
                                    reader.GetInt32(reader.GetOrdinal("TaskId")),
                                    reader.GetString(reader.GetOrdinal("Description")),
                                    reader.GetByte(reader.GetOrdinal("Status")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                                    ));
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return tasksInProgress;
        }
        public static List<TaskDTO> GetTasksNotDone()
        {
            List<TaskDTO> tasksInProgress = new List<TaskDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_GetTasksNotDone", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tasksInProgress.Add(new TaskDTO(
                                    reader.GetInt32(reader.GetOrdinal("TaskId")),
                                    reader.GetString(reader.GetOrdinal("Description")),
                                    reader.GetByte(reader.GetOrdinal("Status")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                                    ));
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return tasksInProgress;
        }
        public static TaskDTO GetTaskByID(int TaskId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand("Sp_GetTaskByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TaskDTO(
                                    reader.GetInt32(reader.GetOrdinal("TaskID")),
                                    reader.GetString(reader.GetOrdinal("Description")),
                                    reader.GetByte(reader.GetOrdinal("Status")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                                );

                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }
        public static int AddNewTask(TaskDTO DTO)
        {
            int newTaskID = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_AddNewTask", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@description", DTO.TaskDescription);
                        command.Parameters.AddWithValue("@status", DTO.TaskStatus);
                        command.Parameters.AddWithValue("@createdAt", DTO.CreatedAt);
                        command.Parameters.AddWithValue("@updatedAt", DTO.UpdatedAt);

                        var outputParameter = new SqlParameter("@newTaskId", SqlDbType.Int)
                        { 
                            Direction = ParameterDirection.Output 
                        };

                        command.Parameters.Add(outputParameter);
                        
                        connection.Open();

                        command.ExecuteNonQuery();

                        connection.Close();

                        newTaskID = (int)outputParameter.Value;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return newTaskID;
        }
        //public static bool UpdateTask()
        //{

        //}
        //public static bool DeleteTask()
        //{

        //}

    }
}
