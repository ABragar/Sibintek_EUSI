using Base.Map.Spatial;
using System;
using System.Collections.Generic;

namespace Base.Map.Clustering
{
    public class QuadClusterGridGroup : IClusterGridGroup
    {
        private const int _minGridZoom = 0;
        private const int _maxGridZoom = 18;

        private readonly IClusterGridFactory _gridFactory;
        private List<IClusterGrid> _grids;

        public QuadClusterGridGroup(IClusterGridFactory gridFactory, Bounds? constraintBounds = null, CacheSettings cacheSettings = null)
        {
            if (gridFactory == null)
            {
                throw new ArgumentNullException(nameof(gridFactory));
            }

            _gridFactory = gridFactory;
            InitialGrids(constraintBounds, cacheSettings);
        }

        private void InitialGrids(Bounds? constraintBounds, CacheSettings cacheSettings)
        {
            _grids = new List<IClusterGrid>(_maxGridZoom - _minGridZoom);

            for (var i = _minGridZoom; i <= _maxGridZoom; i++)
            {
                _grids.Add(_gridFactory.CreateGrid(i, constraintBounds, cacheSettings));
            }
        }

        public void AddObject(IGeoObject obj)
        {
            _grids.ForEach(x => x.AddObject(obj));
        }

        public void RemoveObject(IGeoObject obj)
        {
            _grids.ForEach(x => x.RemoveObject(obj));
        }
    }
}