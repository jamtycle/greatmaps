using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GMap.NET.WindowsForms.Defaults
{
   public static class GMapBrushes
   {
      /// <summary>
      /// Equivalent to AliceBlue with Alpha (222)
      /// </summary>
      public static readonly Brush fill_alice_blue = new SolidBrush(Color.FromArgb(222, Color.AliceBlue));
      /// <summary>
      /// Equivalent to White
      /// </summary>
      public static readonly Brush fill_white = new SolidBrush(Color.White);
      /// <summary>
      /// Equivalent to Gainsboro
      /// </summary>
      public static readonly Brush fill_gainsboro = new SolidBrush(Color.Gainsboro);
      /// <summary>
      /// Equivalent to RoyalBlue with Alpha (33)
      /// </summary>
      public static readonly Brush fill_royal_blue = new SolidBrush(Color.FromArgb(33, Color.RoyalBlue));
   }
}
