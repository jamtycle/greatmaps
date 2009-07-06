﻿
namespace GMap.NET.Internals
{
   using System;

   /// <summary>
   /// maybe this??
   /// PROJCS["LKS94 / Lithuania TM",GEOGCS["LKS94",DATUM["D_Lithuania_1994",SPHEROID["GRS_1980",6378137,298.257222101]],PRIMEM["Greenwich",0],UNIT["Degree",0.017453292519943295]],PROJECTION["Transverse_Mercator"],PARAMETER["latitude_of_origin",0],PARAMETER["central_meridian",24],PARAMETER["scale_factor",0.9998],PARAMETER["false_easting",500000],PARAMETER["false_northing",0],UNIT["Meter",1]]
   /// </summary>
   internal class LKS94Projection : PureProjection
   {
      const double MinLatitude = -85.05112878;
      const double MaxLatitude = 85.05112878;
      const double MinLongitude = -180;
      const double MaxLongitude = 180;

      Size tileSize = new Size(512, 512);
      public override Size TileSize
      {
         get
         {
            return tileSize;
         }
      }

      public override double Axis
      {
         get
         {
            return 6378137;
         }
      }

      public override double Flattening
      {
         get
         {
            return (1.0 / 298.257222101);
         }
      }

      public override Point FromLatLngToPixel(double lat, double lng, int zoom)
      {
         Point ret = Point.Empty;

         lat = Clip(lat, MinLatitude, MaxLatitude);
         lng = Clip(lng, MinLongitude, MaxLongitude);

         Size s = GetTileMatrixSizePixel(zoom);
         double mapSizeX = s.Width;
         double mapSizeY = s.Height;

         double scale = 360.0 / mapSizeX;

         ret.Y = (int) ((90.0 - lat) / scale);
         ret.X = (int) ((lng + 180.0) / scale);

         return ret;
      }

      public override PointLatLng FromPixelToLatLng(int x, int y, int zoom)
      {
         PointLatLng ret = PointLatLng.Empty;

         Size s = GetTileMatrixSizePixel(zoom);
         double mapSizeX = s.Width;
         double mapSizeY = s.Height;

         double scale = 360.0 / mapSizeX;

         ret.Lat = 90 - (y * scale);
         ret.Lng = (x * scale) - 180;

         return ret;
      }

      /// <summary>
      /// Clips a number to the specified minimum and maximum values.
      /// </summary>
      /// <param name="n">The number to clip.</param>
      /// <param name="minValue">Minimum allowable value.</param>
      /// <param name="maxValue">Maximum allowable value.</param>
      /// <returns>The clipped value.</returns>
      double Clip(double n, double minValue, double maxValue)
      {
         return Math.Min(Math.Max(n, minValue), maxValue);
      }

      public override Point FromPixelToTileXY(Point p)
      {
         return new Point((int) (p.X/TileSize.Width), (int) (p.Y/TileSize.Height));
      }

      public override Point FromTileXYToPixel(Point p)
      {
         return new Point((p.X*TileSize.Width), (p.Y*TileSize.Height));
      }

      public override int GetTileMatrixItemCount(int zoom)
      {
         Size s = GetTileMatrixSizeXY(zoom);
         return (s.Width * s.Height);
      }

      public override Size GetTileMatrixSizeXY(int zoom)
      {
         int y = (int) Math.Pow(2, zoom);
         return new Size(2*y, y);
      }

      public override Size GetTileMatrixSizePixel(int zoom)
      {
         Size s = GetTileMatrixSizeXY(zoom);
         s.Width *= TileSize.Width;
         s.Height *= TileSize.Height;
         return s;
      }
   }
}