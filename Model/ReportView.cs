namespace Reporting.Model
{
    public class ReportView
    {

        public ReportView(string name, int count)
        {
            Name = name;
            Count = count;
        }

        public string Name { get; set; }
        public int Count { get; set; }
    }
}
