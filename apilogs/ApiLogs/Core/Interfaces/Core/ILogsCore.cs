using ApiLogs.Model;
using ApiLogs.Persistence;

namespace ApiLogs.Core.Interfaces.Core
{
    public interface ILogsCore
    {
        ResponseLogs GetLogs(int pageNumber, int limit);
        int RegisterLog(LogsDTO log);
        bool UpdateLog(int id, LogsDTO logs);
        bool DeleteLog(int id);
        Logs GetLogById(int id);
    }
}
