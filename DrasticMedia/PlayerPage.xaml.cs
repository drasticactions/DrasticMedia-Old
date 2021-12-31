using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMaui.Overlays;
using DrasticMedia.Core.Services;
using DrasticMedia.Core.Utilities;
using DrasticMedia.ViewModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace DrasticMedia
{
    public partial class PlayerPage : ContentPage, IHitTestView
    {
        private IServiceProvider provider;
        private PlayerPageViewModel vm;

        public PlayerPage(IServiceProvider provider)
        {
            this.InitializeComponent();
            this.provider = provider;
            this.vm = provider.ResolveWith<PlayerPageViewModel>(this);
            this.BindingContext = vm;
            this.ControlLayout.HeightRequest = 200;
            this.HitTestViews.Add(this.ControlLayout);

            //HACK: For some reason, the binding isn't working and I need to get the property off of the service???
            this.vm.Player.PropertyChanged += Player_PropertyChanged;
            this.DrasticSlider.NewPositionRequested += this.DrasticSlider_NewPositionRequested;

            this.VolumeSlider.Value = 100;
        }

        public double GetHeightOfPlayer() => this.ControlLayout.HeightRequest;

        private void Player_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PlayerService.CurrentPosition))
            {
                this.DrasticSlider.Value = this.vm.Player.CurrentPosition;
            }
        }

        private void DrasticSlider_NewPositionRequested(object sender, DrasticSliderPositionChangedEventArgs e)
        {
            if (this.vm.Player?.CurrentPosition != null)
            {
                this.vm.Player.CurrentPosition = e.Position;
            }
        }

        /// <summary>
        /// Gets the hit test views.
        /// </summary>
        public List<IView> HitTestViews { get; } = new List<IView>();
    }
}
