using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WPFPageSwitch
{
	public partial class MainMenu : UserControl, ISwitchable
	{

        //LoginWindowForm loginForm;
        //RegisterForm registerForm;

		public MainMenu()
		{
            
			InitializeComponent();

            //loginForm = new LoginWindowForm();
            //registerForm = new RegisterForm();

            //loginForm.SubmitClicked += new EventHandler(loginWindowForm_SubmitClicked);
            //registerForm.SubmitClicked += new EventHandler(registerForm_SubmitClicked);
		}

		private void newGameButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Switcher.Switch(new Gameplay());
		}

		private void loadGameButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Switcher.Switch(new LoadGame());
		}

		private void optionButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Switcher.Switch(new Option());
		}

        //private void ShowMessageBox(string title, string message, MessageBoxIcon icon)
        //{
            //MessageBoxChildWindow messageWindow =
            //    new MessageBoxChildWindow(title, message, MessageBoxButtons.Ok, icon);

            //messageWindow.Show();
        //}

        #region Event For Child Window
        private void loginWindowForm_SubmitClicked(object sender, EventArgs e)
        {
            //ShowMessageBox("Login Successful", "Welcome, " + loginForm.NameText, MessageBoxIcon.Information);

        }

        private void registerForm_SubmitClicked(object sender, EventArgs e)
        {
        }


        #endregion

        #region ISwitchable Members
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void loginTextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	Switcher.Switch(new Login());
        }

        private void registerTextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	Switcher.Switch(new Register());
        }
        #endregion
		
	}
}