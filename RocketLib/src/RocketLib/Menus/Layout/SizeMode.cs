namespace RocketLib.Menus.Layout
{
    /// <summary>
    /// Specifies how a LayoutElement's size should be calculated
    /// </summary>
    public enum SizeMode
    {
        /// <summary>
        /// Width/Height value is exact size in world units
        /// </summary>
        Fixed,

        /// <summary>
        /// Width/Height value is percentage (0.0 to 1.0) of parent container
        /// </summary>
        Percentage,

        /// <summary>
        /// Element takes all remaining space after other children are sized
        /// </summary>
        Fill,

        /// <summary>
        /// Element automatically sizes to fit its content (only supported by some elements)
        /// </summary>
        Auto
    }
}