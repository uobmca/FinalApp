using System;
using FinalApp.Models;
using FinalApp.Services;
using Xamarin.Forms;

namespace FinalApp.ViewModels {
    public class TagDetailPageViewModel : BindableObject {


        protected static readonly BindableProperty SelectedCategoryProperty =
            BindableProperty.Create(nameof(SelectedCategory), typeof(Category), typeof(TagDetailPageViewModel), null);

        public Category SelectedCategory {
            get => (Category)GetValue(SelectedCategoryProperty);
            set => SetValue(SelectedCategoryProperty, value);   
        }

        IUserDataRepository repository;

        public TagDetailPageViewModel(IUserDataRepository repository) {
            this.repository = repository;
        }



    }
}
