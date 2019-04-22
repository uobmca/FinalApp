using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FinalApp.Commons {
    public static class NavigationExtensions {
        public static Task PushModalAsyncUnique(this INavigation self, Page page) {
            IReadOnlyList<Page> currentStack = Application.Current.MainPage.Navigation.ModalStack;
            int size = currentStack.Count;
            if (size == 0 || currentStack[size - 1].GetType() != page.GetType()) {
                return self.PushModalAsync(page, true);
            }
            return Task.CompletedTask;
        }

        public static Task PushAsyncUnique(this INavigation self, Page page) {
            IReadOnlyList<Page> currentStack = self.NavigationStack;
            int size = currentStack.Count;
            if (size == 0 || currentStack[size - 1].GetType() != page.GetType()) {
                return self.PushAsync(page, true);
            }
            return Task.CompletedTask;
        }
    }
}
