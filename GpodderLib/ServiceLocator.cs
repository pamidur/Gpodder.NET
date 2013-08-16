using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GpodderLib
{
    class ServiceLocator
    {
        private readonly IDictionary<Type, ServiceBase> _instantiatedServices;

        private bool _initialized;

        public ServiceLocator()
        {
            _instantiatedServices = new Dictionary<Type, ServiceBase>();
        }

        public T Get<T>() where T : ServiceBase
        {
            if (_instantiatedServices.ContainsKey(typeof (T)))
            {
                return (T) _instantiatedServices[typeof (T)];
            }
            
            throw new ApplicationException("The requested service is not registered");
        }

        public void RegisterService(Type contract, ServiceBase service)
        {
            if(_initialized)
                throw new Exception("Services already initialized. Connot register new one!" );

            service.ServiceLocator = this;
            _instantiatedServices.Add(contract, service);
        }

        public async Task InitServices()
        {
            if (_initialized)
                throw new Exception("Services already initialized. Connot initialize them anew!");

            //await Task.WhenAll(_instantiatedServices.Values.Select(s => s.Init()));

            foreach (var instantiatedService in _instantiatedServices.Values)
                await instantiatedService.Init();

            _initialized = true;
        }
    }
}
