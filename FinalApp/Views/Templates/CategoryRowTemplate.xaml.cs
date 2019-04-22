using System;
using System.Collections.Generic;
using System.Windows.Input;
using FinalApp.Models;
using Xamarin.Forms;

namespace FinalApp.Views.Templates {
    public partial class CategoryRowTemplate : ViewCell {

        public static readonly BindableProperty EditButtonCommandProperty =
            BindableProperty.Create(nameof(EditButtonCommand), typeof(Command<Category>), typeof(CategoryRowTemplate), null);

        public Command<Category> EditButtonCommand {
            get => (Command<Category>)GetValue(EditButtonCommandProperty);
            set => SetValue(EditButtonCommandProperty, value);
        }

        public CategoryRowTemplate() {
            InitializeComponent();

            var gestureRecognizer = new TapGestureRecognizer();
            gestureRecognizer.Tapped += (sender, e) => { 
                if (EditButtonCommand != null) {
                    EditButtonCommand.Execute(BindingContext);
                }
            };
            editButton.GestureRecognizers.Add(gestureRecognizer);
        }
    }
}
