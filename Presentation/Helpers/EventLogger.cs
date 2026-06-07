using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Presentation.Helpers
{
    internal static class EventLogger
    {
        public const string sourceName = "DVLD";
        
        
        public static void LogEventsToEventViewer(string message, 
            EventLogEntryType type)
        {
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, "Application");
            }

            EventLog.WriteEntry(sourceName, message, type);
        }
    }
}
