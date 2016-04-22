using System;
using UIKit;

namespace CorpTraining.iOS
{
    public static class UIConstants
    {
        // Note: use const instead of static readonly because EasyLayout.cs doesn't accept static readonly.
        // The consumer of this class is in the same assembly (since it is a Shared Project) so const is ok.

        public const float BorderWidth = 1;
        public const float CornerRadius = 10;

        public static readonly float StatusBarHeight = (float)UIApplication.SharedApplication.StatusBarFrame.Height; // Note: The View.Frame.Top starts from the edge of the device's screen.
        public const float SmallHorizontalPad = 15;
        public const float HorizontalPad = 30;
        public const float VerticalPad = 30;

        public const float MaximumControlsWidth = 400;
        public const float ControlsHeight = 50;
        // Height is fixed for specific device, hard to be adaptive!!
        public const float SmallGap = 10f;
        public const float BigGap = 20f;

        public const float NormalFontSize = 20;

        public const float MainColorHue = 0.553f;
        public const float MainColorSaturation = 0.650f;
        public const float MainColorBrightness = 0.821f;
        public static readonly UIColor MainColor = UIColor.FromHSB(UIConstants.MainColorHue, UIConstants.MainColorSaturation, UIConstants.MainColorBrightness);
    }
}

