// <copyright file="ErrorHandlerService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using DrasticMedia.Core;

namespace DrasticMedia.Services
{
    /// <summary>
    /// Error Handler Service.
    /// </summary>
    public class ErrorHandlerService : IErrorHandlerService
    {
        private INavigationService navigation;
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlerService"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation.</param>
        public ErrorHandlerService(INavigationService navigation, ILogger logger)
        {
            this.navigation = navigation;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public void HandleError(Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            // TODO: Log exception to error handling service provider.
            string errorMessage = string.Format(Translations.Common.ErrorMessage, exception.GetType().FullName, exception.Message, exception.StackTrace);

            this.logger.Log(LogLevel.Error, errorMessage);
            this.navigation.DisplayAlertAsync(Translations.Common.ErrorTitle, errorMessage);
        }
    }
}
