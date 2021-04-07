using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using GMap.NET.WindowsForms.Defaults;
using GMap.NET.WindowsForms.Markers;

namespace GMap.NET.WindowsForms.ToolTips
{
   /// <summary>
   /// GMap.NET marker
   /// </summary>
   [Serializable]
   public class GMapToolTip : ISerializable, IDisposable
   {
      #region Fields
      private GMapMarker marker;
      private Point offset;
      /// <summary>
      /// string format
      /// </summary>
      [NonSerialized]
      private StringFormat format = GMapFonts.default_format;
      /// <summary>
      /// font
      /// </summary>
      [NonSerialized]
      private Font font = GMapFonts.default_font;
      /// <summary>
      /// specifies how the outline is painted
      /// </summary>
      [NonSerialized]
      private Pen stroke = GMapColors.stroke_blue;
      /// <summary>
      /// background color
      /// </summary>
      [NonSerialized]
      private Brush fill_color = GMapColors.fill_alice_blue;
      /// <summary>
      /// text foreground
      /// </summary>
      [NonSerialized]
      private Brush foreground_color = GMapFonts.default_foreground;
      /// <summary>
      /// text padding
      /// </summary>
      private Size text_padding = new Size(10, 10);

      private bool tooltip_scalling = true;
      private double zoom_max_value = 15d;
      private float tooltip_font_scale_factor = 2f;

      bool disposed = false;
      #endregion

      #region Constructors
      static GMapToolTip()
      {
      }

      /// <summary>
      /// Build's a Tooltip within the Marker.
      /// </summary>
      /// <param name="_marker"></param>
      public GMapToolTip(GMapMarker _marker)
      {
         this.Marker = _marker;
      }

      /// <summary>
      /// Builds' a Tooltip within the Marker.
      /// </summary>
      /// <param name="_marker"></param>
      /// <param name="_offset"></param>
      public GMapToolTip(GMapMarker _marker, Point _offset)
      {
         this.Marker = _marker;
         this.offset = _offset;
      }
      #endregion

      /// <summary>
      /// Render Method for tooltips
      /// </summary>
      /// <param name="g"></param>
      public virtual void OnRender(Graphics g)
      {
         Size st = g.MeasureString(Marker.ToolTipText, font).ToSize();
         Rectangle rect = new Rectangle(new Point(Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y - st.Height), new Size(st.Width + text_padding.Width, st.Height + text_padding.Height));
         rect.Offset(offset.X, offset.Y);

         g.DrawLine(stroke, Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y, rect.X, rect.Y + rect.Height / 2);

         g.FillRectangle(fill_color, rect);
         g.DrawRectangle(stroke, rect);

         g.DrawString(Marker.ToolTipText, font, foreground_color, rect, format);
      }

      /// <summary>
      /// Gets the rect of a tooltip
      /// </summary>
      /// <param name="_size"></param>
      /// <returns></returns>
      public virtual Rectangle TooltipRectangle(Size _size)
      {
         int x = Marker.ToolTipPosition.X; // - (_size.Width / 2);
         int y = Marker.ToolTipPosition.Y; // - _size.Height;
         int width = _size.Width;// + TextPadding.Width * 2;
         int heigth = _size.Height;// + TextPadding.Height;

         return new Rectangle(x, y, width, heigth);
      }

      #region ISerializable Members
      /// <summary>
      /// Initializes a new instance of the <see cref="GMapToolTip"/> class.
      /// </summary>
      /// <param name="info">The info.</param>
      /// <param name="context">The context.</param>
      protected GMapToolTip(SerializationInfo info, StreamingContext context)
      {
         this.offset = Extensions.GetStruct<Point>(info, "Offset", Point.Empty);
         this.text_padding = Extensions.GetStruct<Size>(info, "TextPadding", new Size(10, 10));
      }

      /// <summary>
      /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
      /// </summary>
      /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
      /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
      /// <exception cref="T:System.Security.SecurityException">
      /// The caller does not have the required permission.
      /// </exception>
      public void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.AddValue("Offset", this.offset);
         info.AddValue("TextPadding", this.text_padding);
      }
      #endregion

      #region IDisposable Members
      /// <summary>
      /// Disposes a TooolTip
      /// </summary>
      public void Dispose()
      {
         if (!disposed)
         {
            disposed = true;
         }
      }
      #endregion

      #region Properties
      /// <summary>
      /// Gets or Sets the Marker.
      /// </summary>
      public GMapMarker Marker { get => marker; internal set => marker = value; }
      /// <summary>
      /// Gets or Sets the Offset.
      /// </summary>
      public Point Offset { get => offset; set => offset = value; }
      /// <summary>
      /// Gets or Sets the StringFormat.
      /// </summary>
      public StringFormat Format { get => format; set => format = value; }
      /// <summary>
      /// Gets or Sets the Font.
      /// </summary>
      public Font Font { get => font; set => font = value; }
      /// <summary>
      /// Gets or Sets the Stroke.
      /// </summary>
      public Pen Stroke { get => stroke; set => stroke = value; }
      /// <summary>
      /// Gets or Sets the Fill Color.
      /// </summary>
      public Brush FillColor { get => fill_color; set => fill_color = value; }
      /// <summary>
      /// Gets or Sets the Foreground (Text) Color.
      /// </summary>
      public Brush ForegroundColor { get => foreground_color; set => foreground_color = value; }
      /// <summary>
      /// Gets or Sets the Text Padding.
      /// </summary>
      public Size TextPadding { get => text_padding; set => text_padding = value; }

      /// <summary>
      /// Gets or Sets if the Tooltip must be re-scalled based on the Zoom of the control.
      /// </summary>
      public bool TooltipScalling { get => tooltip_scalling; set => tooltip_scalling = value; }
      /// <summary>
      /// Gets or Sets the max value that the zoom can be before it stops rendering the tooltip.
      /// </summary>
      public double ZoomMaxValue { get => zoom_max_value; set => zoom_max_value = value; }
      /// <summary>
      /// Gets or Sets the factor of scalling.
      /// </summary>
      public float TooltipFontScaleFactor { get => tooltip_font_scale_factor; set => tooltip_font_scale_factor = value; }
      /// <summary>
      /// Gets the current font-size based on the current Zoom.
      /// </summary>
      public virtual float FontSize
      {
         get
         {
            if (!tooltip_scalling) return this.font.Size;

            var info = (float)Math.Ceiling(75.0 * Math.Log10(1000.0 * this.Marker.Overlay.Control.Zoom / 14619.0)) + tooltip_font_scale_factor;

            if (info <= 0) return 1f;
            else return info;
         }
      }
      #endregion
   }
}
