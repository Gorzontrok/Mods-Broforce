using System.Collections.Generic;
using RocketLib.Menus.Elements;
using RocketLib.Menus.Layout;
using RocketLib.Menus.Utilities;
using RocketLib.Menus.Vanilla;
using UnityEngine;

namespace RocketLib.Menus.Core
{
    public enum FocusRestoreMode
    {
        Default,            // Always return to default/first element
        LastSubmenuTrigger, // Return to element that opened a submenu
        LastFocused         // Return to whatever was last focused
    }

    public class FlexMenu : MonoBehaviour
    {
        protected LayoutContainer rootContainer;
        protected NavigationManager navigationManager;
        protected GridMenuHighlight highlight;

        protected FlexMenu parentFlexMenu;
        protected Menu parentGameMenu;

        private static readonly Dictionary<string, FlexMenu> instances = new Dictionary<string, FlexMenu>();
        private static FlexMenu activeMenu;

        public string InstanceId { get; protected set; } = "default";

        protected bool wasMainMenuLogoVisible;
        protected bool wasVersionTextVisible;
        protected bool hasStoredMainMenuState;
        protected GameObject versionDisplayObject;

        protected float lastInputTime;
        protected bool wasAcceptPressed = false;  // Track if accept was pressed last frame
        protected bool wasDeclinePressed = false; // Track if decline was pressed last frame

        // Focus restoration
        public FocusRestoreMode FocusRestoreMode { get; set; } = FocusRestoreMode.LastSubmenuTrigger;
        protected LayoutElement savedFocusElement;
        protected LayoutElement submenuTriggerElement;

        public LayoutContainer RootContainer
        {
            get { return rootContainer; }
        }

        public NavigationManager Navigation
        {
            get { return navigationManager; }
        }

        public GridMenuHighlight Highlight
        {
            get { return highlight; }
        }


        public virtual string MenuId { get; set; }
        public virtual string MenuTitle { get; set; }

        public bool IsActive { get; protected set; }

        protected virtual void Awake()
        {
            MenuId = GetType().Name;
            MenuTitle = "Flex Menu";

            InitializeContainer();
            navigationManager = new NavigationManager(rootContainer);
        }

        protected virtual void InitializeContainer()
        {
            // Default to vertical layout container
            rootContainer = new VerticalLayoutContainer
            {
                Name = "RootContainer",
                WidthMode = Layout.SizeMode.Fill,
                HeightMode = Layout.SizeMode.Fill,
                Position = Vector2.zero
            };
        }

        protected virtual void CreateHighlight()
        {
            var highlightObj = HighlightFactory.CreateGridHighlight(transform, gameObject.layer);
            if (highlightObj != null)
            {
                highlight = highlightObj.GetComponent<GridMenuHighlight>();
                if (highlight != null)
                {
                    highlight.transform.SetParent(transform, false);
                    highlight.BorderThickness = 3f;
                    highlight.BorderPadding = 2f;

                    // Set initial position to first focused element if available
                    if (navigationManager != null && navigationManager.FocusedElement != null)
                    {
                        var bounds = navigationManager.FocusedElement.GetBounds();
                        // Set position immediately to avoid animation
                        highlight.SetBoundsImmediate(bounds);
                    }
                }
            }
        }

        protected virtual void Start()
        {
            HideMainMenuElements();
            RefreshLayout();
            navigationManager?.FocusFirst();

            // Create highlight after layout and first focus
            CreateHighlight();
        }

        protected virtual void OnEnable()
        {
            IsActive = true;
            lastInputTime = Time.time + 0.2f;  // Add 200ms input delay to prevent click-through
            wasAcceptPressed = false;  // Reset input state
            wasDeclinePressed = false;
            HideMainMenuElements();
            RefreshLayout();

            // Restore focus based on mode
            RestoreFocus();

            // Set highlight position immediately to avoid animation
            if (highlight != null && navigationManager?.FocusedElement != null)
            {
                var bounds = navigationManager.FocusedElement.GetBounds();
                highlight.SetBoundsImmediate(bounds);
            }
        }

        protected virtual void OnDisable()
        {
            IsActive = false;

            // Save the current focused element for potential restoration
            if (navigationManager != null)
            {
                savedFocusElement = navigationManager.FocusedElement;
            }

            // Only restore main menu elements when going back (activeMenu changed)
            // During forward transitions, activeMenu still equals this
            if (activeMenu != this)
            {
                RestoreMainMenuElements();
            }
        }

        protected virtual void OnDestroy()
        {
            RestoreMainMenuElements();
            navigationManager?.Clear();

            string key = $"{GetType().Name}:{InstanceId}";
            instances.Remove(key);
            if (activeMenu == this)
            {
                activeMenu = null;
            }
        }

        public virtual void RefreshLayout()
        {
            if (rootContainer != null)
            {
                // Pass our transform to the root container
                rootContainer.SetMenuTransform(transform);
                rootContainer.UpdateLayout();
                navigationManager?.RefreshFocusableElements();
            }
        }

        protected virtual void Update()
        {
            if (!IsActive) return;

            HandleInput();
            UpdateHighlight();
            UpdateScrollContainers();
        }

