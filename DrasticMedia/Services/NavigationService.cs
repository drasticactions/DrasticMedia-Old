// <copyright file="NavigationService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;

namespace DrasticMedia.Services
{
    /// <summary>
    /// Navigation Service.
    /// </summary>
    public class NavigationService : INavigationService
    {
        private IServiceProvider services;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        public NavigationService(IServiceProvider services)
        {
            this.services = services;
        }

        /// <inheritdoc/>
        public Task DisplayAlertAsync(string title, string message)
        {
            App.Current.Dispatcher.Dispatch(async () => await App.Current.MainPage.DisplayAlert(title, message, Translations.Common.CloseButton).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task PushPageInWindowAsync(Page page, Window window)
        {
            return window.Page.Navigation.PushAsync(page);
        }

        /// <inheritdoc/>
        public Task PushPageInMainWindowAsync(Page page)
        {
            return this.PushPageInWindowAsync(page, this.GetMainWindow());
        }

        /// <inheritdoc/>
        public Task PopModalPageInWindowAsync(Window window)
        {
            return window.Navigation == null
                ? throw new ArgumentException("Window must have a NavigationPage as its base")
                : window.Page.Navigation.PopModalAsync();
        }

        /// <inheritdoc/>
        public Task PopModalPageInMainWindowAsync()
        {
            return this.PopModalPageInWindowAsync(this.GetMainWindow());
        }

        /// <inheritdoc/>
        public Task GoBackPageInWindowAsync(Window window)
        {
            if (window.Page.Navigation.NavigationStack.Count > 0)
            {
                return window.Page.Navigation.PopAsync();
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        /// <inheritdoc/>
        public Task GoBackPageInMainWindowAsync()
        {
            return this.GoBackPageInWindowAsync(this.GetMainWindow());
        }

        /// <inheritdoc/>
        public Task<string> DisplayPromptAsync(string title, string message)
        {
            return this.GetMainWindow().Page.DisplayPromptAsync(title, message, keyboard: Microsoft.Maui.Keyboard.Url);
        }

        private Window GetMainWindow()
        {
            return App.Current.Windows[0];
        }
    }
}
