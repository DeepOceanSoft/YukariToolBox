﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace YukariToolBox.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool Update<TSource>
            (this IEnumerable<TSource> source, Action<TSource> updateAction)
            where TSource : class
        {
            foreach (TSource item in source)
            {
                updateAction(item);
            }

            return true;
        }

        public static (IEnumerable<TSource>, ICollection<TSource>) UpdateWhen<TSource>
            (this ICollection<TSource> source, Func<TSource, bool> whereAction)
            where TSource : struct
        {
            return (source.Where(whereAction), source);
        }

        public static (IEnumerable<TSource>, IList<TSource>) UpdateWhen<TSource>
            (this IList<TSource> source, Func<TSource, bool> whereAction)
            where TSource : struct
        {
            return (source.Where(whereAction), source);
        }

        public static (IEnumerable<TSource>, IList<TSource>) ExecuteUpdate<TSource>
            (this (IEnumerable<TSource>, IList<TSource>) source, TSource newValue)
            where TSource : struct
        {
            if (!source.Item2.IsReadOnly)
            {
                var searchResult = source.Item1.ToList();
                var sourceList   = source.Item2.ToList();
                for (var i = 0; i < searchResult.Count; i++)
                {
                    //查找源列表中有哪些匹配搜索到的内容，将其更新为新值
                    int index = sourceList.FindIndex(j => j.Equals(searchResult[i]));
                    source.Item2[index] = newValue;
                }
            }

            //source.Item2 = temp;
            return (source.Item1, source.Item2);
        }

        public static (IEnumerable<TSource>, ICollection<TSource>) ExecuteUpdate<TSource>
            (this (IEnumerable<TSource>, ICollection<TSource>) source, TSource newValue)
            where TSource : struct
        {
            if (!source.Item2.IsReadOnly)
            {
                bool hasUpdate    = false;
                var  searchResult = source.Item1.ToList();
                var  sourceList   = source.Item2.ToList();
                for (var i = 0; i < searchResult.Count; i++)
                {
                    int index = sourceList.FindIndex(j => j.Equals(searchResult[i]));
                    if (index != -1)
                    {
                        sourceList[index] = newValue;
                        hasUpdate         = true;
                    }
                }

                if (hasUpdate)
                {
                    source.Item2.Clear();
                    sourceList.ForEach(i => { source.Item2.Add(i); });
                }
            }

            //source.Item2 = temp;
            return (source.Item1, source.Item2);
        }
    }
}