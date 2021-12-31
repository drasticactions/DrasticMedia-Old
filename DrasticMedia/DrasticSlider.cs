using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticMedia
{
    /// <summary>
    /// Drastic Slider.
    /// </summary>
    public partial class DrasticSlider : Slider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrasticSlider"/> class.
        /// </summary>
        public DrasticSlider()
        {
            this.DragCompleted += this.DrasticSlider_DragCompleted;
        }

        /// <summary>
        /// Position Changed.
        /// </summary>
        public event EventHandler<DrasticSliderPositionChangedEventArgs>? NewPositionRequested;

        private void DrasticSlider_DragCompleted(object? sender, EventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            this.NewPositionRequested?.Invoke(this, new DrasticSliderPositionChangedEventArgs((float)this.Value));
        }
    }

    /// <summary>
    /// The Drastic Sliders's position changed.
    /// </summary>
    public class DrasticSliderPositionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Drastic Sliders's current position.
        /// </summary>
        public readonly float Position;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrasticSliderPositionChangedEventArgs"/> class.
        /// </summary>
        /// <param name="position">Position.</param>
        internal DrasticSliderPositionChangedEventArgs(float position)
        {
            this.Position = position;
        }
    }
}
