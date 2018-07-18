using System;
using System.Collections.Generic;
using System.Text;

namespace ScanX.Core
{
    public class ServiceBase<TService> where TService : class, new()
    {
        protected static TService _instance;
        
        public static TService Instance { get { return _instance; } }

        protected ServiceBase()
        {
            
        }

        static ServiceBase()
        {
            _instance = new TService();
        }
        
    }
}
