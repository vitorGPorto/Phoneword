using System;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Phoneword
{
    public class MainPage : ContentPage
    {
        Entry phoneNumberText;
        Button translateButton;
        Button callButton;
        string translatedNumber;

        public MainPage()
        {
            this.Padding = new Thickness(20, 20, 20, 20);

            StackLayout panel = new StackLayout
            {
                Spacing = 15
            };

            panel.Children.Add(new Label
            {
                Text = "Digite uma palavra de telefone:",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });

            panel.Children.Add(phoneNumberText = new Entry
            {
                Text = "1-855-XAMARIN",
            });

            panel.Children.Add(translateButton = new Button
            {
                Text = "Traduzir"
            });

            panel.Children.Add(callButton = new Button
            {
                Text = "Ligar",
                IsEnabled = false,
            });

            this.Content = panel;

            translateButton.Clicked += OnTranslate;
            this.Content = panel;

            translateButton.Clicked += OnTranslate;
            callButton.Clicked += OnCall;
            this.Content = panel;
        }
        private void OnTranslate(object sender, EventArgs e)
        {
            string enteredNumber = phoneNumberText.Text;
            translatedNumber = Core.PhonewordTranslator.ToNumber(enteredNumber);

            if (!string.IsNullOrEmpty(translatedNumber))
            {
                callButton.IsEnabled = true;
                callButton.Text = "Ligar " + translatedNumber;
            }
            else
            {
                callButton.IsEnabled = false;
                callButton.Text = "Ligar";
            }
        }

        async void OnCall(object sender, System.EventArgs e)
        {
            if (await this.DisplayAlert(
                "Digite um número",
                "Você gostaria de ligar " + translatedNumber + "?",
                "Sim",
                "Não"))
            {
                try
                {
                    PhoneDialer.Open(translatedNumber);
                }
                catch (ArgumentNullException)
                {
                    await DisplayAlert("Não foi possível discar", "O número de telefone não era válido.", "OK");
                }
                catch (FeatureNotSupportedException)
                {
                    await DisplayAlert("Não foi possível discar", "Discagem por telefone não suportada.", "OK");
                }
                catch (Exception)
                {
                    // Ocorreu outro erro
                    await DisplayAlert("Não foi possível discar", "Falha na discagem telefônica.", "OK");
                }
            }
        }
    }
}
