// <copyright file="BaseViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Library;
using DrasticMedia.Services;
using DrasticMedia.Utilities;

namespace DrasticMedia.ViewModels
{
    /// <summary>
    /// Base View Model.
    /// </summary>
    public class BaseViewModel : ExtendedBindableObject
    {
        private bool isBusy;
        private bool isRefreshing;
        private string title;
        private Page? originalPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        /// <param name="services">IServiceProvider.</param>
        /// <param name="originalPage">Original Page.</param>
        public BaseViewModel(IServiceProvider services, Page? originalPage = null)
        {
            this.originalPage = originalPage;
            this.Services = services;
            this.Navigation = services.GetService<INavigationService>();
            this.Error = services.GetService<IErrorHandlerService>();
            this.MediaLibrary = services.GetService<MediaLibrary>();
            this.CloseDialogCommand = new AsyncCommand(
               async () => await this.ExecuteCloseDialogCommand(),
               null,
               this.Error);
        }

        /// <summary>
        /// Gets or Sets a value indicating whether the view is busy.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }

            set
            {
                this.isBusy = value;
                this.OnPropertyChanged("IsBusy");
            }
        }

        /// <summary>
        /// Gets or Sets a value indicating whether the view is refreshing.
        /// </summary>
        public bool IsRefreshing
        {
            get
            {
                return this.isRefreshing;
            }

            set
            {
                this.isRefreshing = value;
                this.OnPropertyChanged("IsRefreshing");
            }
        }

        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
                this.OnPropertyChanged("Title");
            }
        }

        /// <summary>
        /// Gets the Close Dialog Command.
        /// </summary>
        public AsyncCommand CloseDialogCommand { get; private set; }

        /// <summary>
        /// Gets the service provider collection.
        /// </summary>
        protected IServiceProvider Services { get; private set; }

        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        protected INavigationService Navigation { get; private set; }

        /// <summary>
        /// Gets the error handler service.
        /// </summary>
        protected IErrorHandlerService Error { get; private set; }

        /// <summary>
        /// Gets the Media Library.
        /// </summary>
        protected MediaLibrary MediaLibrary { get; private set; }

        /// <summary>
        /// Load VM Async.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public virtual Task LoadAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Sets title for page.
        /// </summary>
        /// <param name="title">The Title.</param>
        public virtual void SetTitle(string title = "")
        {
            this.Title = title;
        }

        /// <summary>
        /// Unload VM Async.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public virtual Task UnloadAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Check if the original page exists.
        /// </summary>
        /// <returns>Page.</returns>
        /// <exception cref="NullReferenceException">Throws if page doesn't exist.</exception>
        internal Page CheckIfPageExists()
        {
            if (this.originalPage == null)
            {
                throw new NullReferenceException(nameof(this.originalPage));
            }

            return this.originalPage;
        }

        private async Task ExecuteCloseDialogCommand()
        {
            var page = this.CheckIfPageExists();
            await this.Navigation.PopModalPageInWindowViaPageAsync(page);
        }
    }
}
