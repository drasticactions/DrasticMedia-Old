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
            if (App.Current?.MainPage == null)
            {
                return Task.CompletedTask;
            }

            App.Current.Dispatcher.Dispatch(async () => await App.Current.MainPage.DisplayAlert(title, message, Translations.Common.CloseButton).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task PushPageInWindowAsync(Page page, Window? window)
        {
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            if (window == null)
            {
                return Task.CompletedTask;
            }

            if (window.Page is FlyoutPage flyoutPage && flyoutPage.Detail is NavigationPage navPage)
            {
                return navPage.PushAsync(page);
            }

            if (window.Page is NavigationPage baseNavPage)
            {
                return baseNavPage.PushAsync(page);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task PushPageInWindowViaPageAsync(Page page, Page originalPage) => this.PushPageInWindowAsync(page, this.GetWindowFromPage(originalPage));

        /// <inheritdoc/>
        public Task PopModalPageInWindowAsync(Window window)
        {
            var navPage = this.GetBaseNavigationPageInWindow(window);
            return navPage == null
                ? throw new ArgumentException("Window must have a NavigationPage as its base")
                : navPage.Navigation.PopModalAsync();
        }

        /// <inheritdoc/>
        public Task PopModalPageInWindowViaPageAsync(Page page) => this.PopModalPageInWindowAsync(this.GetWindowFromPage(page));

        /// <inheritdoc/>
        public Task GoBackPageInWindowAsync(Window window)
        {
            var navigationPage = this.GetBaseNavigationPageInWindow(window);
            if (navigationPage == null)
            {
                return Task.CompletedTask;
            }

            if (navigationPage.Navigation.NavigationStack.Count > 0)
            {
                return navigationPage.PopAsync();
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        /// <inheritdoc/>
        public Task GoBackPageInWindowViaPageAsync(Page page) => this.GoBackPageInWindowAsync(this.GetWindowFromPage(page));

        /// <inheritdoc/>
        public async Task<string> DisplayPromptInWindowAsync(Window window, string title, string message)
        {
            if (window.Page == null)
            {
                return string.Empty;
            }

            return await window.Page.DisplayPromptAsync(title, message, keyboard: Microsoft.Maui.Keyboard.Url);
        }

        /// <inheritdoc/>
        public Task<string> DisplayPromptInWindowViaPageAsync(Page page, string title, string message)
            => this.DisplayPromptInWindowAsync(this.GetWindowFromPage(page), title, message);

        private NavigationPage? GetBaseNavigationPageInWindow(Window window)
        {
            if (window.Page is FlyoutPage flyoutPage && flyoutPage.Detail is NavigationPage navPage)
            {
                return navPage;
            }

            if (window.Page is NavigationPage baseNavPage)
            {
                return baseNavPage;
            }

            return null;
        }

        private Window GetWindowFromPage(Page page)
        {
            if (page == null)
            {
                throw new NullReferenceException(nameof(page));
            }

            var window = page.GetParentWindow();
            if (window == null)
            {
                throw new NullReferenceException(nameof(window));
            }

            return window;
        }
    }
}