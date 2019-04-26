using System;
using System.Threading.Tasks;
using FinalApp.Models;
using FinalApp.Services;
using Xamarin.Forms;

namespace FinalApp.ViewModels {
    public class TagDetailPageViewModel : BindableObject {


        protected readonly BindableProperty SelectedCategoryProperty =
            BindableProperty.Create(nameof(SelectedCategory), typeof(Category), typeof(TagDetailPageViewModel), new Category());

        public Category SelectedCategory {
            get => (Category)GetValue(SelectedCategoryProperty);
            set => SetValue(SelectedCategoryProperty, value);   
        }

        public IUserDataRepository repository;

        public TagDetailPageViewModel(IUserDataRepository repository) {
            this.repository = repository;
        }

        public async Task Save() {
            await this.repository.SaveUserCategory(SelectedCategory);
        }

    }
}
