using System;
using System.Collections.Generic;
using UnityEngine;

namespace RocketLib.Utils
{
    public static class DrawDebug
    {
        public static Dictionary<string, LineRenderer> lines = new Dictionary<string, LineRenderer>();

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
            LineRenderer line = CreateLine(ID, 2);

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
            LineRenderer line = CreateLine(ID, 5);

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

        /// <summary>
        /// Draw a debug crosshair
        /// </summary>
        /// <param name="ID">Name of crosshair, use the same name to update an existing crosshair</param>
        /// <param name="center">Center of the crosshair</param>
        /// <param name="length">Length of each line</param>
        /// <param name="color">Color of crosshair</param>
        /// <param name="width">Width of crosshair</param>
        public static void DrawCrosshair(string ID, Vector3 center, float length, Color color, float width = 0.3f)
        {
            LineRenderer line1 = CreateLine(ID + "1", 2);
            LineRenderer line2 = CreateLine(ID + "2", 2);

            line1.startColor = line2.startColor = line1.endColor = line2.endColor = color;
            line1.SetPosition(0, new Vector3(center.x - length, center.y, center.z));
            line1.SetPosition(1, new Vector3(center.x + length, center.y, center.z));
            line2.SetPosition(0, new Vector3(center.x, center.y - length, center.z));
            line2.SetPosition(1, new Vector3(center.x, center.y + length, center.z));
            line1.startWidth = line2.startWidth = line1.endWidth = line2.endWidth = width;
        }

        private static LineRenderer CreateLine(string ID, int positionCount)
        {
            LineRenderer line;
            if (!lines.TryGetValue(ID, out line) || line == null)
            {
                line = new GameObject("DebugLine", new Type[] { typeof(Transform), typeof(LineRenderer) }).GetComponent<LineRenderer>();
                line.material = new Material(Shader.Find("Sprites/Default"));
                line.positionCount = positionCount;
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
            return line;
        }
    }
}
