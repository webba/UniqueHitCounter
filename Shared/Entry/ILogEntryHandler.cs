
using System;

namespace BlazorApp.Shared.Entry
{
    public interface ILogEntryHandler
    {
        LogEntry Handle((string cleanedLog, DateTime datetime) log, string characterName);
    }
}
