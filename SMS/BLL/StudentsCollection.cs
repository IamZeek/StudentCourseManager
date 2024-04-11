using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using SMS.BLL.Services;
using SMS.DAL;
using SMS.Models;


namespace SMS.BLL
{
    public class StudentsCollection : IStudents
    {
        private readonly ApplicationDbContext _context;
        public StudentsCollection(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddCourse(int courseid, int studentid)
        {
            try
            {
                var data = await _context.Students.FindAsync(studentid);
                if (data != null)
                {
                    var tempCheck =  !string.IsNullOrEmpty(data.Courses)? data.Courses.Split("|").ToList() : new List<string>();
                    if (!tempCheck.Contains(courseid.ToString()))
                    {
                        if (tempCheck.Count() == 0)
                        {
                            data.Courses = courseid.ToString();
                        }
                        else
                        {
                            if (tempCheck.Count() < 5)
                            {
                                data.Courses = courseid.ToString() + "|" + data.Courses;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                   
                    _context.SaveChanges();
                    return data.Courses;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> DeleteData(int Contid)
        {
            try
            {
                var model = await _context.Students.FindAsync(Contid);
                if (model != null)
                {
                    _context.Students.Remove(model);
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

        public async Task<StudentPage> FetchAll(int  pageNumber,int pageSize)
        {
            try
            {
                //var data = await _context.Students.ToListAsync();
                //return data;
                var query = await _context.Students
                                 .OrderBy(s => s.Id) // Order by some property (e.g., Id)
                                 .Skip((pageNumber - 1) * pageSize) // Skip items on previous pages
                                 .Take(pageSize)
                                 .ToListAsync(); // Take items for the current page
                var pageCount = Math.Ceiling((double) _context.Students.Count()/pageSize);
                var response = new StudentPage
                {
                    DataFetched = query,
                    CurrentPage = pageNumber,
                    NumberOfPages = (int)pageCount
                };

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Courses>> FetchCourses(int Contid)
         {
            try
            {
                string sql = "EXEC sp_CoursePerStudent @id";
                var parameter = new SqlParameter("@id", Contid);
                var courses = await _context.Courses.FromSqlRaw(sql, parameter).ToListAsync();

                return courses;
            }
            catch (Exception ex)
            {
                 return null;
            }

        }

        public async Task<Students> FetchOne(int Contid)
        {
            try
            {
                var data = _context.Students.FirstOrDefault(x => x.Id == Contid);
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> RemoveCourse(int courseid, int studentid)
        {
            try
            {
                var data = await _context.Students.FindAsync(studentid);
                if (data != null)
                {
                    var tempCheck = !string.IsNullOrEmpty(data.Courses) ? data.Courses.Split("|") : [];
                    if (tempCheck.Length == 1)
                    {
                        data.Courses = "";
                    }
                    else
                    {
                        var tempData = "";
                        foreach (var temp in tempCheck)
                        {
                            if(tempData.Length == 0)
                            {
                                if (temp != courseid.ToString())
                                {
                                    tempData = temp;
                                }
                            }
                            else
                            {

                                if (temp != courseid.ToString())
                                {
                                    tempData += "|" + temp;
                                }
                            }
                        }

                        data.Courses = tempData;
                    }
                    _context.SaveChanges();
                    return data.Courses;
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

        public async Task<List<ItemCounts>> StudentsMaxedOut()
        {
            try
            {
                string sql = "EXEC sp_NumberOfCoursesPerStudent @MaxLength";
                var parameter = new SqlParameter("@MaxLength", 5);
                var courses = await _context.itemCounts.FromSqlRaw(sql,parameter).ToListAsync();

                return courses;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> SubmitData(Students studentData)
        {
            try
            {
                _context.Students.Add(studentData);
                _context.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<string> UpdateData(Students studentData)
        {
            try
            {
                var model = await _context.Students.FirstOrDefaultAsync(x => x.Id == studentData.Id);
                if (model != null)
                {
                    model.FirstName = studentData.FirstName;
                    model.SurName = studentData.SurName;
                    model.DOB = studentData.DOB;
                    model.Address1 = studentData.Address1;
                    model.Address2 = studentData.Address2;
                    model.Address3 = studentData.Address3;
                    _context.Students.Update(model);
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
