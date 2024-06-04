﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Simbapetite.Core.Domain.Entities
{
	public class OrderDetails
	{
		[Key]
		public int OrderDetailId { get; set; }
		[Required]
		public int OrderHeaderId { get; set; }
		[Required]
		public int MenuItemId { get; set; }
		[ForeignKey("MenuItemId")]
		public MenuItem MenuItem { get; set; }
		[Required]
		public int Quantity { get; set; }
		[Required]
		public string ItemName { get; set; }
		[Required]
		public double Price { get; set; }
	}
}
