using ApiLogs.Core.Interfaces.Persistence;
using ApiLogs.Model;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace ApiLogs.Persistence
{
    public class LogsDataBase : ILogsDataBase
    {
        private readonly IConfiguration _configuration;
        private MySqlConnection conection = null;        

        public LogsDataBase(IConfiguration configuration) 
        {
            _configuration = configuration;
            conection = new MySqlConnection(_configuration.GetSection("ConnectionStrings:ConnectionMysql").Value);
        }
        /// <summary>
        /// Método que permite eliminar un log
        /// en caso de que sea necesario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteLog(int id)
        {
            int response = 0;
            StringBuilder queryString = new StringBuilder();

            queryString.AppendLine($"DELETE FROM logs WHERE Id = {id}");


            if (!(conection.State == ConnectionState.Open))
            {
                conection.Open();
            }

            using (MySqlCommand command = new MySqlCommand(queryString.ToString(), conection))
            {
                response = command.ExecuteNonQuery();
            }

            conection.Close();
            return response > 0;
        }

        /// <summary>
        /// Método encargado de devolver un log especifico mediante su ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Logs GetLogById(int id)
        {
            Logs log = null;
            StringBuilder queryString = new StringBuilder();

            queryString.AppendLine("SELECT Id, Application, LogType, Module, Timestamp, Summary, Description");
            queryString.AppendLine($"FROM logs WHERE Id = {id}");

            if (!(conection.State == ConnectionState.Open))
            {
                conection.Open();
            }

            using (MySqlCommand command = new MySqlCommand(queryString.ToString(), conection))
            {
                using (IDataReader reader = command.ExecuteReader())
                { 
                    while (reader.Read()) 
                    {
                        log = new Logs();
                        log.Id = reader.GetInt32(0);
                        log.Application = reader.GetString(1);
                        log.LogType = reader.GetString(2);
                        log.Module = reader.GetString(3);
                        log.Timestamp = reader.GetDateTime(4);
                        log.Summary = reader.GetString(5);
                        log.Description = reader.GetString(6);
                    }
                }
            }

            conection.Close();

            return log;
        }

        /// <summary>
        /// Método que permite obtener todos los logs
        /// paginados
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ResponseLogs GetLogs(int pageNumber, int limit)
        {
            int totalRegisters = 0;
            List<Logs> lstLogs = new List<Logs>();
            Logs log = null;
            StringBuilder queryString = new StringBuilder();

            queryString.AppendLine("SELECT Id, Application, LogType, Module, Timestamp, Summary, Description");
            queryString.AppendLine($"FROM logs LIMIT {limit} OFFSET {(pageNumber - 1) * limit}");

            if (!(conection.State == ConnectionState.Open)) 
            { 
                conection.Open();
            }

            using (MySqlCommand command = new MySqlCommand(queryString.ToString(), conection))
            {
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        log = new Logs();
                        log.Id = reader.GetInt32(0);
                        log.Application = reader.GetString(1);
                        log.LogType = reader.GetString(2);
                        log.Module = reader.GetString(3);
                        log.Timestamp = reader.GetDateTime(4);
                        log.Summary = reader.GetString(5);
                        log.Description = reader.GetString(6);

                        lstLogs.Add(log);
                    }
                }
            }

            queryString.Clear();
            queryString.AppendLine("SELECT COUNT(*) FROM logs");

            using (MySqlCommand command = new MySqlCommand(queryString.ToString(), conection))
            {
                totalRegisters = int.Parse(command.ExecuteScalar().ToString());
            }

            conection.Close();

            ResponseLogs responseLogs = new ResponseLogs()
            { 
                TotalRegsiters = totalRegisters,
                LstLogs = lstLogs
            };

            return responseLogs;
        }

        /// <summary>
        /// Mètodo  que permite insertar un log en la
        /// base de datos
        /// </summary>
        /// <param name="logs"></param>
        /// <returns></returns>
        public int RegisterLog(LogsDTO logs)
        {
            int response = 0;
            StringBuilder queryString = new StringBuilder();
            queryString.AppendLine("INSERT INTO logs (Application, LogType, Module, Timestamp, Summary, Description)");
            queryString.AppendLine($"VALUES ('{logs.Application}', '{logs.LogType}', '{logs.Module}', '{logs.Timestamp.ToString("yyyy-MM-dd")}',");
            queryString.AppendLine($"'{logs.Summary}', '{logs.Description}')");

            if (!(conection.State == ConnectionState.Open))
            {
                conection.Open();
            }

            using (MySqlCommand command = new MySqlCommand(queryString.ToString(), conection))
            {
                response = command.ExecuteNonQuery();
            }

            conection.Close();

            return response;
        }

        /// <summary>
        /// Método encargado de actualizar un log mediante su ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="logs"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool UpdateLog(int id, LogsDTO logs)
        {
            int response = 0;
            StringBuilder queryString = new StringBuilder();
            queryString.AppendLine($"UPDATE logs SET Application = '{logs.Application}', LogType = '{logs.LogType}',");
            queryString.AppendLine($"Module = '{logs.Module}', Timestamp = '{logs.Timestamp.ToString("yyyy-MM-dd")}', Summary = '{logs.Summary}',");
            queryString.AppendLine($"Description = '{logs.Description}'");
            queryString.AppendLine($"WHERE Id = {id}");

            if (!(conection.State == ConnectionState.Open))
            {
                conection.Open();
            }


            using (MySqlCommand command = new MySqlCommand(queryString.ToString(), conection))
            {
                response = command.ExecuteNonQuery();
            }

            conection.Close();

            return response > 0;
        }
    }
}
