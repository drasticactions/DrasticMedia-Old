// <copyright file="IDatabase.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticMedia.Core.Database
{
    /// <summary>
    /// Media Database.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the database has been set up.
        /// </summary>
        bool IsInitialized { get; set; }

        /// <summary>
        /// Initialize the database.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Drop the database.
        /// </summary>
        void Drop();

        /// <summary>
        /// Delete everything within the database.
        /// </summary>
        void DeleteAll();
    }
}
