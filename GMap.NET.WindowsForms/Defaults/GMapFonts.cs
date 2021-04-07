using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GMap.NET.WindowsForms.Defaults
{
   /// <summary>
   /// Font-related utilities default values. WindowsForms ONLY!
   /// </summary>
   public static class GMapFonts
   {
      /// <summary>
      /// Default string format
      /// </summary>
      public static readonly StringFormat default_format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
      /// <summary>
      /// Default font
      /// </summary>
      public static readonly Font default_font = new Font("Segoe UI", 9f, FontStyle.Regular);
      /// <summary>
      /// Default forecolor
      /// </summary>
      public static readonly Brush default_foreground = new SolidBrush(Color.Black);
   }
}
