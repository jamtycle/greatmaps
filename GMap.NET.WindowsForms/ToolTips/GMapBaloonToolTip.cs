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
   public class GMapBaloonToolTip : GMapToolTip, ISerializable
   {
      private float radius = 10f;

      #region Constructor
      /// <summary>
      /// Overrides the original Builder. It set's the padding in a equivalent of the default radiud (10f).
      /// </summary>
      /// <param name="_marker"></param>
      public GMapBaloonToolTip(GMapMarker _marker) : base(_marker)
      {
         TextPadding = new Size((int)radius, (int)radius);
      }

      /// <summary>
      /// Overrides the original Builder. It set's the padding in a equivalent of the default radiud (10f).
      /// </summary>
      /// <param name="_marker"></param>
      /// <param name="_offset"></param>
      public GMapBaloonToolTip(GMapMarker _marker, Point _offset) : base(_marker, _offset)
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
         if(Offset != null) rect.Offset(Offset);

         DrawBallon(g, rect);

         g.DrawString(Marker.ToolTipText, Font, ForegroundColor, rect, Format);
      }

      #region Drawing
      /// <summary>
      /// Draw's a ballon
      /// </summary>
      /// <param name="g"></param>
      /// <param name="_rect"></param>
      private void DrawBallon(Graphics g, Rectangle _rect)
      {
         using (GraphicsPath objGP = new GraphicsPath())
         {
            objGP.AddLine(_rect.X + 2 * radius, _rect.Y + _rect.Height, _rect.X + radius, _rect.Y + _rect.Height + radius);
            objGP.AddLine(_rect.X + radius, _rect.Y + _rect.Height + radius, _rect.X + radius, _rect.Y + _rect.Height);

            objGP.AddArc(_rect.X, _rect.Y + _rect.Height - (radius * 2), radius * 2, radius * 2, 90, 90);
            objGP.AddLine(_rect.X, _rect.Y + _rect.Height - (radius * 2), _rect.X, _rect.Y + radius);
            objGP.AddArc(_rect.X, _rect.Y, radius * 2, radius * 2, 180, 90);
            objGP.AddLine(_rect.X + radius, _rect.Y, _rect.X + _rect.Width - (radius * 2), _rect.Y);
            objGP.AddArc(_rect.X + _rect.Width - (radius * 2), _rect.Y, radius * 2, radius * 2, 270, 90);
            objGP.AddLine(_rect.X + _rect.Width, _rect.Y + radius, _rect.X + _rect.Width, _rect.Y + _rect.Height - (radius * 2));
            objGP.AddArc(_rect.X + _rect.Width - (radius * 2), _rect.Y + _rect.Height - (radius * 2), radius * 2, radius * 2, 0, 90); // Corner

            objGP.CloseFigure();

            g.FillPath(FillColor, objGP);
            using (Pen pen = new Pen(StrokeColor, StrokeWidth))
               g.DrawPath(pen, objGP);
            //new Pen(Stroke.Brush, Stroke.Width);
            //try { g.DrawPath(Stroke, objGP); }
            //catch { }
         }
      }

      /// <summary>
      /// Gets the rect of a tooltip
      /// </summary>
      /// <param name="_size"></param>
      /// <returns></returns>
      public override Rectangle TooltipRectangle(Size _size)
      {
         int x = Marker.ToolTipPosition.X;
         int y = Marker.ToolTipPosition.Y - _size.Height;
         int width = _size.Width + TextPadding.Width;
         int heigth = _size.Height + TextPadding.Height;

         return new Rectangle(x, y, width, heigth);
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
      protected GMapBaloonToolTip(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
         this.radius = Extensions.GetStruct<float>(info, "Radius", 10f);
      }

      #endregion

      #region Properties
      public float Radius { get => radius; set => radius = value; }
      #endregion
   }
}
