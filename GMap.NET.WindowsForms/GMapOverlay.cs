using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;
using GMap.NET.ObjectModel;
using GMap.NET.WindowsForms.Markers;

namespace GMap.NET.WindowsForms
{
   /// <summary>
   /// GMap.NET overlay
   /// </summary>
   [Serializable]
   public class GMapOverlay : ISerializable, IDeserializationCallback, IDisposable
   {
      #region Fields
      private GMapControl control;
      private string id;

      private bool isVisible = true;
      private bool isHitTestVisible = true;
      private bool isZoomSignificant = true;

      private readonly ObservableCollectionThreadSafe<GMapMarker> markers = new ObservableCollectionThreadSafe<GMapMarker>();
      private readonly ObservableCollectionThreadSafe<GMapRoute> routes = new ObservableCollectionThreadSafe<GMapRoute>();
      private readonly ObservableCollectionThreadSafe<GMapPolygon> polygons = new ObservableCollectionThreadSafe<GMapPolygon>();
      #endregion

      /// <summary>
      /// Build an Overlay
      /// </summary>
      public GMapOverlay()
      {
         CreateEvents();
      }

      /// <summary>
      /// Build an Overlay with an ID
      /// </summary>
      /// <param name="_id"></param>
      public GMapOverlay(string _id)
      {
         Id = _id;
         CreateEvents();
      }

      #region Rendering
      /// <summary>
      /// updates local positions of objects
      /// </summary>
      internal void ForceUpdate()
      {
         if (Control == null) return;


         foreach (GMapMarker obj in Markers)
            if (obj.IsVisible)
               Control.UpdateMarkerLocalPosition(obj);

         foreach (GMapPolygon obj in Polygons)
            if (obj.IsVisible)
               Control.UpdatePolygonLocalPosition(obj);

         foreach (GMapRoute obj in Routes)
            if (obj.IsVisible)
               Control.UpdateRouteLocalPosition(obj);
      }

      /// <summary>
      /// renders objects/routes/polygons
      /// </summary>
      /// <param name="g"></param>
      public virtual void OnRender(Graphics g)
      {
         if (Control == null) return;


         if (Control.RoutesEnabled)
            RenderRoutes(g);

         if (Control.PolygonsEnabled)
            RenderPolygons(g);

         if (Control.MarkersEnabled)
            RenderMarkersAndToolTips(g);
      }

      private void RenderRoutes(Graphics g)
      {
         for (int i = 0; i < routes.Count; i++)
            if (routes[i].IsVisible)
               routes[i].OnRender(g);
      }

      private void RenderPolygons(Graphics g)
      {
         for (int i = 0; i < polygons.Count; i++)
            if (polygons[i].IsVisible)
               polygons[i].OnRender(g);
      }

      private void RenderMarkersAndToolTips(Graphics g)
      {
         for (int i = 0; i < markers.Count; i++)
            if (markers[i].IsVisible || markers[i].DisableRegionCheck)
               markers[i].OnRender(g);

         for (int i = 0; i < markers.Count; i++)
            if (markers[i].Tooltip != null && markers[i].IsVisible)
               if(!string.IsNullOrEmpty(markers[i].ToolTipText) && (markers[i].ToolTipMode == MarkerTooltipMode.Always || (markers[i].ToolTipMode == MarkerTooltipMode.OnMouseOver && markers[i].IsMouseOver)))
                  markers[i].Tooltip.OnRender(g);
      }
      #endregion

      #region Eventing
      private void CreateEvents()
      {
         Markers.CollectionChanged += new NotifyCollectionChangedEventHandler(Markers_CollectionChanged);
         Routes.CollectionChanged += new NotifyCollectionChangedEventHandler(Routes_CollectionChanged);
         Polygons.CollectionChanged += new NotifyCollectionChangedEventHandler(Polygons_CollectionChanged);
      }

