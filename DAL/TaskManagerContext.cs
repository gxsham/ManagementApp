using System.Data.Entity;
using Domain;
namespace DAL
{
	public class TaskManagerContext: DbContext
	{
		public TaskManagerContext():base("TaskManagerDb")
		{
		}

		public DbSet<Project> Projects { get; set; }
		public DbSet<Member> Members { get; set; }
		public DbSet<Assignment> Assignments { get; set; }
	}
}