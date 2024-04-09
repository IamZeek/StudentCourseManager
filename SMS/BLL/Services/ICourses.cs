using SMS.Models;

namespace SMS.BLL.Services
{
    public interface ICourses
    {
        Task<List<Courses>> FetchAll();
        Task<string> SubmitData(Courses courseData);
        Task<Courses> FetchOne(int Contid);
        Task<string> DeleteData(int Contid);
        Task<string> UpdateData(Courses courseData);
        Task<List<Students>> FetchStudents(int Contid);
        Task<List<ItemCounts>> CoursesMaxedOut();
    }
}
