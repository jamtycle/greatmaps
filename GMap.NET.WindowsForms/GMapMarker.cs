using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;
using GMap.NET.WindowsForms.ToolTips;

namespace GMap.NET.WindowsForms.Markers
{
   /// <summary>
   /// GMap.NET marker
   /// </summary>
   [Serializable]
   public abstract class GMapMarker : ISerializable, IDisposable
   {
      #region Fields
      private GMapOverlay overlay;

      private PointLatLng position;
      private object tag;
      private Point offset;
      private Rectangle area;

      private GMapToolTip tooltip;
      private MarkerTooltipMode tooltip_mode = MarkerTooltipMode.OnMouseOver;
      private string tooltip_text;

      private bool visible = true;

      private bool disable_region_check = false;
      private bool is_hit_test_visible = true;
      private bool is_mouse_over = false;

      bool disposed = false;
      #endregion

      /// <summary>
      /// Marker Default Constructor
      /// </summary>
      /// <param name="_pos"></param>
      public GMapMarker(PointLatLng _pos)
      {
         this.Position = _pos;
      }

      /// <summary>
      /// Draws a marker
      /// </summary>
      /// <param name="g"></param>
      public virtual void OnRender(Graphics g)
      {

      }

      #region ISerializable Members
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
         info.AddValue("Position", this.position);
         info.AddValue("Tag", this.tag);
         info.AddValue("Offset", this.offset);
         info.AddValue("Area", this.area);
         info.AddValue("ToolTip", this.tooltip);
         info.AddValue("ToolTipMode", this.tooltip_mode);
         info.AddValue("ToolTipText", this.tooltip_text);
         info.AddValue("Visible", this.IsVisible);
         info.AddValue("DisableregionCheck", this.DisableRegionCheck);
         info.AddValue("IsHitTestVisible", this.IsHitTestVisible);
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="GMapMarker"/> class.
      /// </summary>
      /// <param name="info">The info.</param>
      /// <param name="context">The context.</param>
      protected GMapMarker(SerializationInfo info, StreamingContext context)
      {
         this.Position = Extensions.GetStruct<PointLatLng>(info, "Position", PointLatLng.Empty);
         this.Tag = Extensions.GetValue<object>(info, "Tag", null);
         this.Offset = Extensions.GetStruct<Point>(info, "Offset", Point.Empty);
         this.area = Extensions.GetStruct<Rectangle>(info, "Area", Rectangle.Empty);

         this.tooltip = Extensions.GetValue<GMapToolTip>(info, "ToolTip", null);
         if (this.tooltip != null) this.tooltip.Marker = this;

         this.ToolTipMode = Extensions.GetStruct<MarkerTooltipMode>(info, "ToolTipMode", MarkerTooltipMode.OnMouseOver);
         this.ToolTipText = info.GetString("ToolTipText");
         this.IsVisible = info.GetBoolean("Visible");
         this.DisableRegionCheck = info.GetBoolean("DisableregionCheck");
         this.IsHitTestVisible = info.GetBoolean("IsHitTestVisible");
      }
      #endregion

      #region IDisposable
      /// <summary>
      /// Dispose a Marker
      /// </summary>
      public virtual void Dispose()
      {
         if (!disposed)
         {
            disposed = true;
            Tag = null;
            if (this.tooltip == null) return;

            tooltip_text = null;
            this.tooltip.Dispose();
            this.tooltip = null;
         }
      }
      #endregion

      #region Properties
      /// <summary>
      /// Gets the current overlay
      /// </summary>
      public GMapOverlay Overlay { get => overlay; set => overlay = value; }
      /// <summary>
      /// Gets or Sets the Position of the Marker
      /// </summary>
      public PointLatLng Position
      {
         get => position;
         set
         {
            if (position == value) return;
            position = value;

            if (!IsVisible) return;
            if (Overlay != null && Overlay.Control != null)
               Overlay.Control.UpdateMarkerLocalPosition(this);
         }
      }
      /// <summary>
      /// Gets or Sets the Offset of the Marker.
      /// </summary>
      public Point Offset
      {
         get => offset;
         set
         {
            if (offset == value) return;
            offset = value;
            if (!IsVisible) return;

            if (Overlay != null && Overlay.Control != null)
               Overlay.Control.UpdateMarkerLocalPosition(this);
         }
      }
      /// <summary>
      /// marker position in local coordinates, internal only, do not set it manualy
      /// </summary>
      public Point LocalPosition
      {
         get => area.Location;
         set
         {
            if (area.Location == value) return;
            area.Location = value;

            if (Overlay != null && Overlay.Control != null)
               if (!Overlay.Control.HoldInvalidation)
                  Overlay.Control.Invalidate();
         }
      }
      /// <summary>
      /// Gets the Size of the Marker.
      /// </summary>
      public Size Size { get => area.Size; set => area.Size = value; }
      /// <summary>
      /// ToolTip position in local coordinates
      /// </summary>
      public Point ToolTipPosition
      {
         get
         {
            Point ret = area.Location;
            ret.Offset(-Offset.X, -Offset.Y);
            return ret;
         }
      }
      /// <summary>
      /// Gets the LocalArea of the Marker.
      /// </summary>
      public Rectangle LocalArea { get => area; }
      /// <summary>
      /// Gets or Sets the Text of the Tooltip. By default it creates a GMapRoundedToolTip
      /// </summary>
      public string ToolTipText
      {
         get => tooltip_text;
         set
         {
            if (this.tooltip == null && !string.IsNullOrEmpty(value))
               this.tooltip = new GMapRoundedToolTip(this);
            tooltip_text = value;
         }
      }
      /// <summary>
      /// is marker visible
      /// </summary>
      public bool IsVisible
      {
         get => visible;
         set
         {
            if (value == visible) return;

            visible = value;

            if (Overlay != null && Overlay.Control != null)
            {
               if (visible)
                  Overlay.Control.UpdateMarkerLocalPosition(this);
               else
                  if (Overlay.Control.IsMouseOverMarker)
               {
                  Overlay.Control.IsMouseOverMarker = false;
                  Overlay.Control.RestoreCursorOnLeave();
               }

               if (!Overlay.Control.HoldInvalidation)
                  Overlay.Control.Invalidate();
            }
         }
      }
      /// <summary>
      /// is mouse over marker
      /// </summary>
      public bool IsMouseOver { get => is_mouse_over; internal set => is_mouse_over = value; }
      /// <summary>
      /// Gets or Sets the Tag of the Marker.
      /// </summary>
      public object Tag { get => tag; set => tag = value; }
      /// <summary>
      /// Gets or Sets the Tooltip of the Marker.
      /// </summary>
      public GMapToolTip Tooltip { get => tooltip; set => tooltip = value; }
      /// <summary>
      /// Gets or Sets the Mode of the Tooltip.
      /// </summary>
      public MarkerTooltipMode ToolTipMode { get => tooltip_mode; set => tooltip_mode = value; }
      /// <summary>
      /// if true, marker will be rendered even if it's outside current view
      /// </summary>
      public bool DisableRegionCheck { get => disable_region_check; set => disable_region_check = value; }
      /// <summary>
      /// can maker receive input
      /// </summary>
      public bool IsHitTestVisible { get => is_hit_test_visible; set => is_hit_test_visible = value; }
      #endregion
   }
}
