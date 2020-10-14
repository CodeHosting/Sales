namespace Sales.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Description { get; set; }

        
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        
        [Display(Name ="Image")]
        public string ImagePath { get; set; }

        
        
        public Decimal Price { get; set; }

        
        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Publish On")]
        [DataType(DataType.Date)]
        public DateTime PublishOn { get; set; }



        public override string ToString()
        {
            return this.Description;
        }

    }
}
