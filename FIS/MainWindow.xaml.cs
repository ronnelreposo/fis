using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Windows;
using FIS.Lib;
using FIS.Windows.LogIn;

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
        new LogInWindow().Show();
        return;
      }
      e.Cancel = true;
    }
  }
}
