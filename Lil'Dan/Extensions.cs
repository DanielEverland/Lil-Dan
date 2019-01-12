using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Lil_Dan
{
    public static class Extensions
    {
        private static readonly Random random = new Random();

        public static T Random<T>(this IEnumerable<T> collection)
        {
            int index = random.Next(0, collection.Count());

            return collection.ElementAt(index);
        }
    }
}
