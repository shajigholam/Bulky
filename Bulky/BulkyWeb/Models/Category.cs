using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
	public class Category
	{
		// if we use Id or name of the model+Id we donnot need data annotation for PK
		[Key]
		public int Id { get; set; }
		[Required]
		[DisplayName("Category Name")]
		public string Name { get; set; }
		[DisplayName("Display Order")]
		public int DisplayOrder { get; set; }
	}
}

