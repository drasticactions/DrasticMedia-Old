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
        /// Push Page In Window.
        /// </summary>
        /// <param name="page"><see cref="Page"/> to navigate to.</param>
        /// <param name="originalPage"><see cref="originalPage"/>.</param>
        /// <returns><see cref="Task"/>.</returns>
        public Task PushPageInWindowViaPageAsync(Page page, Page originalPage);

        /// <summary>
        /// Pop Modal Page From Window.
        /// </summary>
        /// <param name="window"><see cref="Window"/> with Modal to pop.</param>
        /// <returns><see cref="Task"/>.</returns>
        public Task PopModalPageInWindowAsync(Window window);

        /// <summary>
        /// Pop Modal Page From Page Window.
        /// </summary>
        /// <param name="page"><see cref="Page"/> with Modal to pop.</param>
        /// <returns><see cref="Task"/>.</returns>
        public Task PopModalPageInWindowViaPageAsync(Page page);

        /// <summary>
        /// Go back from page in Window.
        /// </summary>
        /// <param name="window">Window.</param>
        /// <returns><see cref="Task"/>.</returns>
        public Task GoBackPageInWindowAsync(Window window);

        /// <summary>
        /// Go back from page in Window.
        /// </summary>
        /// <param name="page">page.</param>
        /// <returns><see cref="Task"/>.</returns>
        public Task GoBackPageInWindowViaPageAsync(Page page);

        /// <summary>
        /// Display Prompt In Window.
        /// </summary>
        /// <param name="window">Window.</param>
        /// <param name="title">Title of Prompt.</param>
        /// <param name="message">Message of Prompt.</param>
        /// <returns>String.</returns>
        public Task<string> DisplayPromptInWindowAsync(Window window, string title, string message);

        /// <summary>
        /// Display Prompt In Window via page.
        /// </summary>
        /// <param name="page">page.</param>
        /// <param name="title">Title of Prompt.</param>
        /// <param name="message">Message of Prompt.</param>
        /// <returns>String.</returns>
        public Task<string> DisplayPromptInWindowViaPageAsync(Page page, string title, string message);
    }
}
