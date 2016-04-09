﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testers
{
    public delegate void progPoolSizeUpdate(int size);

    static class ProgPool
    {

        public static event progPoolSizeUpdate OnProgPoolSizeUpdate = delegate { };

        public static List<Prog> pool = new List<Prog>();

        private static object locker = new object();

        public static void add(Prog prog)
        {
            pool.Add(prog);
            OnProgPoolSizeUpdate(pool.Count);
        }

        public static Prog pop(Tester requestedBy)
        {
            lock(locker)
            {
                OnProgPoolSizeUpdate(pool.Count);
                try
                {
                    return __pop(requestedBy);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            
        }

        private static Prog __pop (Tester reqestedBy)
        {
            foreach(var item in pool)
            {
                if (item.writer.name.Equals(reqestedBy.name)) continue;
                pool.Remove(item);
                return item;
            }
            return null;
        }
    }
}
