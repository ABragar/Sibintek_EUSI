using System;
using Microsoft.Msagl.Drawing;

namespace RestService.Helpers.NetworkGraphSociety
{
    internal class GraphBuilderOptions
    {
        /// <summary>
        /// Using to named Graph. Does not affect to file name. 
        /// </summary>
        internal string Name { get; set; }
        internal int Width { get; set; }
        internal int Height { get; set; }
        internal Color RootColor { get; set; }
        internal Color SheetColor { get; set; }
        internal Color OtherColor { get; set; }
        internal string Root { get; set; }
        internal string Sheet{ get; set; }


        public GraphBuilderOptions Clone()
        {
            return new GraphBuilderOptions()
            { 
                Name = this.Name,
                Width = this.Width,
                Height = this.Height,
                RootColor = this.RootColor,
                SheetColor = this.SheetColor,
                OtherColor = this.OtherColor,
                Root = this.Root,
                Sheet = this.Sheet
            };
        }
    }
}