﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Autofac;
using FinalApp.Commons;
using FinalApp.ViewModels;
using FinalApp.Views.Base;
using FinalApp.Views.Pages.SelectExpense;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace FinalApp.Views.Pages.AddIncomePage {
    public partial class AddIncomePage : ContentPage {

        private Task takePictureAnimation;
        private Task insertManuallyAnimation;

        public AddIncomePage() {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI() {
            ToolbarItems.Add(new ToolbarItem("Close", "ic_close", () => {
                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopModalAsync();
                });
            
            }));
            takePictureOptionGrid.Opacity = 0.0;
            pickPictureOptionGrid.Opacity = 0.0;
            insertManuallyOptionGrid.Opacity = 0.0;


            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (sender, e) => {

                if (sender is Grid grid) {
                    grid.ToString();
                    ShowOptionSelection(grid);
                }

                if (sender == takePictureOptionGrid) {
                    await GoToTakePicturePage();
                    HideOptionSelection(takePictureOptionGrid);
                }

                if (sender == pickPictureOptionGrid) {
                    await OpenPictureGallery();
                    HideOptionSelection(pickPictureOptionGrid);
                }

                if (sender == insertManuallyOptionGrid) {
                    await OpenInsertManuallyPage();
                    HideOptionSelection(insertManuallyOptionGrid);
                }
            };

            takePictureOptionGrid.GestureRecognizers.Add(tapGestureRecognizer);
            pickPictureOptionGrid.GestureRecognizers.Add(tapGestureRecognizer);
            insertManuallyOptionGrid.GestureRecognizers.Add(tapGestureRecognizer);

            Device.BeginInvokeOnMainThread(() => {
                RunAnimations();
            });
        }

        private async Task OpenInsertManuallyPage() {
            var page = new ExpenseDetail.ExpenseDetailPage();
            await Navigation.PushModalAsyncUnique(new AppNavigationPage(page));
        }

        private async Task OpenPictureGallery() {

            if (Device.RuntimePlatform.Equals("iOS")) { 
                await Task.Delay(100); 
            }
            MediaFile mediaFile = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions {
                PhotoSize = PhotoSize.Medium
            });

            if (mediaFile == null) {
                return;
            }

            await SelectExpensePage.PromptUserForExpenseType(Navigation, async (expenseType) => {
                using (var scope = App.Container.BeginLifetimeScope()) {
                    if (scope.Resolve<AnalyzePicturePageViewModel>() is AnalyzePicturePageViewModel viewModel) {
                        viewModel.UserImageFilePath = mediaFile.Path;
                        viewModel.UserImageSource = ImageSource.FromFile(mediaFile.Path);
                        viewModel.ExpenseType = expenseType.ExpenseType;
                        await Navigation.PushAsync(new AnalyzePicture.AnalyzePicturePage {  BindingContext = viewModel }, true);
                    }
                }
            });
        }

        private async Task GoToTakePicturePage() {
            await Navigation.PushAsyncUnique(new TakePicture.TakePicturePage());
        }

        private void ShowOptionSelection(Grid grid) {
            Device.BeginInvokeOnMainThread(() => {
                grid.BackgroundColor = Color.FromHex("#e2e2e2");
            });
        }

        private void HideOptionSelection(Grid grid) {
            Device.BeginInvokeOnMainThread(() => {
                grid.BackgroundColor = Color.Transparent;
            });
        }

        private void RunAnimations() {
            takePictureOptionGrid.FadeTo(1.0, 500);
            takePictureOptionGrid.TranslateTo(0, 0, 500, Easing.SinOut);
            pickPictureOptionGrid.FadeTo(1.0, 500);
            pickPictureOptionGrid.TranslateTo(0, 0, 500, Easing.SinOut);
            insertManuallyOptionGrid.FadeTo(1.0, 500);
            insertManuallyOptionGrid.TranslateTo(0, 0, 500, Easing.SinOut);
        }
    }
}
