using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration.Xml;

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

        public static async Task<List<TaskDTO>> GetAllTaskList()
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

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
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
                            else
                            {
                                return null;
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
        public static async Task<List<TaskDTO>> GetTaskByStatus(int TaskStatus)
        {
            List<TaskDTO> tasks = new List<TaskDTO>();

            try
            {
                using(SqlConnection connection = new SqlConnection(ConnectionString))
                using(SqlCommand command = new SqlCommand("Sp_GetTaskByStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Status", TaskStatus);

                    connection.Open();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            tasks.Add(
                                new TaskDTO(
                                    reader.GetInt32(reader.GetOrdinal("TaskId")),
                                    reader.GetString(reader.GetOrdinal("Description")),
                                    reader.GetByte(reader.GetOrdinal("Status")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                                ));
                        }
                        else
                        {
                            return null;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return tasks;
        }
        public static async Task<TaskDTO> GetTaskByID(int TaskId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand("Sp_GetTaskByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@TaskID", TaskId);

                    connection.Open();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
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
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }
        public static async Task<int> AddNewTask(TaskDTO DTO)
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

                        await command.ExecuteNonQueryAsync();

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
        public static async Task<bool> UpdateTask(TaskDTO DTO)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_UpdateTask", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@TaskId", DTO.TaskID);
                        command.Parameters.AddWithValue("@Description", DTO.TaskDescription);
                        command.Parameters.AddWithValue("@Status", DTO.TaskStatus);
                        command.Parameters.AddWithValue("@CreatedAt", DTO.CreatedAt);
                        command.Parameters.AddWithValue("@UpdatedAt", DTO.UpdatedAt);

                        connection.Open();

                        object? result = await command.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int returnValue))
                        {
                            rowsAffected = Convert.ToInt32(result);
                        }

                        connection.Close();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return rowsAffected > 0;
        }
        public static async Task<bool> DeleteTask(int TaskId)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_DeleteTask", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        command.Parameters.AddWithValue("@TaskId", TaskId);

                        connection.Open();

                        object? result = await command.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int returnValue))
                        {
                            rowsAffected = Convert.ToInt32(result);
                        }

                        connection.Close();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return rowsAffected > 0;
        }
        public static async Task<bool> MarkTaskInProgress(int taskId)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_MarkTaskAsInProgress", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@TaskId", taskId);

                        connection.Open();

                        rowsAffected = await command.ExecuteNonQueryAsync();

                        connection.Close();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return rowsAffected > 0;
        }
        public static async Task<bool> MarkTaskDone(int taskId)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_MarkTaskDone", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@TaskId", taskId);

                        connection.Open();

                        rowsAffected = await command.ExecuteNonQueryAsync();

                        connection.Close();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return rowsAffected > 0;
        }
    }
}
