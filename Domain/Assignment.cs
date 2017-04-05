using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Domain
{
	public class Assignment: Entity
	{
		[Display(Name = "Task Description")]
		[Required]
		public string Description { get; set; }
		[Range(1,8)]
		public int Hours { get; set; }
		public virtual Project Project { get; set; }
		[Display(Name = "Project Name")]
		public virtual int ProjectId { get; set; }
		public virtual Member Member { get; set; }
		[Display(Name = "Member Name")]
		public virtual int MemberId { get; set; }

		public Assignment()
		{

		}

		public Assignment(Assignment extendedAssignment,Member member,Project project, int extraHours)
		{
			Description = extendedAssignment.Description;
			Hours = extraHours;
			Project = project;
			ProjectId = project.Id;
			Member = member;
			MemberId = member.Id; 
		}
	}

}