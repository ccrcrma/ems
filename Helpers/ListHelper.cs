using System.Collections.Generic;

namespace ems.Helpers
{
    public static class ListHelper<T>
    {
        public static List<T> AddRange(List<T> container, List<T> child)
        {
            if (child != null)
            {
                container.AddRange(child);
            }
            return container;
        }
    }
}