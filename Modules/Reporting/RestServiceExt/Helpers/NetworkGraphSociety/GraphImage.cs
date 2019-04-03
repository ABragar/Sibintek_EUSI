using System;

namespace RestService.Helpers.NetworkGraphSociety
{
    internal class GraphImage
    {
        internal int Id { get; set; }
        internal String FileName { get; set; }
        
        /// <summary>
        /// Byte array of image graph
        /// </summary>
        internal byte[] Image { get; set; }
    }
}