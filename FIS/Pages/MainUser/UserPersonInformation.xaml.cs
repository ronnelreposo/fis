using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FIS.Pages.MainUser
{
    /// <summary>
    /// Interaction logic for userPersonInformation.xaml
    /// </summary>
    public partial class userPersonInformation : UserControl
    {
        public userPersonInformation()
        {
            InitializeComponent();
        }

        private void setupButton_Click(object sender, RoutedEventArgs e)
        {
            setupButton.Visibility = Visibility.Collapsed;
            nextButton.Visibility = Visibility.Visible;
            cancelButton.Visibility = Visibility.Visible;
            groupPersonalInformation.IsEnabled = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            setupButton.Visibility = Visibility.Visible;
            nextButton.Visibility = Visibility.Collapsed;
           
        }
        
    }
}
