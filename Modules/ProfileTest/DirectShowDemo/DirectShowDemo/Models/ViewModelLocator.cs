using System;
using System.ComponentModel;
using System.Windows;

namespace DirectShowDemo.Models
{
    public class ViewModelLocator
    {
        public static bool GetAutoHookedUpViewModel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoHookedUpViewModelProperty);
        }

        public static void SetAutoHookedUpViewModel(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoHookedUpViewModelProperty, value);
        }

        // Using a DependencyProperty as the backing store for AutoHookedUpViewModel. 
        //This enables animation, styling, binding, etc...

        public static readonly DependencyProperty AutoHookedUpViewModelProperty =
           DependencyProperty.RegisterAttached("AutoHookedUpViewModel",
           typeof(bool), typeof(ViewModelLocator), new PropertyMetadata(false,
           AutoHookedUpViewModelChanged));

        private static void AutoHookedUpViewModelChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d)) return;
            var viewType = d.GetType();
            //Get Model
            string strModel = viewType.FullName;
            strModel = strModel.Replace(".Views.", ".Models.");
            string[] viewNameArray = strModel.Split('.');
            var modelName = $"{viewNameArray[viewNameArray.Length - 1]}Model";
            var viewTypeName = strModel.Remove(strModel.LastIndexOf('.') + 1);
            var modelTypeName = viewTypeName + modelName;
            var modelType = Type.GetType(modelTypeName);
            object modelInstance = null;
            if (modelType != null)
            {
                modelInstance = Activator.CreateInstance(modelType);
            }

            //Get ViewModel
            string str = viewType.FullName;
            str = str.Replace(".Views.", ".ViewModels.");
            viewTypeName = str;
            var viewModelTypeName = viewTypeName + "ViewModel";
            var viewModelType = Type.GetType(viewModelTypeName);
            if (viewModelType != null)
            {
                var viewModel = Activator.CreateInstance(viewModelType, modelInstance);
                ((FrameworkElement)d).DataContext = viewModel;
            }
        }
    }
}
