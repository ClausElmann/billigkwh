using BilligKwhWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BilligKwhWebApp.Extensions
{
    public static class ListExtensions
    {
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> current, IPagedResultRequest pagingDetails)
        {
            if (pagingDetails is null)
                throw new ArgumentNullException(nameof(pagingDetails));

            return current.Skip(pagingDetails.Page * pagingDetails.PageSize).Take(pagingDetails.PageSize);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (rng == null) throw new ArgumentNullException(nameof(rng));

            return source.ShuffleIterator(rng);
        }

        private static IEnumerable<T> ShuffleIterator<T>(
            this IEnumerable<T> source, Random rng)
        {
            List<T> buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
#pragma warning disable CA5394 // Do not use insecure randomness
                int j = rng.Next(i, buffer.Count);
#pragma warning restore CA5394 // Do not use insecure randomness
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }
}
