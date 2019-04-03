using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Extensions
{
    public static class TreeObjectExtensions
    {
        public static TResult ToTree<TResult, TTree, TParent>(this IEnumerable<TTree> source,
            Func<TTree, TParent> parent_selector,
            Func<TParent, TTree, IEnumerable<TResult>, TResult> result_selector,
            TTree root)
            where TTree : ITreeObject
        {
            var all_child_groups = source.Where(x => x.ParentID != null).GroupBy(x => x.ParentID.Value)
                .ToDictionary(x => x.Key, x => (IEnumerable<TTree>)x);

            return ToTree(all_child_groups, parent_selector, result_selector, parent_selector(default(TTree)), root);
        }

        public static TResult ToTree<TResult, TTree>(this IEnumerable<TTree> source,
            Func<TTree, IEnumerable<TResult>, TResult> result_selector,
            TTree root) where TTree : ITreeObject
        {
            var all_child_groups = source.Where(x => x.ParentID != null).GroupBy(x => x.ParentID.Value)
                .ToDictionary(x => x.Key, x => (IEnumerable<TTree>)x);

            return ToTree<TResult, TTree, object>(all_child_groups,
                x => null, (parent, current, childs) => result_selector(current, childs),
                null, root);
        }

        private static TResult ToTree<TResult, TTree, TParent>(IDictionary<int, IEnumerable<TTree>> all_child_groups,
            Func<TTree, TParent> parent_selector,
            Func<TParent, TTree, IEnumerable<TResult>, TResult> result_selector,
            TParent parent,
            TTree current
            )
            where TTree : ITreeObject
        {
            IEnumerable<TTree> childs;
            return all_child_groups.TryGetValue(current.ID, out childs) ?
                result_selector(parent, current, childs.Select(x => ToTree(all_child_groups, parent_selector, result_selector, parent_selector(current), x))) :
                result_selector(parent, current, Enumerable.Empty<TResult>());
        }
    }
}