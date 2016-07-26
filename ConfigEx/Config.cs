﻿namespace ConfigEx
{
    /// <summary>
    /// Provides readers for main and local Assembly configurations.
    /// </summary>
    public sealed class Config
    {
        static Config()
        {
            Main = new DefaultConfigReader(new MainConfigProvider());
            Local = new DefaultConfigReader(new LocalConfigProvider());
        }

        /// <summary>
        /// The reader that reads values from the Entry Assembly configuration.
        /// </summary>
        public static IConfigReader Main { get; set; }

        /// <summary>
        /// The reader that reads values from the Calling Assembly configuration.
        /// </summary>
        public static IConfigReader Local { get; set; }
    }
}