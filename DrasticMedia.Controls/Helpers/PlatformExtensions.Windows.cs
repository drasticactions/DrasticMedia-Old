using System;
using Microsoft.UI.Xaml;

namespace DrasticMedia
{
	internal static class PlatformExtensions
	{
		internal static FrameworkElement? GetNative(this IElement view, bool returnWrappedIfPresent)
		{
			if (view.Handler is INativeViewHandler nativeHandler && nativeHandler.NativeView != null)
				return nativeHandler.NativeView;

			return (view.Handler?.NativeView as FrameworkElement);

		}
	}
}
