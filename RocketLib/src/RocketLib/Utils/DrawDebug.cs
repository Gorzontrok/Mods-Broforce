using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RocketLib.Utils
{
    public static class DrawDebug
    {
        public static Dictionary<string, LineRenderer> lines = new Dictionary<string, LineRenderer> ();

        /// <summary>
        /// Draw a debug line
        /// </summary>
        /// <param name="ID">Name of line, use the same name to update an existing line</param>
        /// <param name="start">Start position</param>
        /// <param name="end">End position</param>
        /// <param name="color">Color of line</param>
        /// <param name="width">Width of line</param>
        public static void DrawLine(string ID, Vector3 start, Vector3 end, Color color, float width = 0.3f)
        {
            LineRenderer line;
            if (!lines.TryGetValue(ID, out line) || line == null)
            {
                line = new GameObject("DebugLine", new Type[] { typeof(Transform), typeof(LineRenderer) }).GetComponent<LineRenderer>();
                line.material = new Material(Shader.Find("Sprites/Default"));
                line.positionCount = 2;
                line.gameObject.layer = 28;
                if ( lines.ContainsKey(ID) )
                {
                    lines[ID] = line;
                }
                else
                {
                    lines.Add(ID, line);
                }
            }

            line.startColor = color;
            line.endColor = color;
            line.SetPosition(0, start);
            line.SetPosition(1, end);
            line.startWidth = width;
            line.endWidth = width;
        }

        /// <summary>
        /// Draw a debug rectangle
        /// </summary>
        /// <param name="ID">Name of rectangle, use the same name to update an existing rectangle</param>
        /// <param name="corner1">One of the corners</param>
        /// <param name="corner2">The opposite corner of the first one provided</param>
        /// <param name="color">Color of rectangle</param>
        /// <param name="width">Width of rectangle</param>
        public static void DrawRectangle(string ID, Vector3 corner1, Vector3 corner2, Color color, float width = 0.3f)
        {
            LineRenderer line;
            if (!lines.TryGetValue(ID, out line) || line == null)
            {
                line = new GameObject("DebugLine", new Type[] { typeof(Transform), typeof(LineRenderer) }).GetComponent<LineRenderer>();
                line.material = new Material(Shader.Find("Sprites/Default"));
                line.positionCount = 5;
                line.gameObject.layer = 28;
                if (lines.ContainsKey(ID))
                {
                    lines[ID] = line;
                }
                else
                {
                    lines.Add(ID, line);
                }
            }

            line.startColor = color;
            line.endColor = color;
            line.SetPosition(0, corner1);
            line.SetPosition(1, new Vector3(corner1.x, corner2.y, corner1.z));
            line.SetPosition(2, corner2);
            line.SetPosition(3, new Vector3(corner2.x, corner1.y, corner1.z));
            line.SetPosition(4, corner1);
            line.startWidth = width;
            line.endWidth = width;
        }
    }
}
