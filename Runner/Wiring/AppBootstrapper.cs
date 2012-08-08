using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Ninject;
using StaticVoid.OrmPerformance.Harness;
using StaticVoid.OrmPerformance.Runner.Wiring;

namespace StaticVoid.OrmPerformance.Runner
{
	public class AppBootstrapper : Bootstrapper<IRunOrmTests>
	{
        private IKernel _kernel;

		protected override void Configure()  
        {
            _kernel = new StandardKernel(new CaliburnMicroModule(), new OrmConfigurationModule(), new HarnessModule());
            _kernel.Rebind<ISampleSizeStep>().To<PlusOneSampleSizeStep>();
        }

		protected override object GetInstance(Type serviceType, string key)
		{
            if (serviceType != null)
            {
                return _kernel.Get(serviceType);
            }

            throw new ArgumentNullException("serviceType");
		}

		protected override IEnumerable<object> GetAllInstances(Type serviceType)
		{
            if (serviceType != null)
            {
                return _kernel.GetAll(serviceType);
            }

            throw new ArgumentNullException("serviceType");
		}

		protected override void BuildUp(object instance)
		{
            _kernel.Inject(instance);
		}
	}
}
