using System.Windows;
using System.Windows.Media;

namespace CommonUILib.Models
{
    public class CommonUtility
    {
        private static CommonUtility _instance;
        public static CommonUtility Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new CommonUtility();
                }
                return _instance;
            }
        }
        public LinearGradientBrush GetGradientBrushByHex(string startHex, string endHex, double endOffset)
        {
            var startBrush = (Color)ColorConverter.ConvertFromString(startHex);
            var endBrush = (Color)ColorConverter.ConvertFromString(endHex);
            LinearGradientBrush lb = new LinearGradientBrush();
            lb.StartPoint = new Point(0, 0);
            lb.EndPoint = new Point(1, 1);
            lb.GradientStops = new GradientStopCollection() {
                new GradientStop() { Color = startBrush, Offset = 0.0 },
                new GradientStop() { Color = endBrush, Offset = endOffset}};
            return lb;
        }
    }
}
