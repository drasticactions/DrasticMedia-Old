// <copyright file="PageBackground.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticMedia.Overlays
{
    /// <summary>
    /// Page Background.
    /// </summary>
    public partial class PageBackground : WindowOverlay, IVisualTreeElement
    {
        private bool pageOverlayNativeElementsInitialized;
        internal bool pageSet;
        internal Page? page;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageBackground"/> class.
        /// </summary>
        /// <param name="window">Window.</param>
        public PageBackground(IWindow window)
            : base(window)
        {
        }

        /// <inheritdoc/>
        public IReadOnlyList<IVisualTreeElement> GetVisualChildren()
        {
            if (this.pageSet && this.page is IVisualTreeElement element)
            {
                return element.GetVisualChildren();
            }

            return new List<IVisualTreeElement>();
        }

        /// <inheritdoc/>
        public IVisualTreeElement? GetVisualParent()
        {
            return this.Window as IVisualTreeElement;
        }
    }
}
