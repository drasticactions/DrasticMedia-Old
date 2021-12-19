using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace DrasticMedia
{
    public class HolderPage : FlyoutPage
    {
        public HolderPage(Page flyoutPage, Page detailPage)
        {
            this.Flyout = flyoutPage;
            this.Detail = detailPage;
        }
    }
}
