using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;

namespace Impulse.Helpers
{
    public class JobFactory : IJobFactory
    {
        #region Var

        protected readonly IServiceScope scope;

        #endregion

        #region Ctor

        public JobFactory(IServiceProvider container)
        {
            scope = container.CreateScope();
        }

        #endregion

        #region Public
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            /* return service of specific type */
            return scope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            // Allow to JobFactory to release unmanaged resources 
            (job as IDisposable)?.Dispose();
        }

        public void Dispose()
        {
            // Releases resources 
            scope.Dispose();
        }

        #endregion
    }
}
