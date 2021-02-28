namespace WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Sale
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [Required]
        public int Client_Id { get; set; }

        [Required]
        public int Item_Id { get; set; }

        [Required]
        public int Manager_Id { get; set; }

        public virtual Client Client { get; set; }

        public virtual Item Item { get; set; }

        public virtual Manager Manager { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
