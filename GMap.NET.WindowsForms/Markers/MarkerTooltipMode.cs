using System;
using System.Collections.Generic;
using System.Text;

namespace GMap.NET.WindowsForms.Markers
{
   /// <summary>
   /// Defines the Mode of the tooltip in a Marker
   /// </summary>
   public enum MarkerTooltipMode
   {
      /// <summary>
      /// Tooltips pop's up when the mouse is over it.
      /// </summary>
      OnMouseOver,
      /// <summary>
      /// Hides the tooltip.
      /// </summary>
      Hide,
      /// <summary>
      /// Always shows the tooltip.
      /// </summary>
      Always,
   }
}
