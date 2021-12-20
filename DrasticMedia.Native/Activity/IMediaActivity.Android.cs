using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Services;

namespace DrasticMedia.Native.Activity
{
    public interface IMediaActivity
    {
        MediaPlayerServiceBinder Binder { get; set; }
    }
}