        protected virtual void OnGUI()
        {
            // Check both IsActive and GameObject active state
            if (!IsActive || !gameObject.activeSelf)
            {
                return;
            }

            if (rootContainer != null)
            {
                rootContainer.Render();
            }
        }

        protected virtual void HandleInput()
        {
            // Use Broforce's InputReader for exact menu behavior
            bool up = false, down = false, left = false, right = false;
            bool accept = false, decline = false;

            // Try keyboard first, then combined input (matches Broforce's Menu.CheckInput)
            if (!InputReader.GetMenuInputStandardKeys(ref up, ref down, ref left, ref right, ref accept, ref decline))
            {
                InputReader.GetMenuInputCombined(ref up, ref down, ref left, ref right, ref accept, ref decline, 0);
            }

            // Prevent any input during initial cooldown period
            if (Time.time < lastInputTime)
            {
                return;  // Still in cooldown after menu activation
            }

            // Check timing like Broforce does (200ms for directional, 50ms for accept/decline)
            if (up || down || left || right)
            {
                if (Time.time - lastInputTime < 0.2f)  // upDownDelay
                {
                    return; // Too soon, ignore input
                }
                lastInputTime = Time.time;
            }

            if (accept || decline)
            {
                if (Time.time - lastInputTime < 0.05f)
                {
                    return; // Too soon, ignore input
                }
                lastInputTime = Time.time;
            }

            // Track button press/release state for toggles
            bool acceptJustPressed = accept && !wasAcceptPressed;
            bool declineJustPressed = decline && !wasDeclinePressed;

            // Process navigation - only process activate on first press
            if (up || down || left || right || acceptJustPressed)
            {
                navigationManager?.ProcessInput(
                    left ? -1f : (right ? 1f : 0f),
                    up ? 1f : (down ? -1f : 0f),
                    acceptJustPressed,  // Only activate on initial press, not hold
                    declineJustPressed
                );
            }

            if (declineJustPressed)
            {
                GoBack();
            }

            // Update state for next frame
            wasAcceptPressed = accept;
            wasDeclinePressed = decline;
        }

        protected virtual void RestoreFocus()
        {
            if (navigationManager == null) return;

            LayoutElement elementToFocus = null;

            switch (FocusRestoreMode)
            {
                case FocusRestoreMode.LastSubmenuTrigger:
                    // Try to restore to the element that opened a submenu
                    if (submenuTriggerElement != null && navigationManager.IsElementFocusable(submenuTriggerElement))
                    {
                        elementToFocus = submenuTriggerElement;
                    }
                    break;

                case FocusRestoreMode.LastFocused:
                    // Try to restore to the last focused element
                    if (savedFocusElement != null && navigationManager.IsElementFocusable(savedFocusElement))
                    {
                        elementToFocus = savedFocusElement;
                    }
                    break;

                case FocusRestoreMode.Default:
                    // Always use default (first element)
                    break;
            }

            // Focus the chosen element or fall back to first
            if (elementToFocus != null)
            {
                navigationManager.FocusElement(elementToFocus);
            }
            else
            {
                navigationManager.FocusFirst();
            }
        }

        protected virtual void UpdateHighlight()
        {
            if (highlight == null || navigationManager == null) return;

            var focused = navigationManager.FocusedElement;
            if (focused != null)
            {
                var bounds = focused.GetBounds();
                highlight.SetTargetBounds(bounds);

                if (!highlight.gameObject.activeSelf)
                {
                    highlight.gameObject.SetActive(true);
                }
            }
            else if (highlight.gameObject.activeSelf)
            {
                highlight.gameObject.SetActive(false);
            }
        }

        private void UpdateScrollContainers()
        {
            // Find and update any ScrollContainers in the hierarchy
            UpdateScrollContainersRecursive(rootContainer);
        }

        private void UpdateScrollContainersRecursive(LayoutElement element)
        {
            if (element == null) return;

            // If this is a ScrollContainer, call its Update method
            if (element is ScrollContainer scrollContainer)
            {
                scrollContainer.Update();
            }

            // Recursively check children if this is a container
            if (element is LayoutContainer container)
            {
                foreach (var child in container.Children)
                {
                    UpdateScrollContainersRecursive(child);
                }
            }
        }

        public virtual void GoBack()
        {

            RestoreMainMenuElements();

            if (parentFlexMenu != null)
            {
                parentFlexMenu.gameObject.SetActive(true);
                activeMenu = parentFlexMenu;
                gameObject.SetActive(false);
            }
            else if (parentGameMenu != null)
            {
                parentGameMenu.gameObject.SetActive(true);
                activeMenu = null;
                gameObject.SetActive(false);
            }
            else
            {
                var allMenus = FindObjectsOfType<Menu>();
                foreach (var menu in allMenus)
                {
                    if (menu.name == "MainMenu")
                    {
                        menu.gameObject.SetActive(true);
                        break;
                    }
                }
                activeMenu = null;
                gameObject.SetActive(false);
            }
        }

