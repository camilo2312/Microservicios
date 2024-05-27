using ApiLogs.Model;

namespace ApiLogs.Core.Interfaces.Persistence
{
    public interface ILogsDataBase
    {
        ResponseLogs GetLogs(int pageNumber, int limit);
        int RegisterLog(LogsDTO log);
        bool UpdateLog(int id, LogsDTO logs);
        bool DeleteLog(int id);
        Logs GetLogById(int id);
    }
}
