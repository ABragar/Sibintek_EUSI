using System;
using System.Collections.Concurrent;
using NLog;

namespace RestService.Helpers.NetworkGraphSociety
{
    internal class GraphImageProvider
    {
        private static readonly ConcurrentDictionary<int,GraphImage> _cache=new ConcurrentDictionary<int, GraphImage>();
        private static long _preClean;
        
        private static ILogger _log = LogManager.GetCurrentClassLogger();
        internal GraphBuilder _builder;

        internal GraphImageProvider(GraphBuilder builder)
        {
            _builder = builder;
        }
        
        /// <summary>
        /// Create and return <see cref="GraphImage"/> by society id.
        /// </summary>
        /// <param name="id">Society id</param>
        /// <param name="fromCache">Indicates the need to take the value from the cache</param>
        /// <returns></returns>
        internal GraphImage GetGraphImageBySocietyId(int id,bool fromCache=true)
        {
            if (!fromCache) return CreateGraphImageBySocietyId(id);
            if (_cache.ContainsKey(id)) return _cache[id];
            
            GraphImage graphImage = CreateGraphImageBySocietyId(id);
            if (!_cache.TryAdd(id, graphImage))
                _log.Info("Graph immage created but failed to add graph immage (Society id={id}) to the cache.");
            return graphImage;
        }

        private GraphImage CreateGraphImageBySocietyId(int id)
        {
            if(!_builder.IsReset) _builder.Reset();
            _builder.Id = id;
            _builder.Build();
            return _builder.Image;
        }

    }
}