﻿using CmediaSDKTestApp.BaseModels;
using System.Collections.ObjectModel;

namespace CmediaSDKTestApp.Models
{
    class HorzSliderControlViewModel : MenuItem
    {
        public IMenuItem Btn_Left { get; set; }
        public IMenuItem SliderValueStr { get; set; }
        public IMenuItem SlideUnitStr { get; set; }
        public IMenuItem Btn_Right { get; set; }
        public IMenuItem SliderTitle { get; set; }
        public IMenuItem SliderTickFrequency { get; set; }
        public IMenuItem SliderMinimum { get; set; }
        public IMenuItem SliderMaximum { get; set; }
    }
}