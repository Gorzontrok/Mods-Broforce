using System.Collections.Generic;
using RocketLib.Menus.Elements;
using UnityEngine;

namespace RocketLib.Menus.Layout
{
    /// <summary>
    /// Container that arranges children in a grid with specified rows and columns
    /// </summary>
    public class GridLayoutContainer : LayoutContainer
    {
        public int Columns { get; set; } = 2;
        public int Rows { get; set; } = 0; // 0 = auto calculate based on children count

        public float ColumnSpacing { get; set; } = 10f;
        public float RowSpacing { get; set; } = 10f;

        public HorizontalAlignment CellHorizontalAlignment { get; set; } = HorizontalAlignment.Center;
        public VerticalAlignment CellVerticalAlignment { get; set; } = VerticalAlignment.Center;

        public GridLayoutContainer(string name = "GridContainer") : base(name)
        {
        }

        protected override void ArrangeChildren()
        {
            if (Children.Count == 0) return;

            var childrenToPosition = this.GetChildrenToPosition();
            if (childrenToPosition.Count == 0) return;

            float availableWidth = ActualSize.x - (Padding * 2);
            float availableHeight = ActualSize.y - (Padding * 2);

            int actualColumns = Columns > 0 ? Columns : 1;
            int actualRows = Rows;

            if (actualRows == 0)
            {
                actualRows = (int)Mathf.Ceil((float)childrenToPosition.Count / actualColumns);
            }

            float totalColumnSpacing = ColumnSpacing * Mathf.Max(0, actualColumns - 1);
            float totalRowSpacing = RowSpacing * Mathf.Max(0, actualRows - 1);

            float cellWidth = (availableWidth - totalColumnSpacing) / actualColumns;
            float cellHeight = (availableHeight - totalRowSpacing) / actualRows;

            float containerLeft = ActualPosition.x - (ActualSize.x / 2);
            float containerTop = ActualPosition.y + (ActualSize.y / 2);

            for (int i = 0; i < childrenToPosition.Count; i++)
            {
                var child = childrenToPosition[i];

                int column = i % actualColumns;
                int row = i / actualColumns;

                float cellCenterX = containerLeft + Padding + (column * (cellWidth + ColumnSpacing)) + (cellWidth / 2);
                float cellCenterY = containerTop - Padding - (row * (cellHeight + RowSpacing)) - (cellHeight / 2);

                float childWidth = cellWidth;
                float childHeight = cellHeight;

                switch (child.WidthMode)
                {
                    case SizeMode.Fixed:
                        childWidth = Mathf.Min(child.Width, cellWidth);
                        break;
                    case SizeMode.Percentage:
                        childWidth = (child.Width / 100f) * cellWidth;
                        break;

                    case SizeMode.Fill:
                        break;
                }

                switch (child.HeightMode)
                {
                    case SizeMode.Fixed:
                        childHeight = Mathf.Min(child.Height, cellHeight);
                        break;
                    case SizeMode.Percentage:
                        childHeight = (child.Height / 100f) * cellHeight;
                        break;

                    case SizeMode.Fill:
                        break;
                }

                if (child.MinSize.x > 0) childWidth = Mathf.Max(childWidth, child.MinSize.x);
                if (child.MaxSize.x > 0) childWidth = Mathf.Min(childWidth, child.MaxSize.x);
                if (child.MinSize.y > 0) childHeight = Mathf.Max(childHeight, child.MinSize.y);
                if (child.MaxSize.y > 0) childHeight = Mathf.Min(childHeight, child.MaxSize.y);

                childWidth = Mathf.Min(childWidth, cellWidth);
                childHeight = Mathf.Min(childHeight, cellHeight);

                HorizontalAlignment hAlign = child.HorizontalAlignmentOverride ?? CellHorizontalAlignment;
                VerticalAlignment vAlign = child.VerticalAlignmentOverride ?? CellVerticalAlignment;

                float childX = cellCenterX;
                switch (hAlign)
                {
                    case HorizontalAlignment.Left:
                        childX = cellCenterX - (cellWidth / 2) + (childWidth / 2);
                        break;
                    case HorizontalAlignment.Right:
                        childX = cellCenterX + (cellWidth / 2) - (childWidth / 2);
                        break;
                }

                float childY = cellCenterY;
                switch (vAlign)
                {
                    case VerticalAlignment.Top:
                        childY = cellCenterY + (cellHeight / 2) - (childHeight / 2);
                        break;
                    case VerticalAlignment.Bottom:
                        childY = cellCenterY - (cellHeight / 2) + (childHeight / 2);
                        break;
                }

                child.ActualPosition = new Vector2(childX, childY);
                child.ActualSize = new Vector2(childWidth, childHeight);

            }

            DetectOverflow(childrenToPosition, actualColumns, actualRows);
        }

