using UnityEngine;
using System.Collections;


namespace InControl
{
	public static class Utility
	{
		private static Vector2[] circleVertexList = {
			new Vector2( +0.0000f, +1.0000f ),
			new Vector2( +0.2588f, +0.9659f ),
			new Vector2( +0.5000f, +0.8660f ),
			new Vector2( +0.7071f, +0.7071f ),
			new Vector2( +0.8660f, +0.5000f ),
			new Vector2( +0.9659f, +0.2588f ),
			new Vector2( +1.0000f, +0.0000f ),
			new Vector2( +0.9659f, -0.2588f ),
			new Vector2( +0.8660f, -0.5000f ),
			new Vector2( +0.7071f, -0.7071f ),
			new Vector2( +0.5000f, -0.8660f ),
			new Vector2( +0.2588f, -0.9659f ),
			new Vector2( +0.0000f, -1.0000f ),
			new Vector2( -0.2588f, -0.9659f ),
			new Vector2( -0.5000f, -0.8660f ),
			new Vector2( -0.7071f, -0.7071f ),
			new Vector2( -0.8660f, -0.5000f ),
			new Vector2( -0.9659f, -0.2588f ),
			new Vector2( -1.0000f, -0.0000f ),
			new Vector2( -0.9659f, +0.2588f ),
			new Vector2( -0.8660f, +0.5000f ),
			new Vector2( -0.7071f, +0.7071f ),
			new Vector2( -0.5000f, +0.8660f ),
			new Vector2( -0.2588f, +0.9659f ),
			new Vector2( +0.0000f, +1.0000f )
		};


		public static void DrawCircleGizmo( Vector2 center, float radius )
		{
			var p = (circleVertexList[0] * radius) + center;
			var c = circleVertexList.Length;
			for (int i = 1; i < c; i++)
			{
				Gizmos.DrawLine( p, p = (circleVertexList[i] * radius) + center );
			}
		}


		public static void DrawCircleGizmo( Vector2 center, float radius, Color color )
		{
			Gizmos.color = color;
			DrawCircleGizmo( center, radius );
		}


		public static void DrawOvalGizmo( Vector2 center, Vector2 size )
		{
			var r = size / 2.0f;
			var p = Vector2.Scale( circleVertexList[0], r ) + center;
			var c = circleVertexList.Length;
			for (int i = 1; i < c; i++)
			{
				Gizmos.DrawLine( p, p = Vector2.Scale( circleVertexList[i], r ) + center );
			}
		}


		public static void DrawOvalGizmo( Vector2 center, Vector2 size, Color color )
		{
			Gizmos.color = color;
			DrawOvalGizmo( center, size );
		}


		public static void DrawRectGizmo( Rect rect )
		{
			var p0 = new Vector3( rect.xMin, rect.yMin );
			var p1 = new Vector3( rect.xMax, rect.yMin );
			var p2 = new Vector3( rect.xMax, rect.yMax );
			var p3 = new Vector3( rect.xMin, rect.yMax );
			Gizmos.DrawLine( p0, p1 );
			Gizmos.DrawLine( p1, p2 );
			Gizmos.DrawLine( p2, p3 );
			Gizmos.DrawLine( p3, p0 );
		}


		public static void DrawRectGizmo( Rect rect, Color color )
		{
			Gizmos.color = color;
			DrawRectGizmo( rect );
		}


		public static void DrawRectGizmo( Vector2 center, Vector2 size )
		{
			var hw = size.x / 2.0f;
			var hh = size.y / 2.0f;
			var p0 = new Vector3( center.x - hw, center.y - hh );
			var p1 = new Vector3( center.x + hw, center.y - hh );
			var p2 = new Vector3( center.x + hw, center.y + hh );
			var p3 = new Vector3( center.x - hw, center.y + hh );
			Gizmos.DrawLine( p0, p1 );
			Gizmos.DrawLine( p1, p2 );
			Gizmos.DrawLine( p2, p3 );
			Gizmos.DrawLine( p3, p0 );
		}


		public static void DrawRectGizmo( Vector2 center, Vector2 size, Color color )
		{
			Gizmos.color = color;
			DrawRectGizmo( center, size );
		}


		public static bool GameObjectIsCulledOnCurrentCamera( GameObject gameObject )
		{
			return (Camera.current.cullingMask & (1 << gameObject.layer)) == 0;
		}


		public static Color MoveColorTowards( Color color0, Color color1, float maxDelta )
		{
			var r = Mathf.MoveTowards( color0.r, color1.r, maxDelta );
			var g = Mathf.MoveTowards( color0.g, color1.g, maxDelta );
			var b = Mathf.MoveTowards( color0.b, color1.b, maxDelta );
			var a = Mathf.MoveTowards( color0.a, color1.a, maxDelta );
			return new Color( r, g, b, a );
		}


		public static float ApplyDeadZone( float value, float lowerDeadZone, float upperDeadZone )
		{
			return Mathf.InverseLerp( lowerDeadZone, upperDeadZone, Mathf.Abs( value ) ) * Mathf.Sign( value );
		}


		public static Vector2 ApplyCircularDeadZone( Vector2 axisVector, float lowerDeadZone, float upperDeadZone )
		{
			var magnitude = Mathf.InverseLerp( lowerDeadZone, upperDeadZone, axisVector.magnitude );
			return axisVector.normalized * magnitude;
		}


		public static Vector2 ApplyCircularDeadZone( float axisX, float axisY, float lowerDeadZone, float upperDeadZone )
		{
			return ApplyCircularDeadZone( new Vector2( axisX, axisY ), lowerDeadZone, upperDeadZone );
		}
	}
}

