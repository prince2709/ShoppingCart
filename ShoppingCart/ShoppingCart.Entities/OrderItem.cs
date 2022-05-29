using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Entities
{
    [Table("OrderItem")]
    public class OrderItem
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }
        public int ProductID { get; set; }
        public int ProductQuantity { get; set; }
        public Product Product { get; set; }
        [NotMapped]
        public decimal TotalOrderItemPrice { get { return (Product.UnitPrice * ProductQuantity); } }

    }
}
