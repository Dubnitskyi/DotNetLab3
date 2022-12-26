using System;
using System.Diagnostics;

namespace ProcessManager
{
    public class RunningProcess
    {
        public int Id { set; get; }
        public long RAM { set; get; }
        public DateTime StartTime { set; get; }
        public ProcessPriorityClass Priority { set; get; }
        public int Numbers { set; get; }

        public RunningProcess(int id, long amountOfRAM, DateTime processStartTime, ProcessPriorityClass processPriority, int numberOfThreads)
        {
            Id = id;
            RAM = amountOfRAM;
            StartTime = processStartTime;
            Priority = processPriority;
            Numbers = numberOfThreads;
        }
    }
}