        private void DetectOverflow(List<LayoutElement> childrenToPosition, int actualColumns, int actualRows)
        {
            if (childrenToPosition.Count == 0) return;

            // Get actual camera bounds dynamically
            float screenTop, screenBottom, screenLeft, screenRight;
            GetCameraBounds(out screenTop, out screenBottom, out screenLeft, out screenRight);

            // Check for off-screen elements
            List<string> offScreenElements = new List<string>();
            int totalCells = actualColumns * actualRows;
            int overflow = childrenToPosition.Count - totalCells;

            // Check visible children for off-screen rendering
            int maxIndex = Mathf.Min(childrenToPosition.Count, totalCells);
            for (int i = 0; i < maxIndex; i++)
            {
                var child = childrenToPosition[i];
                float childTop = child.ActualPosition.y + (child.ActualSize.y / 2);
                float childBottom = child.ActualPosition.y - (child.ActualSize.y / 2);

                if (childTop > screenTop || childBottom < screenBottom)
                {
                    string childName = string.IsNullOrEmpty(child.Name) ? child.GetType().Name : child.Name;
                    string issue = "";
                    if (childTop > screenTop) issue = $"top {childTop:F0} > screen {screenTop:F0}";
                    if (childBottom < screenBottom)
                    {
                        if (!string.IsNullOrEmpty(issue)) issue += ", ";
                        issue += $"bottom {childBottom:F0} < screen {screenBottom:F0}";
                    }
                    offScreenElements.Add($"{child.GetType().Name} '{childName}' ({issue})");
                }
            }

            // Log based on severity
            if (offScreenElements.Count > 0)
            {
                // CRITICAL: Elements are off-screen
                RocketLib.Main.logger.Error($"[ERROR] GridLayoutContainer '{Name}' has OFF-SCREEN elements!");
                foreach (string element in offScreenElements)
                {
                    RocketLib.Main.logger.Error($"  - {element}");
                }
            }

            if (overflow > 0)
            {
                // WARNING: Too many children for grid
                RocketLib.Main.logger.Warning($"[WARNING] GridLayoutContainer '{Name}' has more children than grid cells!");
                RocketLib.Main.logger.Warning($"  - Grid: {actualColumns}x{actualRows} = {totalCells} cells");
                RocketLib.Main.logger.Warning($"  - Children: {childrenToPosition.Count} ({overflow} won't be displayed)");

                // List hidden children
                List<string> overflowChildren = new List<string>();
                for (int i = totalCells; i < childrenToPosition.Count; i++)
                {
                    var child = childrenToPosition[i];
                    string childName = string.IsNullOrEmpty(child.Name) ? child.GetType().Name : child.Name;
                    overflowChildren.Add($"{child.GetType().Name} '{childName}'");
                }
                if (overflowChildren.Count > 0)
                {
                    RocketLib.Main.logger.Warning($"  - Hidden: {string.Join(", ", overflowChildren.ToArray())}");
                }
            }
        }




    }
}
