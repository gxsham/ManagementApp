using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Domain
{
	public class Project:Entity
	{
		[Display(Name = "Project Name")]
		public string Name { get; set; }
		public virtual ICollection<Member> Members { get; set; }
		public virtual ICollection<Assignment> Assignments { get; set; }
	}
}