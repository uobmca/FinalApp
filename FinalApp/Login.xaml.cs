using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FinalApp
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            var vm = new LoginViewModel(this.Navigation);
            this.BindingContext = vm;
            vm.DisplayInvalidLoginPrompt += Vm_DisplayInvalidLoginPrompt;
            InitializeComponent();

            //Email.Completed += (object sender, EventArgs e) =>
            //{
            //    Password.Focus();
            //};

            //Password.Completed += (object sender, EventArgs e) =>
            //{
            //    vm.SubmitCommand.Execute(null);
            //};
        }

        void Vm_DisplayInvalidLoginPrompt()
        {
            DisplayAlert("Error", "Invalid Login, try again", "OK");
        }

    }
}
