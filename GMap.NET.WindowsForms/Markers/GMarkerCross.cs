using System.Drawing;
using System.Runtime.Serialization;
using System;

namespace GMap.NET.WindowsForms.Markers
{
   /// <summary>
   /// GMapMarker
   /// </summary>
   [Serializable]
   public class GMarkerCross : GMapMarker, ISerializable
   {
      [NonSerialized]
      private Pen pen = Defaults.GMapPens.stroke_red;

      /// <summary>
      /// Marker Contructor
      /// </summary>
      /// <param name="_point"></param>
      public GMarkerCross(PointLatLng _point) : base(_point)
      {
         IsHitTestVisible = false;
      }

      /// <summary>
      /// Render a Marker
      /// </summary>
      /// <param name="g"></param>
      public override void OnRender(Graphics g)
      {
         Point p1 = new Point(LocalPosition.X, LocalPosition.Y);
         p1.Offset(0, -10);
         Point p2 = new Point(LocalPosition.X, LocalPosition.Y);
         p2.Offset(0, 10);

         Point p3 = new Point(LocalPosition.X, LocalPosition.Y);
         p3.Offset(-10, 0);
         Point p4 = new Point(LocalPosition.X, LocalPosition.Y);
         p4.Offset(10, 0);

         g.DrawLine(pen, p1.X, p1.Y, p2.X, p2.Y);
         g.DrawLine(pen, p3.X, p3.Y, p4.X, p4.Y);
      }

      #region ISerializable
      void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
      {
         base.GetObjectData(info, context);
      }
      #endregion

      #region Properties
      public Pen MarkerPen { get => pen; set => pen = value; }
      #endregion
   }
}
