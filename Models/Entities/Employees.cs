namespace NorthwindMarket.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Employees
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employees()
        {
            Employees1 = new HashSet<Employees>();
            Orders = new HashSet<Orders>();
            Territories = new HashSet<Territories>();
        }

        [Key]
        public int EmployeeID { get; set; }

        [Required, Display(Name = "Soyad")]
        [StringLength(20)]
        public string LastName { get; set; }

        [Required, Display(Name = "Ad")]
        [StringLength(10)]
        public string FirstName { get; set; }

        [StringLength(30), Required, Display(Name = "Meslek")]
        public string Title { get; set; }

        [StringLength(25)]
        public string TitleOfCourtesy { get; set; }

        [Required, Display(Name = "Doðum Tarihi")]
        public DateTime BirthDate { get; set; }

        [Required, Display(Name = "Ýþe Giriþ Tarihi")]
        public DateTime HireDate { get; set; }

        [StringLength(60), Display(Name = "Adres")]
        public string Address { get; set; }

        [StringLength(15), Display(Name = "Þehir")]
        public string City { get; set; }

        [StringLength(15), Display(Name = "Bölge")]
        public string Region { get; set; }

        [StringLength(10), Display(Name = "Posta Kodu")]
        public string PostalCode { get; set; }

        [StringLength(15), Display(Name = "Ülke")]
        public string Country { get; set; }

        [StringLength(24), Display(Name = "Telefon")]
        public string HomePhone { get; set; }

        [StringLength(4)]
        public string Extension { get; set; }

        [Column(TypeName = "ntext"), Required, Display(Name = "Hakkýmda")]
        public string Notes { get; set; }

        public int? ReportsTo { get; set; }

        [StringLength(20), Required, Display(Name = "Kullanýcý Adý")]
        public string UserName { get; set; }

        [StringLength(20), Required, Display(Name = "Þifre")]
        public string Password { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employees> Employees1 { get; set; }

        public virtual Employees Employees2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Territories> Territories { get; set; }
    }
}
