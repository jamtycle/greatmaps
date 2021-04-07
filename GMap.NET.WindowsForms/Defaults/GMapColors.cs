using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GMap.NET.WindowsForms.Defaults
{
   /// <summary>
   /// Colors default values. WindowsForms ONLY!
   /// </summary>
   public static class GMapColors
   {
      /// <summary>
      /// Equivalent to MidnightBlue with Alpha (140)
      /// </summary>
      public static readonly Pen stroke_blue = new Pen(Color.FromArgb(140, Color.MidnightBlue)) { Width = 2, LineJoin = System.Drawing.Drawing2D.LineJoin.Round, StartCap = System.Drawing.Drawing2D.LineCap.RoundAnchor };
      /// <summary>
      /// Equivalent to Red
      /// </summary>
      public static readonly Pen stroke_red = new Pen(Color.Red) { Width = 1, LineJoin = System.Drawing.Drawing2D.LineJoin.Round, StartCap = System.Drawing.Drawing2D.LineCap.RoundAnchor };


      /// <summary>
      /// Equivalent to AliceBlue with Alpha (222)
      /// </summary>
      public static readonly Brush fill_alice_blue = new SolidBrush(Color.FromArgb(222, Color.AliceBlue));
   }
}
