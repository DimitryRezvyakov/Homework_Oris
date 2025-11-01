using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<(T1? First, T2? Second)> ZipLongest<T1, T2>(
        this IEnumerable<T1> first,
        IEnumerable<T2> second)
        {
            using var e1 = first.GetEnumerator();
            using var e2 = second.GetEnumerator();

            bool has1, has2;
            while ((has1 = e1.MoveNext()) | (has2 = e2.MoveNext())) // заметь: одиночное "|"
            {
                yield return (
                    has1 ? e1.Current : default,
                    has2 ? e2.Current : default
                );
            }
        }
    }
}
