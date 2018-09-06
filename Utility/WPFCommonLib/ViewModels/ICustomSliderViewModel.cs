namespace WPFCommonLib.ViewModels
{
    public interface ICustomSliderViewModel
    {
        double CoreMax { get; set; }
        double CoreMin { get; set; }
        double CoreTick { get; set; }
        string CoreTitle { get; set; }
        double CoreValue { get; set; }
    }
}
