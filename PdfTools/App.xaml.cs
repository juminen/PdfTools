﻿using PdfTools.UI.View;
using PdfTools.UI.ViewModel;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace PdfTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        ApplicationMainViewModel vm = new ApplicationMainViewModel();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetRegionalSettings();

            //Sets a value that indicates whether a data-bound TextBox should display 
            //a string that is identical to the value of the source its Text property.
            //ts. Jos laatikko on bindattu esim. desimaaliluvuksi ja UpdateSourceTrigger=PropertyChanged
            //eikä siihen ei ole laitettu StringFormattia, niin homma kusee huolella
            //(esim. jos käyttäjä koittaa kirjoittaa pilkullista lukua).
            //HUOM. Vaikuttaa kaikkiin TextBoxeihin.
            //https://stackoverflow.com/questions/14600842/bind-textbox-to-float-value-unable-to-input-dot-comma
            FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
            ShowMainView();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            vm.SaveSettings();
        }

        private void SetRegionalSettings()
        {
            //By default, a WPF application doesn’t respect the user’s regional settings.
            //Instead of using the current culture, the en-US culture is used instead.

            // set regional settings
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(
                        CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        private void ShowMainView()
        {            
            ApplicationMainView view = new ApplicationMainView()
            {
                DataContext = vm
            };
            view.Show();
        }
    }
}
