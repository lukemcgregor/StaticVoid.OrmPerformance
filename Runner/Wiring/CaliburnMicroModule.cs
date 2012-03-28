using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Ninject.Modules;
using Ninject.Extensions.Conventions;

namespace StaticVoid.OrmPerfomance.Runner
{
    public class CaliburnMicroModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IWindowManager>().To<WindowManager>().InSingletonScope();
            Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();

            Bind<IShell>().To<ShellViewModel>().InSingletonScope();

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
