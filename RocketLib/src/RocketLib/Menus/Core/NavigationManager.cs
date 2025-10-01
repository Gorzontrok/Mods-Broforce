using System.Collections.Generic;
using System.Linq;
using RocketLib.Menus.Elements;
using UnityEngine;

namespace RocketLib.Menus.Core
{
    public class NavigationManager
    {
        private LayoutElement rootElement;
        private LayoutElement focusedElement;
        private List<LayoutElement> focusableElements;
        private bool wrapNavigation;

        public LayoutElement FocusedElement
        {
            get { return focusedElement; }
            set { SetFocus(value); }
        }

        public bool WrapNavigation
        {
            get { return wrapNavigation; }
            set { wrapNavigation = value; }
        }

        public NavigationManager(LayoutElement root)
        {
            rootElement = root;
            wrapNavigation = true;
            RefreshFocusableElements();
        }

        public void RefreshFocusableElements()
        {
            if (rootElement == null)
            {
                focusableElements = new List<LayoutElement>();
                return;
            }

            focusableElements = rootElement.GetFocusableElements();
            focusableElements.Sort((a, b) =>
            {
                int yCompare = b.ActualPosition.y.CompareTo(a.ActualPosition.y);
                if (yCompare != 0) return yCompare;
                return a.ActualPosition.x.CompareTo(b.ActualPosition.x);
            });

            if (focusedElement == null || !focusableElements.Contains(focusedElement))
            {
                FocusFirst();
            }
        }

        public void FocusFirst()
        {
            if (focusableElements != null && focusableElements.Count > 0)
            {
                SetFocus(focusableElements[0]);
            }
            else
            {
                SetFocus(null);
            }
        }

        public void FocusLast()
        {
            if (focusableElements != null && focusableElements.Count > 0)
            {
                SetFocus(focusableElements[focusableElements.Count - 1]);
            }
            else
            {
                SetFocus(null);
            }
        }

        public bool IsElementFocusable(LayoutElement element)
        {
            return focusableElements != null && focusableElements.Contains(element);
        }

        public void FocusElement(LayoutElement element)
        {
            if (IsElementFocusable(element))
            {
                SetFocus(element);
            }
        }

        private void SetFocus(LayoutElement element)
        {
            if (focusedElement == element) return;

            if (focusedElement != null)
            {
                focusedElement.OnFocusLost();
            }

            focusedElement = element;

            if (focusedElement != null)
            {
                focusedElement.OnFocusGained();
            }
        }

        public bool ProcessInput(float horizontal, float vertical, bool activate, bool back)
        {
            // Timing is handled by FlexMenu using Broforce's system
            // We just process the input immediately here

            bool handled = false;

            if (Mathf.Abs(horizontal) > 0.5f)
            {
                if (horizontal > 0)
                {
                    handled = NavigateRight();
                }
                else
                {
                    handled = NavigateLeft();
                }
            }
            else if (Mathf.Abs(vertical) > 0.5f)
            {
                if (vertical > 0)
                {
                    handled = NavigateUp();
                }
                else
                {
                    handled = NavigateDown();
                }
            }
            else if (activate && focusedElement != null)
            {
                focusedElement.OnActivated();
                handled = true;
            }

            return handled;
        }

        public bool NavigateUp()
        {
            return NavigateInDirection(Vector2.up);
        }

        public bool NavigateDown()
        {
            return NavigateInDirection(Vector2.down);
        }

        public bool NavigateLeft()
        {
            return NavigateInDirection(Vector2.left);
        }

        public bool NavigateRight()
        {
            return NavigateInDirection(Vector2.right);
        }

        private bool NavigateInDirection(Vector2 direction)
        {
            if (focusedElement == null || focusableElements == null || focusableElements.Count <= 1)
            {
                return false;
            }

            var currentPos = focusedElement.ActualPosition;
            LayoutElement bestCandidate = null;
            float bestScore = float.MaxValue;

            foreach (var element in focusableElements)
            {
                if (element == focusedElement) continue;

                var elementPos = element.ActualPosition;
                var toElement = elementPos - currentPos;

                float dot = Vector2.Dot(toElement.normalized, direction);
                if (dot <= 0.1f) continue;

                float distance = toElement.magnitude;
                float angle = Mathf.Acos(dot);
                float score = distance * (1f + angle * 2f);

                if (score < bestScore)
                {
                    bestScore = score;
                    bestCandidate = element;
                }
            }

            if (bestCandidate == null && wrapNavigation)
            {
                bestCandidate = FindWrapCandidate(direction);
            }

            if (bestCandidate != null)
            {
                SetFocus(bestCandidate);
                return true;
            }

            return false;
        }

        private LayoutElement FindWrapCandidate(Vector2 direction)
        {
            if (focusableElements == null || focusableElements.Count == 0) return null;

            if (direction == Vector2.up)
            {
                return focusableElements
                    .Where(e => e != focusedElement)
                    .OrderBy(e => e.ActualPosition.y)
                    .ThenBy(e => Mathf.Abs(e.ActualPosition.x - focusedElement.ActualPosition.x))
                    .FirstOrDefault();
            }
            else if (direction == Vector2.down)
            {
                return focusableElements
                    .Where(e => e != focusedElement)
                    .OrderByDescending(e => e.ActualPosition.y)
                    .ThenBy(e => Mathf.Abs(e.ActualPosition.x - focusedElement.ActualPosition.x))
                    .FirstOrDefault();
            }
            else if (direction == Vector2.left)
            {
                return focusableElements
                    .Where(e => e != focusedElement)
                    .OrderByDescending(e => e.ActualPosition.x)
                    .ThenBy(e => Mathf.Abs(e.ActualPosition.y - focusedElement.ActualPosition.y))
                    .FirstOrDefault();
            }
            else if (direction == Vector2.right)
            {
                return focusableElements
                    .Where(e => e != focusedElement)
                    .OrderBy(e => e.ActualPosition.x)
                    .ThenBy(e => Mathf.Abs(e.ActualPosition.y - focusedElement.ActualPosition.y))
                    .FirstOrDefault();
            }

            return null;
        }

        public void NavigateToElement(LayoutElement element)
        {
            if (element != null && focusableElements != null && focusableElements.Contains(element))
            {
                SetFocus(element);
            }
        }

        public void NavigateByIndex(int index)
        {
            if (focusableElements != null && index >= 0 && index < focusableElements.Count)
            {
                SetFocus(focusableElements[index]);
            }
        }

        public int GetFocusedIndex()
        {
            if (focusedElement == null || focusableElements == null)
            {
                return -1;
            }

            return focusableElements.IndexOf(focusedElement);
        }

        public void Clear()
        {
            SetFocus(null);
            focusableElements = null;
            rootElement = null;
        }
    }
}
