using UnityEngine;
using UnityEngine.UI;

namespace Packages.SoftsEssentialKit.Runtime.Extensions
{
	public static class ImageExtensions {

		public static float _GetAlpha(this Image image)
		{
			return image.color.a;
		}
	
		public static void _SetAlpha(this Image image, float alpha)
		{
			Color newColor = image.color;
			newColor.a = alpha;
			image.color = newColor;
		}
	}
}
