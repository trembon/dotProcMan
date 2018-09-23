using dotProcMan.Models;
using dotProcMan.ScheduledJobs;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotProcMan.Services
{
    public interface IScheduledJobSerivce
    {
        Task Initialize(IEnumerable<ManagedProcess> processes);
    }

    public class ScheduledJobSerivce : IScheduledJobSerivce
    {
        private IScheduler scheduler;

        public ScheduledJobSerivce(IScheduler scheduler)
        {
            this.scheduler = scheduler;
        }

        public async Task Initialize(IEnumerable<ManagedProcess> processes)
        {
            foreach (ManagedProcess process in processes)
            {
                if (string.IsNullOrWhiteSpace(process.RestartSchedule))
                    continue;

                IJobDetail restartProcessJob = JobBuilder.Create<RestartProcessScheduledJob>()
                    .WithIdentity(process.ID.ToString(), "default")
                    .Build();

                ITrigger restartProcessTrigger = TriggerBuilder.Create()
                    .WithIdentity(process.ID.ToString(), "default")
                    .WithCronSchedule(process.RestartSchedule)
                    .Build();

                await scheduler.ScheduleJob(restartProcessJob, restartProcessTrigger);
            }
        }
    }
}
