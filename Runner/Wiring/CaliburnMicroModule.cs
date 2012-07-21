using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Ninject.Modules;
using Ninject.Extensions.Conventions;
using StaticVoid.OrmPerformance.Messaging;

namespace StaticVoid.OrmPerformance.Runner.Wiring
{
    public class CaliburnMicroModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IWindowManager>().To<WindowManager>().InSingletonScope();
            Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
            Bind<ISendMessages>().To<EventAggregatorForwarder>().InSingletonScope();

            Bind<IRunOrmTests>().To<OrmTestRunnerViewModel>().InSingletonScope();

            Kernel.Scan((x) =>
            {
                x.FromAssemblyContaining<CaliburnMicroModule>();
                x.Where(type => type.Name.EndsWith("Service"));
                x.BindWithDefaultConventions();
                x.InSingletonScope();
            });
        }
    }
}
