﻿using System;
using Autofac;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FinalApp
{
    public partial class App : Application {

        public static IContainer Container { get; private set; }

        public App() {
            InitializeComponent();
            RegisterDependencies();
            MainPage = new Login();
        }

        protected override void OnStart() {
            // Handle when your app starts
        }

        protected override void OnSleep() {
            // Handle when your app sleeps
        }

        protected override void OnResume() {
            // Handle when your app resumes
        }

        private void RegisterDependencies() {
            var builder = new ContainerBuilder();
            builder.RegisterType<LoginViewModel>();
            builder.RegisterType<MainPageViewModel>();
            Container = builder.Build();
        }
    }
}
