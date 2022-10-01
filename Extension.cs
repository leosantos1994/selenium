using System.Collections.Generic;

namespace RoboVotacao
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
