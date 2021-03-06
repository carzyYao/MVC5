﻿using MVC5.Models;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace MVC5.Controllers
{
    public class DepartmentsController : BaseController
    {
        DepartmentRepository repo;
        PersonRepository repoPerson;
        CourseRepository repoCourse;

        public DepartmentsController()
        {
            repo = RepositoryHelper.GetDepartmentRepository();
            repoPerson = RepositoryHelper.GetPersonRepository(repo.UnitOfWork);
            repoCourse = RepositoryHelper.GetCourseRepository(repo.UnitOfWork);
        }

        // GET: Departments
        public ActionResult Index()
        {
            return View(repo.All());
        }

        public ActionResult Create()
        {
            ViewBag.InstructorID = new SelectList(repoPerson.All(), "ID", "FirstName");

            return View();
        }

        [HttpPost]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                repo.Add(department);
                repo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            ViewBag.InstructorID = new SelectList(repoPerson.All().OrderBy(p => p.FirstName), "ID", "FirstName");

            return View(department);
        }

        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            var dept = repo.All().FirstOrDefault(p => p.DepartmentID == id.Value);

            ViewBag.InstructorID = new SelectList(repoPerson.All().OrderBy(p => p.FirstName), "ID", "FirstName", dept.InstructorID);

            return View(dept);
        }

        [HttpPost]
        public ActionResult Edit(int id, DepartmentEdit department)
        {
            if (ModelState.IsValid)
            {
                var item = repo.GetOne(id);

                item.InjectFrom(department);

                repo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            var dept = repo.GetOne(id);

            ViewBag.InstructorID = new SelectList(repoPerson.All(), "ID", "FirstName", dept.InstructorID);

            return View(dept);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dept = repo.GetOne(id.Value);

            if (dept == null)
            {
                return HttpNotFound();
            }

            return View(dept);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dept = repo.GetOne(id.Value);

            if (dept == null)
            {
                return HttpNotFound();
            }

            return View(dept);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection form)
        {
            var dept = repo.GetOne(id);
            repo.Delete(dept);
            repo.UnitOfWork.Commit();

            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult CoursesUnderDetails(int id)
        {
            var data = repoCourse.All().Where(p => p.DepartmentID == id);
            return PartialView(data);
        }
    }
}