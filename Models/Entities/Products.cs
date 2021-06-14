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

        [Key, Display(Name = "�r�n No")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "L�tfen �r�n ad�n� girin."), Display(Name = "�r�n Ad�")]
        [StringLength(40)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "L�tfen bir �retici se�in."), Display(Name = "�retici Firma")]
        public int? SupplierID { get; set; }

        [Required(ErrorMessage = "L�tfen bir kategori se�in."), Display(Name = "Kategori")]
        public int? CategoryID { get; set; }

        [StringLength(20), Required(ErrorMessage = "L�tfen �r�n detay�n� girin."), Display(Name = "�r�n Detay�")]
        public string QuantityPerUnit { get; set; }

        [Required(ErrorMessage = "L�tfen fiyat girin."), Display(Name = "Birim Fiyat ($)")]
        public decimal? UnitPrice { get; set; }
        [Required(ErrorMessage = "L�tfen stok adedi girin."), Display(Name = "Stok Adedi")]
        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }


        [Display(Name = "Sat��a Koy")]
        public bool Activated { get; set; }

        public virtual Categories Categories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }

        public virtual Suppliers Suppliers { get; set; }
    }
}
