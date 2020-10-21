﻿

namespace Sales.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
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
        #endregion


        #region Properties

        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
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
            this.IsRefreshing = false;
            var list = (List<Product>)response.Result;
            this.Products = new ObservableCollection<Product>(list);

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
