using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class Extension
    {
        public static List<T> Splice<T>(this List<T> list, int index, int count)
        {
            lock (list)
            {
                var items = list.GetRange(index, count);
                list.RemoveRange(index, count);
                return items;
            }
        }
    }
}
