using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Domain
{
	
	public class Member:Entity
	{
		[Display(Name = "Member Name")]
		public string Name { get; set; }
		public virtual ICollection<Assignment> Assignments { get; set; }
	}
}