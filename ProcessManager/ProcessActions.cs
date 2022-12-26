using System.Collections.Generic;
using System.Diagnostics;

namespace ProcessManager
{
    public static class ProcessActions
    {
        public static List<RunningProcess> procList = new List<RunningProcess>();

        public static void StartProcess(string Path)
        {
            Process process = Process.Start(Path);
            RunningProcess newProcess = new RunningProcess(process.Id, process.PagedMemorySize64, process.StartTime, process.PriorityClass, process.Threads.Count);
            procList.Add(newProcess);
        }
        public static void KillProcessFromList(int processId)
        {
            for (int i = 0; i < procList.Count; i++)
            {
                if (Equals(procList[i].Id, processId))
                {
                    procList.Remove(procList[i]);
                }
            }
        }
        public static bool KillProcess(int processId)
        {
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                if (process.Id == processId)
                {
                    KillProcessFromList(processId);
                    process.Kill();
                    return true;
                }
            }
            return false;
        }
        public static bool AllowActionForProcess(int processId)
        {
            for (int i = 0; i < procList.Count; i++)
                if (Equals(procList[i].Id, processId))
                    return true;
            return false;
        }
        public static void ChangeProcessPriority(int processId, ProcessPriorityClass priority)
        {
            for (int i = 0; i < procList.Count; i++)
                if (Equals(procList[i].Id, processId))
                    procList[i].Priority = priority;

            var processes = Process.GetProcesses();
            foreach (var process in processes)
                if (process.Id == processId)
                    process.PriorityClass = priority;
        }

        
    }
}
