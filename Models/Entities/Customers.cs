namespace NorthwindMarket.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customers()
        {
            Orders = new HashSet<Orders>();
        }

        [Key]
        [Required(ErrorMessage = "L�tfen bir ID sa�lay�n.")]
        [MinLength(5), MaxLength(5), Display(Name = "M��teri ID")]
        public string CustomerID { get; set; }

        [Required(ErrorMessage = "L�tfen bir �irket ad� sa�lay�n.")]
        [StringLength(40), Display(Name = "�irket Ad�")]
        public string CompanyName { get; set; }

        [StringLength(30), Required(ErrorMessage = "L�tfen isim ve soyisim yaz�n."), Display(Name = "Ki�i Ad�")]
        public string ContactName { get; set; }

        [StringLength(30), Display(Name = "�leti�im Ba�l���")]
        public string ContactTitle { get; set; }

        [StringLength(60), Display(Name = "Adres")]
        public string Address { get; set; }

        [StringLength(15), Display(Name = "�ehir")]
        public string City { get; set; }

        [StringLength(15), Display(Name = "B�lge")]
        public string Region { get; set; }

        [StringLength(10), Display(Name = "Posta Kodu")]
        public string PostalCode { get; set; }

        [StringLength(15), Display(Name = "�lke")]
        public string Country { get; set; }

        [StringLength(24), Display(Name = "Telefon")]
        public string Phone { get; set; }

        [StringLength(24), Display(Name = "Faks")]
        public string Fax { get; set; }

        [StringLength(20), Required]
        public string Password { get; set; }

        public bool Activated { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders { get; set; }


    }
}
