using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FinalApp.Models;
using FinalApp.Services;
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;

namespace FinalApp.ViewModels {
    public class IncomesPageViewModel : BindableObject {

        protected readonly BindableProperty IncomesChartProperty =
            BindableProperty.Create(nameof(IncomesChart), typeof(Chart), typeof(IncomesPageViewModel), null);

        protected readonly BindableProperty UserIncomesProperty =
            BindableProperty.Create(nameof(UserIncomes), typeof(IEnumerable<UserIncome>), typeof(IncomesPageViewModel), new List<UserIncome>());

        public Chart IncomesChart {
            get => (Chart)GetValue(IncomesChartProperty);
            set => SetValue(IncomesChartProperty, value);
        }

        public IEnumerable<UserIncome> UserIncomes {
            get => (IEnumerable<UserIncome>)GetValue(UserIncomesProperty);
            set => SetValue(UserIncomesProperty, value);
        }

        private IUserDataRepository repository;

        public IncomesPageViewModel(IUserDataRepository repository) {
            this.repository = repository;
            Update();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            base.OnPropertyChanged(propertyName);
            if (propertyName == UserIncomesProperty.PropertyName) {
                UpdateIncomesChart();
            }
        }

        private void UpdateIncomesChart() {
            IncomesChart = new RadarChart {
                Entries = UserIncomes.GroupBy((arg) => arg.CategoryId).Select((arg) => new ChartEntry((float)arg.Sum((item) => item.Amount)) {
                    Color = CategoryIdToSKColor((int)arg.First().CategoryId),
                    TextColor = CategoryIdToSKColor((int)arg.First().CategoryId),
                    Label = CategoryIdToString((int)arg.First().CategoryId),
                    ValueLabel = arg.First().Amount.ToString("F1")
                }),
                LabelTextSize = 18.0f,
                BackgroundColor = SKColors.Transparent
            };
        }

        private string CategoryIdToString(int id) {
            switch (id) {
                case 1: return "House";
                case 2: return "Car";
                case 3: return "Entertainment";
                default: return "Other";
            }
        }

        private SKColor CategoryIdToSKColor(int id) {
            switch (id) {
                case 1: return SKColors.Green;
                case 2: return SKColors.Blue;
                case 3: return SKColors.DeepPink;
                default: return SKColors.Red;
            }
        }

        public async void Update() {
            UserIncomes = await repository.GetUserIncomes();
            UpdateIncomesChart();
        }
    }
}
