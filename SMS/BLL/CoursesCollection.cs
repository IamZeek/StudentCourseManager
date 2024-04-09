using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMS.BLL.Services;
using SMS.DAL;
using SMS.Models;

namespace SMS.BLL
{
    public class CoursesCollection : ICourses
    {
        private readonly ApplicationDbContext _context;

        public CoursesCollection(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ItemCounts>> CoursesMaxedOut()
        {
            try
            {
                string sql = "EXEC sp_NumberOfCoursesPerStudent @MaxLength";
                var parameter = new SqlParameter("@MaxLength", 5);
                var courses = await _context.itemCounts.FromSqlRaw(sql, parameter).ToListAsync();

                return courses;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> DeleteData(int Contid)
        {
            try
            {
                var model = await _context.Courses.FindAsync(Contid);
                if (model != null)
                {
                    _context.Courses.Remove(model);
                    await _context.SaveChangesAsync();
                    return "Success";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<List<Courses>> FetchAll()
        {
            try
            {
                var data = await _context.Courses.ToListAsync();
                return data;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<Courses> FetchOne(int Contid)
        {
            try
            {
                var data = _context.Courses.FirstOrDefault(x => x.Id == Contid);
                return data;
            }
            catch(Exception ex )
            {
                return null;
            }
        }

        public async Task<List<Students>> FetchStudents(int Contid)
        {
            try
            {
                string sql = "EXEC sp_StudentPerCourse @id";
                var parameter = new SqlParameter("@id", Contid);
                var students = await _context.Students.FromSqlRaw(sql, parameter).ToListAsync();

                return students;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<string> SubmitData(Courses courseData)
        {
            try
            {
                _context.Courses.Add(courseData);
                _context.SaveChanges();
                return "Success";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> UpdateData(Courses courseData)
        {
            try
            {
                var model = await _context.Courses.FirstOrDefaultAsync( x => x.Id == courseData.Id);
                if (model != null)
                {
                    model.CourseCode = courseData.CourseCode;
                    model.CourseName = courseData.CourseName;
                    model.StartDate = courseData.StartDate;
                    model.EndDate = courseData.EndDate;
                    _context.Courses.Update(model);
                    await _context.SaveChangesAsync();
                    return "Success";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
