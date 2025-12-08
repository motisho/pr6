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
            try
            {
                if (MainWindow.mainWindow.UserLogIn.Image != null && MainWindow.mainWindow.UserLogIn.Image.Length > 0)
                {
                    using (var ms = new System.IO.MemoryStream(MainWindow.mainWindow.UserLogIn.Image))
                    {
                        var bi = new System.Windows.Media.Imaging.BitmapImage();
                        bi.BeginInit();
                        bi.StreamSource = ms;
                        bi.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                        bi.EndInit();
                        IUserControl.Source = bi;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void BtnSetPin_Click(object sender, RoutedEventArgs e)
        {
            string pin = $"{TbPin1.Password}{TbPin2.Password}{TbPin3.Password}{TbPin4.Password}";
            if (pin.Length == 4 && pin.All(char.IsDigit))
            {
                MainWindow.mainWindow.UserLogIn.SavePin(pin);
                MessageBox.Show("PIN установлен успешно!");
                MainWindow.mainWindow.ShowMainApp();
            }
            else
            {
                MessageBox.Show("Введите 4 цифры!");
                ClearPins();
            }
        }

        private void ClearPins()
        {
            TbPin1.Password = TbPin2.Password = TbPin3.Password = TbPin4.Password = "";
            TbPin1.Focus();
        }

        private void TbPin1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && TbPin1.Password.Length == 1) TbPin2.Focus();
        }

        private void TbPin2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && TbPin2.Password.Length == 1) TbPin3.Focus();
        }

        private void TbPin3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && TbPin3.Password.Length == 1) TbPin4.Focus();
        }

        private void TbPin4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) BtnSetPin_Click(null, null);
        }

        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }
    }
}