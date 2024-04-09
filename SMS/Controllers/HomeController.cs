using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using SMS.BLL.Services;
using SMS.DAL;
using SMS.Models;
using System.Diagnostics;

namespace SMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICourses _courses;
        private readonly IStudents _students;

        public HomeController(ILogger<HomeController> logger, ICourses courses,IStudents students)
        {
            _logger = logger;
            _courses = courses;
            _students = students;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeView(string name)
        {
            if(!string.IsNullOrEmpty(name))
            {
                switch (name)
                {
                    case "partialview_Home":
                        var studentsMaxedOutData = await _students.StudentsMaxedOut();
                        var coursesMaxedOutData = await _courses.CoursesMaxedOut();
                        var data = new
                        {
                            StudentMaxout = studentsMaxedOutData != null? studentsMaxedOutData.Where(x=> x.ExceedsMaxCount == 1).ToList() : [],
                            CourseMaxOut = coursesMaxedOutData != null? coursesMaxedOutData.Where(x => x.ExceedsMaxCount == 1).ToList(): []
                        };
                        TempData["StudentMaxout"] = data.StudentMaxout.Count();
                        TempData["CourseMaxOut"] = data.CourseMaxOut.Count();
                        return PartialView("PartialViews/partialview_Home", studentsMaxedOutData);

                    case "partialview_Students":
                        var studentsData = await _students.FetchAll();
                        return PartialView("PartialViews/partialview_Students", studentsData);

                    case "partialview_Courses":
                        var courseData = await _courses.FetchAll();
                        return PartialView("PartialViews/partialview_Courses", courseData);
                    default:
                        return View();
                }
            }
            else
            {
                return PartialView("./PartialViews/parialview_Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitCourse(Courses formData)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var submitData = _courses.SubmitData(formData);
                    return Json(submitData);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
            else
            {
                return Json("Data not Parsed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitStudent(Students formData)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var submitData = _students.SubmitData(formData);
                    return Json(submitData);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
            else
            {
                return Json("Data not Parsed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCourse(Courses formData)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var submitData = await _courses.UpdateData(formData);
                    return Json(submitData);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
            else
            {
                return Json("Data not Parsed");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStudent(Students formData)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var submitData = await _students.UpdateData(formData);
                    return Json(submitData);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
            else
            {
                return Json("Data not Parsed");
            }
        }

        [HttpPost]
        public ActionResult FetchOne(int id, string modelType, string modelTransType)
        {
            try
            {
                if(modelTransType == "coursesModel")
                {
                    var submitData = _courses.FetchOne(id);
                    return Json(submitData.Result);
                }
                else
                {
                    var submitData = _students.FetchOne(id);
                    return Json(submitData.Result);
                }
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult DeleteData(int id, string modelType)
        {
            try
            {
                if (modelType == "coursesModel")
                {
                    var submitData = _courses.DeleteData(id);
                    return Json(submitData.Result);
                }
                else
                {
                    var submitData = _students.DeleteData(id);
                    return Json(submitData.Result);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> FetchCoursesPerStudent(int id, string transType)
        {
            if(transType == "CoursePerStudent")
            {
                try
                {
                    var fetchedCoursesPerStudent = await _students.FetchCourses(id);
                    var tempidlist = fetchedCoursesPerStudent.Select(s => s.Id).ToList();
                    var fetchedCourses = await _courses.FetchAll();
                    var combinedData = new
                    {
                        CoursesPerStudent = fetchedCoursesPerStudent,
                        Courses = fetchedCourses.Where(course => !tempidlist.Contains(course.Id)).ToList()

                    };
                    return Json(combinedData);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
            else
            {
                try
                {
                    var fetchedStudentPerCourse = await _courses.FetchStudents(id);
                    var tempidlist = fetchedStudentPerCourse.Select(s => s.Id).ToList();
                    var fetchStudents = await _students.FetchAll();
                    var combinedDataStudentsPerCorse = new
                    {
                        StudentPerCourse = fetchedStudentPerCourse,
                        Students = fetchStudents.Where(students => !tempidlist.Contains(students.Id)).ToList()

                    };
                    return Json(combinedDataStudentsPerCorse);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddCourseToStudent(int courseid, int studentid,string transType)
        {
            try
            {
                var submitData = await _students.AddCourse(courseid,studentid);
                if(submitData != null)
                {
                    if (transType == "CoursePerStudent")
                    {
                        var fetchNew = await _courses.FetchOne(courseid);
                        return Json(fetchNew);
                    }
                    else
                    {
                        var fetchNew = await _students.FetchOne(studentid);
                        return Json(fetchNew);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> RemoveCourseToStudent(int courseid, int studentid,string transType)
        {
            try
            {
                var submitData = await _students.RemoveCourse(courseid, studentid);
                if (submitData != null)
                {
                    if(transType == "CoursePerStudent")
                    {
                        var fetchNew = await _courses.FetchOne(courseid);
                        return Json(fetchNew);
                    }
                    else
                    {
                        var fetchNew = await _students.FetchOne(studentid);
                        return Json(fetchNew);
                    }
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
