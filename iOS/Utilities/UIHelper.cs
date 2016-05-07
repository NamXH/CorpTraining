using System;
using CoreGraphics;
using UIKit;
using Foundation;

namespace CorpTraining.iOS
{
    public static class UIHelper
    {
        public static CGRect GetTextSize(string text, UIFont font, nfloat maxAvailableWidth, nfloat maxAvailableHeight)
        {
            return new NSString(text).GetBoundingRect(
                new CGSize(maxAvailableWidth, maxAvailableHeight),
                NSStringDrawingOptions.UsesLineFragmentOrigin, 
                new UIStringAttributes(){ Font = font }, 
                null);
        }
    }
}

