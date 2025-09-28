using RocketLib.Menus.Layout;

namespace RocketLib.Menus.Elements
{
    /// <summary>
    /// A lightweight invisible element used for spacing in layouts
    /// </summary>
    public class SpacerElement : LayoutElement
    {
        public SpacerElement(string name = "Spacer") : base(name)
        {
            IsVisible = false;
            IsFocusable = false;
            WidthMode = SizeMode.Fill;
            HeightMode = SizeMode.Fill;
        }

        public override void Render()
        {
            // Spacers don't render anything
        }

        public override void Cleanup()
        {
            // Nothing to cleanup for spacers
            base.Cleanup();
        }
    }
}