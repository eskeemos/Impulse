using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;

namespace Impulse.Helpers
{
    public class JobFactory : IJobFactory
    {
        #region Variables

        protected readonly IServiceScope scope;

        #endregion

        #region Constructor

        /// <summary>
        /// Set up variables
        /// </summary>
        /// <param name="_scope">Job scope</param>
        public JobFactory(IServiceProvider container)
        {
            this.scope = container.CreateScope();
        }

        #endregion

        #region Implemented functions

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return scope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            (job as IDisposable)?.Dispose();
        }

        #endregion

        #region Public

        public void Dispose()
        {
            scope.Dispose();
        }

        #endregion
    }
}
