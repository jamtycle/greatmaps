using System.Drawing;
using System.Collections.Generic;
using GMap.NET.WindowsForms.Properties;
using System;
using System.Runtime.Serialization;

namespace GMap.NET.WindowsForms.Markers
{
   /// <summary>
   /// Marker used to display build-in markers.
   /// </summary>
   [Serializable]
   public class GMarkerGoogle : GMapMarker, ISerializable, IDeserializationCallback
   {
      private Bitmap bitmap;
      private Bitmap bitmap_shadow;
      private GMarkerGoogleType type;
      static readonly Dictionary<string, Bitmap> icon_cache = new Dictionary<string, Bitmap>();

      #region Constructors
      /// <summary>
      /// Marker using the build-in markers.
      /// </summary>
      /// <param name="_point"></param>
      /// <param name="_type"></param>
      public GMarkerGoogle(PointLatLng _point, GMarkerGoogleType _type) : base(_point)
      {
         this.type = _type;
         if (_type != GMarkerGoogleType.none) LoadBitmap();
      }

      /// <summary>
      /// marker using manual bitmap, NonSerialized
      /// </summary>
      /// <param name="_point"></param>
      /// <param name="_bitmap"></param>
      public GMarkerGoogle(PointLatLng _point, Bitmap _bitmap) : base(_point)
      {
         this.bitmap = _bitmap;
         Size = new Size(_bitmap.Width, _bitmap.Height);
         Offset = new Point(-Size.Width / 2, -Size.Height);
      }
      #endregion

      private void LoadBitmap()
      {
         bitmap = GetIcon(Type.ToString());
         Size = new System.Drawing.Size(bitmap.Width, bitmap.Height);

         switch (Type)
         {
            case GMarkerGoogleType.arrow:
               {
                  Offset = new Point(-11, -Size.Height);

                  if (arrowshadow == null)
                  {
                     arrowshadow = Resources.arrowshadow;
                  }
                  bitmap_shadow = arrowshadow;
               }
               break;

            case GMarkerGoogleType.blue:
            case GMarkerGoogleType.blue_dot:
            case GMarkerGoogleType.green:
            case GMarkerGoogleType.green_dot:
            case GMarkerGoogleType.yellow:
            case GMarkerGoogleType.yellow_dot:
            case GMarkerGoogleType.lightblue:
            case GMarkerGoogleType.lightblue_dot:
            case GMarkerGoogleType.orange:
            case GMarkerGoogleType.orange_dot:
            case GMarkerGoogleType.pink:
            case GMarkerGoogleType.pink_dot:
            case GMarkerGoogleType.purple:
            case GMarkerGoogleType.purple_dot:
            case GMarkerGoogleType.red:
            case GMarkerGoogleType.red_dot:
               {
                  Offset = new Point(-Size.Width / 2 + 1, -Size.Height + 1);

                  if (msmarker_shadow == null)
                  {
                     msmarker_shadow = Resources.msmarker_shadow;
                  }
                  bitmap_shadow = msmarker_shadow;
               }
               break;

            case GMarkerGoogleType.black_small:
            case GMarkerGoogleType.blue_small:
            case GMarkerGoogleType.brown_small:
            case GMarkerGoogleType.gray_small:
            case GMarkerGoogleType.green_small:
            case GMarkerGoogleType.yellow_small:
            case GMarkerGoogleType.orange_small:
            case GMarkerGoogleType.purple_small:
            case GMarkerGoogleType.red_small:
            case GMarkerGoogleType.white_small:
               {
                  Offset = new Point(-Size.Width / 2, -Size.Height + 1);

                  if (shadow_small == null)
                  {
                     shadow_small = Resources.shadow_small;
                  }
                  bitmap_shadow = shadow_small;
               }
               break;

            case GMarkerGoogleType.green_big_go:
            case GMarkerGoogleType.yellow_big_pause:
            case GMarkerGoogleType.red_big_stop:
               {
                  Offset = new Point(-Size.Width / 2, -Size.Height + 1);
                  if (msmarker_shadow == null)
                  {
                     msmarker_shadow = Resources.msmarker_shadow;
                  }
                  bitmap_shadow = msmarker_shadow;
               }
               break;

            case GMarkerGoogleType.blue_pushpin:
            case GMarkerGoogleType.green_pushpin:
            case GMarkerGoogleType.yellow_pushpin:
            case GMarkerGoogleType.lightblue_pushpin:
            case GMarkerGoogleType.pink_pushpin:
            case GMarkerGoogleType.purple_pushpin:
            case GMarkerGoogleType.red_pushpin:
               {
                  Offset = new Point(-9, -Size.Height + 1);

                  if (pushpin_shadow == null)
                  {
                     pushpin_shadow = Resources.pushpin_shadow;
                  }
                  bitmap_shadow = pushpin_shadow;
               }
               break;
         }
      }

      /// <summary>
      /// Renders a Marker
      /// </summary>
      /// <param name="g"></param>
      public override void OnRender(Graphics g)
      {
         if (bitmap_shadow != null) g.DrawImage(bitmap_shadow, LocalPosition.X, LocalPosition.Y, bitmap_shadow.Width, bitmap_shadow.Height);
         g.DrawImage(bitmap, LocalPosition.X, LocalPosition.Y, Size.Width, Size.Height);
      }

      #region Caché & Disposing
      internal static Bitmap GetIcon(string name)
      {
         Bitmap ret;
         if (!icon_cache.TryGetValue(name, out ret))
         {
            ret = Resources.ResourceManager.GetObject(name, Resources.Culture) as Bitmap;
            icon_cache.Add(name, ret);
         }
         return ret;
      }

      /// <summary>
      /// Dispose the Marker
      /// </summary>
      public override void Dispose()
      {
         if (bitmap != null)
         {
            if (!icon_cache.ContainsValue(bitmap))
            {
               bitmap.Dispose();
               bitmap = null;
            }
         }

         base.Dispose();
      }
      #endregion

      #region ISerializable Members
      void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.AddValue("type", this.Type);
         //info.AddValue("Bearing", this.Bearing);

         base.GetObjectData(info, context);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="info"></param>
      /// <param name="context"></param>
      protected GMarkerGoogle(SerializationInfo info, StreamingContext context) : base(info, context)
      {
         this.type = Extensions.GetStruct(info, "type", GMarkerGoogleType.none);
      }
      #endregion

      #region IDeserializationCallback Members
      /// <summary>
      /// Desarilization Method
      /// </summary>
      /// <param name="sender"></param>
      public void OnDeserialization(object sender)
      {
         if (Type != GMarkerGoogleType.none)
         {
            LoadBitmap();
         }
      }
      #endregion      

      #region Statics
      static Bitmap arrowshadow;
      static Bitmap msmarker_shadow;
      static Bitmap shadow_small;
      static Bitmap pushpin_shadow;
      #endregion

      #region Properties
      /// <summary>
      /// Gets the current Bitmap
      /// </summary>
      public Bitmap Bitmap { get => bitmap; }
      /// <summary>
      /// Gets the current BitmapShadow
      /// </summary>
      public Bitmap BitmapShadow { get => bitmap_shadow; }
      /// <summary>
      /// Gets the current MarkerType
      /// </summary>
      public GMarkerGoogleType Type { get => type; }
      /// <summary>
      /// Gets the icon cache
      /// </summary>
      public static Dictionary<string, Bitmap> IconCache => icon_cache;
      #endregion
   }
}
