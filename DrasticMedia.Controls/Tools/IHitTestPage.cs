using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticMedia.Core.Tools
{
    public interface IHitTestPage
    {
        List<IView> HitTestViews { get; }
    }
}
