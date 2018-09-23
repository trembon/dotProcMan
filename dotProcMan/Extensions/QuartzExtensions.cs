using dotProcMan.ScheduledJobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace dotProcMan.Extensions
{
    public static class QuartzExtensions
    {
        public static async void UseQuartz(this IApplicationBuilder app)
        {
            IScheduler scheduler = app.ApplicationServices.GetService<IScheduler>();

            var jobFactory = new JobFactory(app.ApplicationServices);
            scheduler.JobFactory = jobFactory;

            await scheduler.Start();
        }

        public static void AddQuartz(this IServiceCollection services)
        {
            AddQuartz(services, null);
        }

        public static async void AddQuartz(this IServiceCollection services, Action<NameValueCollection> configuration)
        {
            var props = new NameValueCollection();

            configuration?.Invoke(props);

            ISchedulerFactory factory = new StdSchedulerFactory(props);
            IScheduler scheduler = await factory.GetScheduler();

            services.AddSingleton(scheduler);

            services.AddTransient<RestartProcessScheduledJob>();
        }

        public class JobFactory : IJobFactory
        {
            protected readonly IServiceProvider serviceProvider;

            public JobFactory(IServiceProvider serviceProvider)
            {
                this.serviceProvider = serviceProvider;
            }

            public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
            {
                return serviceProvider.GetService(bundle.JobDetail.JobType) as IJob;
            }

            public void ReturnJob(IJob job)
            {
                (job as IDisposable)?.Dispose();
            }
        }
    }
}
