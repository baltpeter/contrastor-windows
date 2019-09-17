using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Contrastor
{
    /// <summary>
    /// Interaction logic for Popup.xaml
    /// </summary>
    public partial class PopupControl : UserControl
    {
        public PopupControl()
        {
            InitializeComponent();

            DataContext = this;

            firstEyeDropper.SelectedColor = Color.FromRgb(0, 0, 0);
            secondEyeDropper.SelectedColor = Color.FromRgb(255, 255, 255);
        }

        public Color FirstColor
        {
            get => (Color)ColorConverter.ConvertFromString("#" + firstColorInput.Text);
            set
            {
                firstColorInput.Text = value.ToString().Substring(3);
                Update(null, null);
            }
        }
        public Color SecondColor
        {
            get => (Color)ColorConverter.ConvertFromString("#" + secondColorInput.Text);
            set
            {
                secondColorInput.Text = value.ToString().Substring(3);
                Update(null, null);
            }
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            firstColorInput.Text = ColorHelpers.CleanUpHexCode(firstColorInput.Text);
            secondColorInput.Text = ColorHelpers.CleanUpHexCode(secondColorInput.Text);

            foreach(var obj in new[] { ratioLabel, ratingLabel, firstPound, secondPound}) obj.Foreground = new SolidColorBrush(FirstColor);
            backgroundBorder.Background = new SolidColorBrush(SecondColor);

            double ratio = ColorHelpers.ContrastRatio(FirstColor, SecondColor);
            ratioLabel.Text = ratio.ToString("N2");
            if (ratio >= 7.0) ratingLabel.Text = "AAA";
            else if (ratio >= 4.5) ratingLabel.Text = "AA";
            else if (ratio >= 3.0) ratingLabel.Text = "AA*";
            else ratingLabel.Text = "FAIL";
        }
    }
}
