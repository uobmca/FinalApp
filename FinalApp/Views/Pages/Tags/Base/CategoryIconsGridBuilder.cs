using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using FinalApp.Models;

namespace FinalApp.Views.Pages.Tags {
    public class CategoryIconsGridBuilder {

        private List<CategoryIcon> categoriesIcons = new List<CategoryIcon>();

        public CategoryIcon SelectedCategoryIcon { get; private set; }

        private Grid outGrid;
        private int maxColumns;

        public CategoryIconsGridBuilder() {
        }

        public CategoryIconsGridBuilder(int nColumns) {
            this.maxColumns = nColumns;
        }

        public Grid Build(int nColumns) {

            InitCategoriesIcons(nColumns);
            int nItems = categoriesIcons.Count();
            int nRows = (int)Math.Ceiling((double)(nItems / nColumns));

            RowDefinitionCollection rowDefinitions = new RowDefinitionCollection();
            for (int i = 0; i < nRows; i++) {
                rowDefinitions.Add(new RowDefinition { Height = new GridLength(32.0, GridUnitType.Absolute) });
            }

            ColumnDefinitionCollection columnDefinitions = new ColumnDefinitionCollection();
            for (int i = 0; i < nColumns; i++) {
                columnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            Grid grid = new Grid {
                RowDefinitions = rowDefinitions,
                ColumnDefinitions = columnDefinitions,
                RowSpacing = 16.0
            };

            foreach (CategoryIcon categoryIcon in categoriesIcons) {
                grid.Children.Add(categoryIcon.IconImage, categoryIcon.Column, categoryIcon.Row);
            }
            outGrid = grid;

            OnCategoryIconClicked(categoriesIcons.FirstOrDefault());

            return outGrid;
        }

        public void SetSelected(Category category) {
            foreach (CategoryIcon categoryIcon in categoriesIcons) {
                if (categoryIcon.IconSource.Equals(category.Icon)) {
                    OnCategoryIconClicked(categoryIcon);
                }
            }
        }

        private void InitCategoriesIcons(int nColumns) {
            int nItems = CategoryIcon.DefaultSources.Count();
            int row;
            int column;
            string source;
            categoriesIcons.Clear();

            for (int i = 0; i < nItems; i++) {
                source = CategoryIcon.DefaultSources[i];
                row = (int)(i / nColumns);
                column = i % nColumns;

                categoriesIcons.Add(new CategoryIcon(source, row, column) { OnClickedCommand = new Command<CategoryIcon>(OnCategoryIconClicked) });
            }
        }

        private void OnCategoryIconClicked(CategoryIcon selectedIcon) {
            if (selectedIcon == null) return;

            foreach (CategoryIcon icon in categoriesIcons) {
                if (icon != selectedIcon) {
                    icon.IconImage.Opacity = 0.25;
                }
            }

            SelectedCategoryIcon = selectedIcon;
            selectedIcon.IconImage.Opacity = 1.0;
        }
    }
}
