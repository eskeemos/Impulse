using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impulse.Helpers
{
    public class JobFactory : IJobFactory
    {
        protected readonly IServiceScope scope;
        public JobFactory(IServiceProvider container)
        {
            scope = container.CreateScope();
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var res = scope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
            return res;
        }
        public void ReturnJob(IJob job)
        {
            (job as IDisposable)?.Dispose();
        }
        public void Dispose()
        {
            scope.Dispose();
        }
    }

}
