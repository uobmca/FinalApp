using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace FinalApp
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Action DisplayInvalidLoginPrompt;
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        //public ICommand SubmitCommand { protected set; get; }

        public ICommand GoogleLoginCommand { protected set; get; }

        private readonly INavigation navigation;
        public LoginViewModel(INavigation navigation)
        {
            //SubmitCommand = new Command(OnSubmit);
            GoogleLoginCommand = new Command(GoogleLogin);
            this.navigation = navigation;
        }

        public void GoogleLogin(object obj)
        {
            Application.Current.MainPage = new MainPage();
        }


        //public void OnSubmit()
        //{
        //    if (email != "123456" || password != "123")
        //    {
        //        DisplayInvalidLoginPrompt();
        //    }

        //    else
        //    {
        //        Application.Current.MainPage = new MainPage();
        //    }
        //}
       
    }
}
