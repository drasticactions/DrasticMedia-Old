// <copyright file="INavigationService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace DrasticMedia.Services
{
 /// <summary>
    /// Navigation Service.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Display Alert to User.
        /// </summary>
        /// <param name="title">Title of message.</param>
        /// <param name="message">Message to user.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        Task DisplayAlertAsync(string title, string message);

        /// <summary>
        /// Push Page In Window.
        /// </summary>
        /// <param name="page"><see cref="Page"/> to navigate to.</param>
        /// <param name="window"><see cref="Window"/>.</param>
        /// <returns><see cref="Task"/>.</returns>
        public Task PushPageInWindowAsync(Page page, Window window);

        /// <summary>
        /// Push Page In Main Window.
        /// </summary>
        /// <param name="page"><see cref="Page"/> to navigate to.</param>
        /// <returns><see cref="Task"/>.</returns>
        public Task PushPageInMainWindowAsync(Page page);

        /// <summary>
        /// Pop Modal page from Main Window.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public Task PopModalPageInMainWindowAsync();

        /// <summary>
        /// Pop Modal Page From Window.
        /// </summary>
        /// <param name="window"><see cref="Window"/> with Modal to pop.</param>
        /// <returns><see cref="Task"/>.</returns>
        public Task PopModalPageInWindowAsync(Window window);

        /// <summary>
        /// Go back from page in Window.
        /// </summary>
        /// <param name="window">Window.</param>
        /// <returns><see cref="Task"/>.</returns>
        public Task GoBackPageInWindowAsync(Window window);

        /// <summary>
        /// Go back from page in Main Window.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public Task GoBackPageInMainWindowAsync();

        /// <summary>
        /// Display Prompt.
        /// </summary>
        /// <param name="title">Title of Prompt.</param>
        /// <param name="message">Message of Prompt.</param>
        /// <returns>String.</returns>
        public Task<string> DisplayPromptAsync(string title, string message);
    }
}
