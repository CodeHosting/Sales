
namespace Sales.BackEnd.Models
{
    
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sales.Common.Models;

    public class ProductView: Product
    {
        public HttpPostedFileBase ImageFile{ get; set; }

    }
}