using System;
using System.Collections.Generic;

namespace GpodderLib
{
    internal class ServiceLocator
    {
        private static readonly Lazy<ServiceLocator> _serviceLocatorInializer = new Lazy<ServiceLocator>(()=>new ServiceLocator()); 
        public static ServiceLocator Instance
        {
            get { return _serviceLocatorInializer.Value; }
        }

        private readonly IDictionary<Type, object> _instantiatedServices;

        internal ServiceLocator()
        {
            _instantiatedServices = new Dictionary<Type, object>();
        }

        public T GetService<T>()
        {
            if (_instantiatedServices.ContainsKey(typeof (T)))
            {
                return (T) _instantiatedServices[typeof (T)];
            }
            
            throw new ApplicationException("The requested service is not registered");
        }

        public void RegisterService(Type contract, object service)
        {
            _instantiatedServices.Add(contract, service);
        }
    }
}