      private void ClearEvents()
      {
         Markers.CollectionChanged -= new NotifyCollectionChangedEventHandler(Markers_CollectionChanged);
         Routes.CollectionChanged -= new NotifyCollectionChangedEventHandler(Routes_CollectionChanged);
         Polygons.CollectionChanged -= new NotifyCollectionChangedEventHandler(Polygons_CollectionChanged);
      }
      #endregion

      #region Collections
      private void Polygons_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
      {
         if (e.NewItems != null)
         {
            foreach (GMapPolygon obj in e.NewItems)
            {
               if (obj != null)
               {
                  obj.Overlay = this;
                  if (Control != null)
                  {
                     Control.UpdatePolygonLocalPosition(obj);
                  }
               }
            }
         }

         if (Control != null)
         {
            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Reset)
            {
               if (Control.IsMouseOverPolygon)
               {
                  Control.IsMouseOverPolygon = false;
#if !PocketPC
                  Control.RestoreCursorOnLeave();
#endif
               }
            }

            if (!Control.HoldInvalidation)
            {
               Control.Invalidate();
            }
         }
      }

      private void Routes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
      {
         if (e.NewItems != null)
         {
            foreach (GMapRoute obj in e.NewItems)
            {
               if (obj != null)
               {
                  obj.Overlay = this;
                  if (Control != null)
                  {
                     Control.UpdateRouteLocalPosition(obj);
                  }
               }
            }
         }

         if (Control != null)
         {
            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Reset)
            {
               if (Control.IsMouseOverRoute)
               {
                  Control.IsMouseOverRoute = false;
#if !PocketPC
                  Control.RestoreCursorOnLeave();
#endif
               }
            }

            if (!Control.HoldInvalidation)
            {
               Control.Invalidate();
            }
         }
      }

      private void Markers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
      {
         if (e.NewItems != null)
         {
            foreach (GMapMarker obj in e.NewItems)
            {
               if (obj != null)
               {
                  obj.Overlay = this;
                  if (Control != null)
                  {
                     Control.UpdateMarkerLocalPosition(obj);
                  }
               }
            }
         }

         if (Control != null)
         {
            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Reset)
            {
               if (Control.IsMouseOverMarker)
               {
                  Control.IsMouseOverMarker = false;
#if !PocketPC
                  Control.RestoreCursorOnLeave();
#endif
               }
            }

            if (!Control.HoldInvalidation)
            {
               Control.Invalidate();
            }
         }
      }

      /// <summary>
      /// Clears all the lists in the overlay
      /// </summary>
      public void ClearAllLists()
      {
         Markers.Clear();
         Routes.Clear();
         Polygons.Clear();
      }
      #endregion

      #region ISerializable

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
         info.AddValue("Id", this.Id);
         info.AddValue("IsVisible", this.IsVisible);

         GMapMarker[] markerArray = new GMapMarker[this.Markers.Count];
         this.Markers.CopyTo(markerArray, 0);
         info.AddValue("Markers", markerArray);

         GMapRoute[] routeArray = new GMapRoute[this.Routes.Count];
         this.Routes.CopyTo(routeArray, 0);
         info.AddValue("Routes", routeArray);

         GMapPolygon[] polygonArray = new GMapPolygon[this.Polygons.Count];
         this.Polygons.CopyTo(polygonArray, 0);
         info.AddValue("Polygons", polygonArray);
      }

      private GMapMarker[] deserializedMarkerArray;
      private GMapRoute[] deserializedRouteArray;
      private GMapPolygon[] deserializedPolygonArray;

      /// <summary>
      /// Initializes a new instance of the <see cref="GMapOverlay"/> class.
      /// </summary>
      /// <param name="info">The info.</param>
      /// <param name="context">The context.</param>
      protected GMapOverlay(SerializationInfo info, StreamingContext context)
      {
         this.Id = info.GetString("Id");
         this.IsVisible = info.GetBoolean("IsVisible");

         this.deserializedMarkerArray = Extensions.GetValue<GMapMarker[]>(info, "Markers", new GMapMarker[0]);
         this.deserializedRouteArray = Extensions.GetValue<GMapRoute[]>(info, "Routes", new GMapRoute[0]);
         this.deserializedPolygonArray = Extensions.GetValue<GMapPolygon[]>(info, "Polygons", new GMapPolygon[0]);

         CreateEvents();
      }

      #endregion

      #region IDeserializationCallback Members

      /// <summary>
      /// Runs when the entire object graph has been deserialized.
      /// </summary>
      /// <param name="sender">The object that initiated the callback. The functionality for this parameter is not currently implemented.</param>
      public void OnDeserialization(object sender)
      {
         // Populate Markers
         foreach (GMapMarker marker in deserializedMarkerArray)
         {
            marker.Overlay = this;
            this.Markers.Add(marker);
         }

         // Populate Routes
         foreach (GMapRoute route in deserializedRouteArray)
         {
            route.Overlay = this;
            this.Routes.Add(route);
         }

         // Populate Polygons
         foreach (GMapPolygon polygon in deserializedPolygonArray)
         {
            polygon.Overlay = this;
            this.Polygons.Add(polygon);
         }
      }

      #endregion

      #region IDisposable
      bool disposed = false;

      /// <summary>
      /// Disposes The Overlay
      /// </summary>
      public void Dispose()
      {
         if (!disposed)
         {
            disposed = true;

            ClearEvents();

            foreach (var m in Markers)
            {
               m.Dispose();
            }

            foreach (var r in Routes)
            {
               r.Dispose();
            }

            foreach (var p in Polygons)
            {
               p.Dispose();
            }

            ClearAllLists();
         }
      }
      #endregion

      #region Properties
      /// <summary>
      /// Gets And Sets The Control of the Overlay
      /// </summary>
      public GMapControl Control { get => control; internal set => control = value; }
      /// <summary>
      /// if false don't consider contained objects when box zooming
      /// </summary>
      public bool IsZoomSignificant { get => isZoomSignificant; set => isZoomSignificant = value; }
      /// <summary>
      /// HitTest visibility for entire overlay
      /// </summary>
      public bool IsHitTestVisible { get => isHitTestVisible; set => isHitTestVisible = value; }
      /// <summary>
      /// is overlay visible
      /// </summary>
      public bool IsVisible
      {
         get => isVisible;
         set
         {
            if (value != isVisible)
            {
               isVisible = value;

               if (Control != null)
               {
                  if (isVisible)
                  {
                     Control.HoldInvalidation = true;
                     {
                        ForceUpdate();
                     }
                     Control.Refresh();
                  }
                  else
                  {
                     if (Control.IsMouseOverMarker)
                     {
                        Control.IsMouseOverMarker = false;
                     }

                     if (Control.IsMouseOverPolygon)
                     {
                        Control.IsMouseOverPolygon = false;
                     }

                     if (Control.IsMouseOverRoute)
                     {
                        Control.IsMouseOverRoute = false;
                     }
#if !PocketPC
                     Control.RestoreCursorOnLeave();
#endif

                     if (!Control.HoldInvalidation)
                     {
                        Control.Invalidate();
                     }
                  }
               }
            }
         }
      }
      /// <summary>
      /// overlay Id
      /// </summary>
      public string Id { get => id; set => id = value; }
      /// <summary>
      /// list of markers, should be thread safe
      /// </summary>
      public ObservableCollectionThreadSafe<GMapMarker> Markers => markers;
      /// <summary>
      /// list of routes, should be thread safe
      /// </summary>
      public ObservableCollectionThreadSafe<GMapRoute> Routes => routes;
      /// <summary>
      /// list of polygons, should be thread safe
      /// </summary>
      public ObservableCollectionThreadSafe<GMapPolygon> Polygons => polygons;
      #endregion
   }
}