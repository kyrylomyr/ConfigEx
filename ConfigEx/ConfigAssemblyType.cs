namespace ConfigEx
{
    /// <summary>
    /// Defines the type of config Assembly.
    /// </summary>
    public enum ConfigAssemblyType
    {
        /// <summary>
        /// The App.config at the same location where the Assembly is located itself will be used.
        /// </summary>
        ClassLibrary,

        /// <summary>
        /// The Web.config will be used.
        /// </summary>
        WebMainAssembly
    }
}
