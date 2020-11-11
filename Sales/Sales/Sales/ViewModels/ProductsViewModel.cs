

namespace Sales.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Sales.Services;
    using Xamarin.Forms;

    public class ProductsViewModel: BaseViewModel
    {
        #region Atributtes
        private ApiService ApiService;

        private bool isRefreshing;

        private ObservableCollection<ProductItemViewModel> products;
        #endregion


        #region Properties

        public List<Product> MyProducts { get; set; }

        public ObservableCollection<ProductItemViewModel> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructors
        public ProductsViewModel()
        {
            instance = this;
            this.ApiService = new ApiService();
            this.LoadProducts();
        }

        #endregion

        #region Singleton
        private static ProductsViewModel instance;

        public static ProductsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ProductsViewModel();
            }
            return instance;
        }
        #endregion

        #region Methods
        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            var connection = await this.ApiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            var url = Application.Current.Resources["UrlApi"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var productsController = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.ApiService.GetList<Product>(url, prefix, productsController);

            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            this.MyProducts = (List<Product>)response.Result;
            this.RefreshList();
            this.IsRefreshing = false;
        }

        public void RefreshList()
        {
            var MylistProductItemViewModel = MyProducts.Select(p => new ProductItemViewModel
            {
                Description = p.Description,
                ImageArray = p.ImageArray,
                ImagePath = p.ImagePath,
                Price = p.Price,
                ProductId = p.ProductId,
                IsAvailable = p.IsAvailable,
                PublishOn = p.PublishOn,
                Remarks = p.Remarks,
            });

            ////Se comenta este código porque no es muy practico hacerlo ya que puede ser tardado
            //var Mylist = new List<ProductItemViewModel>();
            //foreach (var item in list)
            //{
            //    Mylist.Add(new ProductItemViewModel
            //    {
            //    });
            //}

            //no se puede mandar una lista de productos, se tiene que mandar una lista de productItemView Model
            //hay dos maneras de hacerlo, una con un foreach que se encontrará comentado ya que en este caso es demasiado lento y 
            //afecta en el performance y la otra manera es con una expresion lambda asignando cada uno de los atributos a la lista de 
            //ProductItemViewModel
            this.Products = new ObservableCollection<ProductItemViewModel>(MylistProductItemViewModel.OrderBy(p => p.Description));
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadProducts);
            }
        } 
        #endregion
    }
}
