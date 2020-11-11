

namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using System;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Sales.Views;

    public class ProductItemViewModel : Product
    {
        #region Attributes
        private ApiService apiService;
        #endregion

        #region Constructors

        public ProductItemViewModel()
        {
            this.apiService = new ApiService();
        }
        #endregion


        #region Commands

        /// <summary>
        /// EDITAR
        /// </summary>
        public ICommand EditProductCommand
        {
            get
            {
                return new RelayCommand(EditProduct);
            }

        }

        private async void EditProduct()
        {
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel(this);  
            await Application.Current.MainPage.Navigation.PushAsync(new EditProductPage());
        }


        /// <summary>
        /// BORRAR 
        /// </summary>
        public ICommand DeleteProductCommand 
        {
            get
            {
                return new RelayCommand(DeleteProduct);
            }
        
        }

        private async void DeleteProduct()
        {
            var answer = await Application.Current.MainPage.DisplayAlert(
                Languages.Confirm,
                Languages.ConfirmDelete,
                Languages.Yes,
                Languages.No);
            if (!answer)
            {
                return;
            }


            //checamos la conexion

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            var url = Application.Current.Resources["UrlApi"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var productsController = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.apiService.Delete(url, prefix, productsController, this.ProductId);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }


            //para refrescar la lista despues de eliminar un registro de la base de datos.
            var productViewModel = ProductsViewModel.GetInstance();
            var deletedProduct = productViewModel.Products.Where(p => p.ProductId == this.ProductId).FirstOrDefault();
            if (deletedProduct != null)
            {
                productViewModel.Products.Remove(deletedProduct);
            }
        }
        #endregion
    }
}
