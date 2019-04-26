using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinalApp.Models;
using FinalApp.Services;
using FinalApp.Views.Base;
using FinalApp.Views.Pages.Tags;
using Xamarin.Forms;

namespace FinalApp.ViewModels {
    public class TagsPageViewModel : BindableObject {

        // Bindable Properties
        protected readonly BindableProperty CategoriesProperty =
            BindableProperty.Create(nameof(Categories), typeof(IEnumerable<Category>), typeof(TagsPageViewModel), new List<Category>());

        protected readonly BindableProperty CategoryEditButtonCommandProperty =
            BindableProperty.Create(nameof(CategoryEditButtonCommand), typeof(Command<Category>), typeof(TagsPageViewModel), null);

        public IEnumerable<Category> Categories {
            get => (IEnumerable<Category>)GetValue(CategoriesProperty);
            set => SetValue(CategoriesProperty, value);
        }

        public Command<Category> CategoryEditButtonCommand {
            get => (Command<Category>)GetValue(CategoryEditButtonCommandProperty);
            set => SetValue(CategoryEditButtonCommandProperty, value);
        }

        public IUserDataRepository repository;

        public TagsPageViewModel(IUserDataRepository repository) {
            this.repository = repository;
        }

        public async Task ReloadData() {
            Categories = await repository.GetUserCategories();
        }

    }
}
