﻿using Antlr.Runtime.Misc;
using MVC5.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace MVC5.Controllers
{
    public class ReportsController : Controller
    {
        ContosoUniversityEntities db = new ContosoUniversityEntities();


        public ReportsController()
        {
            db.Database.Log = (msg) =>
            {
                Debug.WriteLine("-----------------------------------------");
                Debug.WriteLine(msg);
                Debug.WriteLine("-----------------------------------------");
            };
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CoursesReport1()
        {
            var data = (from c in db.Course
                        select new CoursesReport1VM()
                        {
                            CourseID = c.CourseID,
                            CourseName = c.Title,
                            StudentCount = c.Enrollments.Count(),
                            TeacherCount = c.Teachers.Count(),
                            AvgGrade = c.Enrollments.Where(e => e.Grade.HasValue).Average(e => e.Grade.Value)
                        }).ToList();

            return View(data);
        }

        public ActionResult CoursesReport2()
        {
            var data = db.Database.SqlQuery<CoursesReport1VM>(@"
SELECT
    Course.CourseID, 
    Course.Title AS CourseName,
	(SELECT COUNT(CourseID) FROM CourseInstructor WHERE (CourseID = Course.CourseID)) AS TeacherCount,
	(SELECT COUNT(CourseID) FROM Enrollment WHERE (Course.CourseID = Enrollment.CourseID)) AS StudentCount,
	(SELECT AVG(Cast(Grade as Float)) FROM Enrollment WHERE (Course.CourseID = Enrollment.CourseID)) AS AvgGrade
FROM   Course
GROUP BY Course.CourseID, Course.Title");

            return View("CoursesReport1", data);
        }

        public ActionResult CoursesReport3(int? id)
        {
            if(!id.HasValue)
                return RedirectToAction("CoursesReport1");

            var data = db.Database.SqlQuery<CoursesReport1VM>(@"
SELECT
    Course.CourseID, 
    Course.Title AS CourseName,
	(SELECT COUNT(CourseID) FROM CourseInstructor WHERE (CourseID = Course.CourseID)) AS TeacherCount,
	(SELECT COUNT(CourseID) FROM Enrollment WHERE (Course.CourseID = Enrollment.CourseID)) AS StudentCount,
	(SELECT AVG(Cast(Grade as Float)) FROM Enrollment WHERE (Course.CourseID = Enrollment.CourseID)) AS AvgGrade
FROM   Course
WHERE  Course.CourseID = @p0
GROUP BY Course.CourseID, Course.Title", id);

            return View("CoursesReport1", data);
        }
    

    }
}