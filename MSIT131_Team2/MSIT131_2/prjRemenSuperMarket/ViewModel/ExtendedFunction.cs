using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prjRemenSuperMarket.Models;

namespace prjRemenSuperMarket.ViewModel
{
    public static class ExtendedFunction
    {
        /// <summary> 類別中指定欄位不重複 </summary>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> NoRepeatKeys = new HashSet<TKey>();
            
            foreach (TSource element in source)
            {
                if (NoRepeatKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        

    }
}
