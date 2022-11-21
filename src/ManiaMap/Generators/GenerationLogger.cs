using System;

namespace MPewsey.ManiaMap.Generators
{
    /// <summary>
    /// A singleton class for handling generation logging events.
    /// </summary>
    public class GenerationLogger
    {
        /// <summary>
        /// The logger singleton.
        /// </summary>
        private static GenerationLogger Current { get; set; } = new GenerationLogger();

        /// <summary>
        /// The logging event.
        /// </summary>
        private event Action<string> LogEvent;

        /// <summary>
        /// Instantiates a new logger.
        /// </summary>
        private GenerationLogger()
        {

        }

        /// <summary>
        /// Sends the message via the logging event.
        /// </summary>
        /// <param name="message">The log message.</param>
        public static void Log(string message)
        {
            Current.LogEvent?.Invoke(message);
        }

        /// <summary>
        /// Adds the listener to the logging event.
        /// </summary>
        /// <param name="action">The delegate to register.</param>
        public static void AddListener(Action<string> action)
        {
            Current.LogEvent += action;
        }

        /// <summary>
        /// Removes the listener from the logging event.
        /// </summary>
        /// <param name="action">The delegate to remove.</param>
        public static void RemoveListener(Action<string> action)
        {
            Current.LogEvent -= action;
        }

        /// <summary>
        /// Removes all listeners from the logger.
        /// </summary>
        public static void RemoveAllListeners()
        {
            Current = new GenerationLogger();
        }
    }
}
