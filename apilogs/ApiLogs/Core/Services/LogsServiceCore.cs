using ApiLogs.Core.Interfaces.Core;
using ApiLogs.Core.Interfaces.Persistence;
using ApiLogs.Model;

namespace ApiLogs.Core.Services
{
    public class LogsServiceCore : ILogsCore
    {
        private readonly ILogsDataBase _logsDataBase;

        public LogsServiceCore(ILogsDataBase logsDataBase)
        {
            _logsDataBase = logsDataBase;
        }
        public bool DeleteLog(int id)
        {
            try
            {
                return _logsDataBase.DeleteLog(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Logs GetLogById(int id)
        {
            try
            {
                return _logsDataBase.GetLogById(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ResponseLogs GetLogs(int pageNumber, int limit)
        {
            try
            {
                return _logsDataBase.GetLogs(pageNumber, limit);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int RegisterLog(LogsDTO log)
        {
            try
            {
                return _logsDataBase.RegisterLog(log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool UpdateLog(int id, LogsDTO log)
        {
            try
            {
                return _logsDataBase.UpdateLog(id, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
