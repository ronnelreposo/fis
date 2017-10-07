using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Windows;
using FIS.Lib;

namespace FIS
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow: ModernWindow
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void ModernWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      var confirmation = ModernDialog.ShowMessage("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButton.YesNo);
      if (confirmation == MessageBoxResult.Yes) {
        e.Cancel = false;
        new LoginWindow().Show();
        return;
      }
      e.Cancel = true;
    }
  }
}
