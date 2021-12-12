// <copyright file="ParseMediaException.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DrasticMedia.Core.Exceptions
{
    /// <summary>
    /// Parse Media Exception.
    /// </summary>
    public class ParseMediaException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseMediaException"/> class.
        /// </summary>
        public ParseMediaException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseMediaException"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        public ParseMediaException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseMediaException"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="innerException">Inner Exception.</param>
        public ParseMediaException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseMediaException"/> class.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">StreamingContext.</param>
        protected ParseMediaException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
