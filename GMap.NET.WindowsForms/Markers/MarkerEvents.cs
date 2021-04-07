using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GMap.NET.WindowsForms.Markers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
   public delegate void MarkerClick(GMapMarker item, MouseEventArgs e);
   public delegate void MarkerEnter(GMapMarker item);
   public delegate void MarkerLeave(GMapMarker item);
   public delegate void MarkerDoubleClick(GMapMarker item, MouseEventArgs e);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
