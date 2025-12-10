using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace pr6.Pages
{
    /// <summary>
    /// Логика взаимодействия для PinSetup.xaml
    /// </summary>
    public partial class PinSetup : Page
    {
        public PinSetup()
        {
            InitializeComponent();
            PinControl.Clear(); 
        }

        private void PinControl_PinCompleted(string pin)
        {
            BtnSetPin.IsEnabled = true; 
        }

        private void BtnSetPin_Click(object sender, RoutedEventArgs e)
        {
            string pin = PinControl.PinCode;

            if (pin.Length == 4)
            {
                MainWindow.mainWindow.UserLogIn.SavePin(pin);
                MessageBox.Show("PIN установлен успешно!");
                MainWindow.mainWindow.ShowMainApp();
            }
            else
            {
                MessageBox.Show("Введите 4 цифры!");
                PinControl.Clear();
                BtnSetPin.IsEnabled = false;
            }
        }

        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }
    }
}