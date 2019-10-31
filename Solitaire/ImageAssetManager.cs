using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Solitaire
{
    public static class ImageAssetManager
    {
        private static Dictionary<string, Drawable> imageCache = new Dictionary<string, Drawable>();

        public static Drawable GetDrawable(Context context, string url)
        {
            // If the asset isn't already loaded and insidie our dictionary:
            if (!imageCache.ContainsKey(url))
            {
                // If we can't find the asset we will return null
                try
                {
                    imageCache.Add(url, Drawable.CreateFromStream(context.Assets.Open(url), null));
                }
                catch
                {
                    return null;
                }                
            }

            return imageCache[url];
        }
    }
}