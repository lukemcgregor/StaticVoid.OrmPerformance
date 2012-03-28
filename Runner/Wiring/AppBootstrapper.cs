using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Ninject;

namespace StaticVoid.OrmPerfomance.Runner
{
	public class AppBootstrapper : Bootstrapper<IShell>
	{
        private IKernel _kernel;

		/// <summary>
		/// By default, we are configured to use MEF
		/// </summary>
		protected override void Configure()  
        {
            _kernel = new StandardKernel(new CaliburnMicroModule());
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
