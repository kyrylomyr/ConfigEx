namespace ConfigEx
{
    /// <summary>
    /// Defines the type of config assembly.
    /// </summary>
    public enum ConfigAssemblyType
    {
        /// <summary>
        /// The App.config of the test project assembly will be used.
        /// </summary>
        TestMainAssembly,

        /// <summary>
        /// The Web.config of the Web project assembly will be used.
        /// </summary>
        WebMainAssembly
    }
}
