﻿
namespace GMap.NET.WindowsForms.ToolTips
{
   using System.Drawing;
   using System.Drawing.Drawing2D;
   using System;
   using System.Runtime.Serialization;

#if !PocketPC
   /// <summary>
   /// GMap.NET marker
   /// </summary>
   [Serializable]
   public class GMapRoundedToolTip : GMapToolTip, ISerializable
   {
      public float Radius = 10f;

      public GMapRoundedToolTip(GMapMarker marker)
         : base(marker)
      {
         TextPadding = new Size((int)Radius, (int)Radius);
      }

      public void DrawRoundRectangle(Graphics g, Pen pen, float h, float v, float width, float height, float radius)
      {
         using(GraphicsPath gp = new GraphicsPath())
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

            g.FillPath(Fill, gp);
            g.DrawPath(pen, gp);
         }
      }

      public override void OnRender(Graphics g)
      {
         if (this.Marker.Overlay.Control.Zoom <= 15.0) return;

         float emSize = (float)Math.Ceiling(75.0 * Math.Log10(1000.0 * this.Marker.Overlay.Control.Zoom / 14619.0));
         if (emSize <= 0.0f) emSize = 1f;

         this.Font = new Font("Segoe UI", emSize, FontStyle.Bold);

         System.Drawing.Size st = g.MeasureString(Marker.ToolTipText, Font).ToSize();

         System.Drawing.Rectangle rect = new System.Drawing.Rectangle(  Marker.ToolTipPosition.X - (st.Width / 2), 
                                                                        Marker.ToolTipPosition.Y - st.Height, 
                                                                        st.Width + TextPadding.Width * 2, 
                                                                        st.Height + TextPadding.Height);
         rect.Offset(Offset.X, Offset.Y);

         g.DrawLine(Stroke, Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y, rect.X + Radius / 2, rect.Y + rect.Height - Radius / 2);

         DrawRoundRectangle(g, Stroke, rect.X, rect.Y, rect.Width, rect.Height, Radius);

#if !PocketPC
         if(Format.Alignment == StringAlignment.Near)
         {
            rect.Offset(TextPadding.Width, 0);
         }
         g.DrawString(Marker.ToolTipText, Font, Foreground, rect, Format);
#else
         g.DrawString(ToolTipText, ToolTipFont, TooltipForeground, rect, ToolTipFormat);
#endif
      }


      #region ISerializable Members

      void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.AddValue("Radius", this.Radius);

         base.GetObjectData(info, context);
      }

      protected GMapRoundedToolTip(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
         this.Radius = Extensions.GetStruct<float>(info, "Radius", 10f);
      }

      #endregion
   }
#endif
}
