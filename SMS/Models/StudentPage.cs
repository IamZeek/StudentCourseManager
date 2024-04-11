namespace SMS.Models
{
    public class StudentPage
    {
        public List<Students> DataFetched { get; set; }  = new List<Students>();
        public int NumberOfPages { get; set; }
        public int CurrentPage { get; set; }
    }

}
