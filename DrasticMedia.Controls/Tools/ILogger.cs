// <copyright file="ILogger.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DrasticMedia.Core
{
    // in order of severity..
    public enum LogLevel
    {
        /// <summary>
        /// Used for filtering only; All messages are logged.
        /// </summary>
        All, // must be first

        /// <summary>
        /// Informational messages used for debugging or to trace code execution.
        /// </summary>
        Debug,

        /// <summary>
        /// Informational messages containing performance metrics.
        /// </summary>
        Perf,

        /// <summary>
        /// Informational messages that might be of interest to the user.
        /// </summary>
        Info,

        /// <summary>
        /// Warnings.
        /// </summary>
        Warn,

        /// <summary>
        /// Errors that are handled gracefully.
        /// </summary>
        Error,

        /// <summary>
        /// Errors that are not handled gracefully.
        /// </summary>
        Fail,

        /// <summary>
        /// Used for filtering only; No messages are logged.
        /// </summary>
        None, // must be last
    }

    [Serializable]
    public sealed class LogMessage
    {
        public const string TimestampFormat = "yyyy-MM-dd HH:mm:ss.f";

        public DateTime Timestamp { get; }
        public LogLevel Level { get; }
        public string Message { get; private set; }

        public LogMessage(DateTime timestamp, LogLevel level, string message)
        {
            if (level <= LogLevel.All || level >= LogLevel.None)
                throw new ArgumentException("Invalid log level", nameof(level));
            if (message is null)
                throw new ArgumentNullException(nameof(message));
            Timestamp = timestamp;
            Level = level;
            Message = message;
        }

        public LogMessage WithMessage(string message)
        {
            var result = (LogMessage)MemberwiseClone();
            result.Message = message;
            return result;
        }

        public override string ToString()
            => $"[DrasticMedia] ({Timestamp.ToString(TimestampFormat)}): {Level.ToString().ToUpperInvariant()}: {Message}";

        /// <summary>
        /// Return message appropriate for the output pad/pane, which is more visible to the end user (but still technical).
        /// Here, we want to have somewhat concise output, minimzing horizontal scrolling.
        /// </summary>
        public string ToOutputPaneString()
        {
            // In the output pane, only show time, not date, to make it more concise.
            // Use the locale specific long time format (e.g. "1:45:30 PM" for en-US)
            String timestamp = Timestamp.ToString("T");

            return $"[{timestamp}]  {Message}";
        }
    }

    public interface ILogger
    {
        void Log(LogMessage message);
    }

    public static class Logger
    {
        public static void Log(this ILogger logger, LogLevel level, string? message)
            => logger.Log(new LogMessage(DateTime.Now, level, message ?? string.Empty));

        public static void Log(this ILogger logger,
            Exception ex,
            LogLevel level = LogLevel.Error,
            [CallerMemberName] string memberName = "(unknown)",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            logger.Log(level, $"Caught exception in {memberName} at {sourceLineNumber}: {ex}\n{ex.StackTrace}");
        }

        public static void LogIfFaulted(this Task task,
            ILogger logger,
            LogLevel level = LogLevel.Error,
            [CallerMemberName] string memberName = "(unknown)",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            task.ContinueWith(t => logger.Log(t.Exception, level, memberName, sourceLineNumber),
                TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
        }

        public static void Log(this ILogger logger,
            Stopwatch sw,
            LogLevel level = LogLevel.Perf,
            [CallerMemberName] string memberName = "(unknown)",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            logger.Log(level, $"Elapsed time in {memberName} at {sourceLineNumber}: {sw.ElapsedMilliseconds}ms");
        }

        /// <summary>
        /// Returns a new <see cref="ILogger"/> that prefixes every message with
        ///  parenthesis and the given tag.
        /// </summary>
        public static ILogger WithTag(this ILogger logger, string tag, bool includeTagInUserVisibleMessages = false)
            => new TaggedLogger(logger, tag, includeTagInUserVisibleMessages);
    }

    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Sets the minimum <see cref="LogLevel"/> for this logger.
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.All;

        public virtual void Log(LogMessage message)
        {
            if (message.Level < LogLevel)
                return;

            Console.WriteLine(message.Message);
        }
    }

    class TaggedLogger : ILogger
    {
        ILogger logger;
        string tag;
        bool includeTagInUserVisibileMessages;

        public TaggedLogger(ILogger logger, string tag, bool includeTagInUserVisibileMessages)
        {
            this.logger = logger;
            this.tag = tag;
            this.includeTagInUserVisibileMessages = includeTagInUserVisibileMessages;
        }

        public void Log(LogMessage log)
        {
            LogLevel level = log.Level;
            bool isUserVisible = level == LogLevel.Info || level == LogLevel.Warn || level == LogLevel.Error;
            bool includeTag = !isUserVisible || includeTagInUserVisibileMessages;

            if (includeTag)
                logger.Log(log.WithMessage($"({tag}) {log.Message}"));
            else logger.Log(log);
        }
    }
}
