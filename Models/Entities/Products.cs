namespace NorthwindMarket.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        [Key, Display(Name = "Ürün No")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Lütfen ürün adýný girin."), Display(Name = "Ürün Adý")]
        [StringLength(40)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Lütfen bir üretici seçin."), Display(Name = "Üretici Firma")]
        public int? SupplierID { get; set; }

        [Required(ErrorMessage = "Lütfen bir kategori seçin."), Display(Name = "Kategori")]
        public int? CategoryID { get; set; }

        [StringLength(20), Required(ErrorMessage = "Lütfen ürün detayýný girin."), Display(Name = "Ürün Detayý")]
        public string QuantityPerUnit { get; set; }

        [Required(ErrorMessage = "Lütfen fiyat girin."), Display(Name = "Birim Fiyat ($)")]
        public decimal? UnitPrice { get; set; }
        [Required(ErrorMessage = "Lütfen stok adedi girin."), Display(Name = "Stok Adedi")]
        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }


        [Display(Name = "Satýþa Koy")]
        public bool Activated { get; set; }

        public virtual Categories Categories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }

        public virtual Suppliers Suppliers { get; set; }
    }
}
