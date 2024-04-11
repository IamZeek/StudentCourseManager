using SMS.Models;

namespace SMS.BLL.Services
{
    public interface IStudents
    {
        Task<StudentPage> FetchAll(int pageNumber, int pageSize);
        Task<List<ItemCounts>> StudentsMaxedOut();
        Task<string> SubmitData(Students studentData);
        Task<Students> FetchOne(int Contid);
        Task<string> DeleteData(int Contid);
        Task<string> AddCourse(int courseid, int studentid);
        Task<string> RemoveCourse(int courseid, int studentid);
        Task<string> UpdateData(Students studentData);
        Task<List<Courses>> FetchCourses(int Contid);
    }
}
