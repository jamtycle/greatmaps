using System.Drawing;
using System.Drawing.Drawing2D;
using System;
using System.Runtime.Serialization;
using GMap.NET.WindowsForms.Markers;

namespace GMap.NET.WindowsForms.ToolTips
{
   /// <summary>
   /// GMap.NET marker
   /// </summary>
   [Serializable]
   public class GMapRoundedToolTip : GMapToolTip, ISerializable
   {
      private float radius = 10f;

      #region Constructors
      /// <summary>
      /// Overrides the original Builder. It set's the padding in a equivalent of the default radiud (10f).
      /// </summary>
      /// <param name="_marker"></param>
      public GMapRoundedToolTip(GMapMarker _marker) : base(_marker)
      {
         TextPadding = new Size((int)radius, (int)radius);
      }

      /// <summary>
      /// Overrides the original Builder. It set's the padding in a equivalent of the default radiud (10f).
      /// </summary>
      /// <param name="_marker"></param>
      /// <param name="_offset"></param>
      public GMapRoundedToolTip(GMapMarker _marker, Point _offset) : base(_marker, _offset)
      {
         TextPadding = new Size((int)radius, (int)radius);
      }

      #endregion

      /// <summary>
      /// Renders a tooltip
      /// </summary>
      /// <param name="g"></param>
      public override void OnRender(Graphics g)
      {
         if (TooltipScalling && this.Marker.Overlay.Control.Zoom <= ZoomMaxValue) return;

         var cfont = new Font(this.Font.FontFamily, FontSize, this.Font.Style);

         Size st = g.MeasureString(Marker.ToolTipText, cfont).ToSize();

         Rectangle rect = TooltipRectangle(st);
         if (Offset != null) rect.Offset(Offset);

         //g.DrawLine(Stroke, Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y, rect.X + radius / 2, rect.Y + rect.Height - radius / 2);

         //g.DrawRectangle(Defaults.GMapColors.stroke_red, this.Marker.ToolTipPosition.X - (st.Width / 2), this.Marker.ToolTipPosition.Y - (st.Height / 2), 4, 4);
         DrawRoundRectangle(g, this.Marker.ToolTipPosition.X - (st.Width / 2), this.Marker.ToolTipPosition.Y - (st.Height / 2), rect.Width + TextPadding.Width, rect.Height + TextPadding.Height, radius);

         if (Format.Alignment == StringAlignment.Near) rect.Offset(TextPadding.Width, 0);

         g.DrawString(Marker.ToolTipText, cfont, ForegroundColor, this.Marker.ToolTipPosition.X + (TextPadding.Width / 2), this.Marker.ToolTipPosition.Y + (TextPadding.Height / 2), Format);
      }

      #region Drawing
      /// <summary>
      /// Draw's a rounded Rectangle
      /// </summary>
      /// <param name="g"></param>
      /// <param name="h"></param>
      /// <param name="v"></param>
      /// <param name="width"></param>
      /// <param name="height"></param>
      /// <param name="radius"></param>
      private void DrawRoundRectangle(Graphics g, float h, float v, float width, float height, float radius)
      {
         using (GraphicsPath gp = new GraphicsPath())
         {
            gp.AddLine(h + radius, v, h + width - (radius * 2), v);
            gp.AddArc(h + width - (radius * 2), v, radius * 2, radius * 2, 270, 90);
            gp.AddLine(h + width, v + radius, h + width, v + height - (radius * 2));
            gp.AddArc(h + width - (radius * 2), v + height - (radius * 2), radius * 2, radius * 2, 0, 90); // Corner
            gp.AddLine(h + width - (radius * 2), v + height, h + radius, v + height);
            gp.AddArc(h, v + height - (radius * 2), radius * 2, radius * 2, 90, 90);
            gp.AddLine(h, v + height - (radius * 2), h, v + radius);
            gp.AddArc(h, v, radius * 2, radius * 2, 180, 90);

            gp.CloseFigure();

            g.FillPath(FillColor, gp);
            g.DrawPath(Stroke, gp);
         }
      }
      #endregion

      #region ISerializable Members
      void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.AddValue("Radius", this.radius);

         base.GetObjectData(info, context);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="info"></param>
      /// <param name="context"></param>
      protected GMapRoundedToolTip(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
         this.radius = Extensions.GetStruct<float>(info, "Radius", 10f);
      }
      #endregion

      #region Properties
      /// <summary>
      /// Gets or Sets the current radius.
      /// </summary>
      public float Radius { get => radius; set => radius = value; }
      #endregion
   }
}
