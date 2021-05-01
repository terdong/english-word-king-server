using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWK_Server.TeamGehem.Utility
{
    public class Singleton<T> where T:class, new()
    {
        private static volatile T instance = null;
        private static readonly object syncRoot = new Object();

        protected Singleton() { }

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new T();
                    }
                }
                return instance;
            }
        }

        public virtual void Clear()
        {
            lock (syncRoot)
            {
                instance = null;
                instance = new T();
            }
        }  

        public virtual void Release()
        {
            lock (syncRoot)
            {
                instance = null;
            }
        }
    }
}