        protected virtual void HideMainMenuElements()
        {
            // Only store the state once, the first time we hide
            if (!hasStoredMainMenuState)
            {
                var mainMenu = GetMainMenu() as MainMenu;
                if (mainMenu == null)
                {
                    return;
                }

                // Store the original state of the logo
                if (mainMenu.logo != null)
                {
                    wasMainMenuLogoVisible = mainMenu.logo.gameObject.activeSelf;
                }

                // Store the original state of the version display
                var versionDisplay = GameObject.FindObjectOfType<DisplayVersionNumber>();
                if (versionDisplay != null)
                {
                    versionDisplayObject = versionDisplay.gameObject;  // Store reference
                    wasVersionTextVisible = versionDisplayObject.activeSelf;
                }

                hasStoredMainMenuState = true;
            }

            // Always hide the elements (even if we already stored the state)
            var menu = GetMainMenu() as MainMenu;
            if (menu != null && menu.logo != null)
            {
                menu.logo.gameObject.SetActive(false);
            }

            var version = GameObject.FindObjectOfType<DisplayVersionNumber>();
            if (version != null)
            {
                version.gameObject.SetActive(false);
            }
        }

        protected virtual void RestoreMainMenuElements()
        {
            var mainMenu = GetMainMenu() as MainMenu;
            if (mainMenu == null)
            {
                return;
            }

            // Restore the logo GameObject directly (like ComplexMenu does)
            if (mainMenu.logo != null)
            {
                mainMenu.logo.gameObject.SetActive(wasMainMenuLogoVisible);
            }

            // Restore the version number display using stored reference
            if (versionDisplayObject != null)
            {
                versionDisplayObject.SetActive(wasVersionTextVisible);
            }
            else
            {
                // Fallback to finding it if we don't have the reference
                var versionDisplay = GameObject.FindObjectOfType<DisplayVersionNumber>();
                if (versionDisplay != null)
                {
                    versionDisplay.gameObject.SetActive(wasVersionTextVisible);
                }
            }
        }

        protected Menu GetMainMenu()
        {
            Menu current = parentGameMenu;
            while (current != null)
            {
                if (current.name == "MainMenu")
                {
                    return current;
                }

                var customMenu = current as BaseCustomMenu;
                if (customMenu != null)
                {
                    var parentField = typeof(BaseCustomMenu).GetField("parentMenu", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (parentField != null)
                    {
                        current = parentField.GetValue(customMenu) as Menu;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            var allMenus = FindObjectsOfType<Menu>();
            foreach (var menu in allMenus)
            {
                if (menu.name == "MainMenu")
                {
                    return menu;
                }
            }

            return null;
        }

        public virtual T AddElement<T>(LayoutElement parent = null) where T : LayoutElement, new()
        {
            var element = new T();
            var container = (parent as LayoutContainer) ?? rootContainer;
            container?.AddChild(element);
            return element;
        }

        public virtual LayoutContainer AddContainer(LayoutElement parent = null, LayoutMode mode = LayoutMode.Vertical)
        {
            var container = LayoutContainer.Create(mode);
            var parentContainer = (parent as LayoutContainer) ?? rootContainer;
            parentContainer?.AddChild(container);
            return container;
        }

        public static T Show<T>(FlexMenu parentFlex = null, Menu parentGame = null, string instanceId = "default") where T : FlexMenu
        {
            var type = typeof(T);
            string key = $"{type.Name}:{instanceId}";

            if (instances.TryGetValue(key, out FlexMenu existing))
            {
                T menu = existing as T;
                menu.parentFlexMenu = parentFlex;
                menu.parentGameMenu = parentGame;

                if (parentFlex != null)
                {
                    // Save the parent's currently focused element as the submenu trigger
                    if (parentFlex.navigationManager != null)
                    {
                        parentFlex.submenuTriggerElement = parentFlex.navigationManager.FocusedElement;
                    }
                    parentFlex.gameObject.SetActive(false);
                }
                else if (parentGame != null)
                {
                    parentGame.gameObject.SetActive(false);
                }

                if (activeMenu != null && activeMenu != menu)
                {
                    activeMenu.gameObject.SetActive(false);
                }

                menu.gameObject.SetActive(true);
                activeMenu = menu;

                return menu;
            }

            string goName = instanceId == "default" ? type.Name : $"{type.Name}_{instanceId}";
            var go = new GameObject(goName);
            var newMenu = go.AddComponent<T>();
            newMenu.InstanceId = instanceId;
            instances[key] = newMenu;
            newMenu.parentFlexMenu = parentFlex;
            newMenu.parentGameMenu = parentGame;

            if (parentFlex != null)
            {
                // Save the parent's currently focused element as the submenu trigger
                if (parentFlex.navigationManager != null)
                {
                    parentFlex.submenuTriggerElement = parentFlex.navigationManager.FocusedElement;
                }
                parentFlex.gameObject.SetActive(false);
            }
            else if (parentGame != null)
            {
                parentGame.gameObject.SetActive(false);
            }

            if (activeMenu != null)
            {
                activeMenu.gameObject.SetActive(false);
            }

            newMenu.gameObject.SetActive(true);
            activeMenu = newMenu;

            return newMenu;
        }
    }
}