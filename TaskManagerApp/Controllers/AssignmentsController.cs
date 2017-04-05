using DAL;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace TaskManagerApp.Controllers
{
    public class AssignmentsController : Controller
    {
        private TaskManagerContext db = new TaskManagerContext();

        // GET: Assignments
        public ActionResult Index()
        {
            var assignments = db.Assignments.Include(a => a.Member).Include(a => a.Project);
            return View(assignments.ToList());
        }

        // GET: Assignments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // GET: Assignments/Create
        public ActionResult Create()
        {
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Name");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Hours,ProjectId,MemberId")] Assignment assignment)
        {
            if (ModelState.IsValid && LoadBalancer(assignment))
            {
				//LoadBalancer(assignment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MemberId = new SelectList(db.Members, "Id", "Name", assignment.MemberId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", assignment.ProjectId);
            return View(assignment);
        }
		
		[NonAction]
		public bool LoadBalancer(Assignment assignment)
		{
			Member member = db.Members.Find(assignment.MemberId);
			int workingHours = member.Assignments.Select(x => x.Hours).Sum();

			if(assignment.Hours + workingHours <= 8)
			{
				db.Assignments.Add(assignment);
				return true;
			}
			else
			{
				int extraHours = workingHours + assignment.Hours - 8;
				assignment.Hours =  assignment.Hours - extraHours ;
				db.Assignments.Add(assignment);

				var members = db.Members.ToList();
				foreach (var item in members)
				{
					var element = db.Members.Find(item.Id);
					int freeHours = 8 - element.Assignments.Select(x => x.Hours).Sum();
					//assign task to one of the members
					if (freeHours > extraHours)
					{
						var proj = db.Projects.Find(assignment.ProjectId);
						Assignment newAssignment = new Assignment(assignment, element,proj, extraHours);
						db.Assignments.Add(newAssignment);
						return true;
					}
				}
				return false; 
			}
		}
        // GET: Assignments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Name", assignment.MemberId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", assignment.ProjectId);
            return View(assignment);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Hours,ProjectId,MemberId")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Name", assignment.MemberId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", assignment.ProjectId);
            return View(assignment);
        }

        // GET: Assignments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Assignment assignment = db.Assignments.Find(id);
            db.Assignments.Remove(assignment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
