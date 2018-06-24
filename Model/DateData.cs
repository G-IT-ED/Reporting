using System;

namespace Reporting.Model
{
    class DateData
    {
        public DateData(DateTime dateStart, DateTime dateFinish, int idObject)
        {
            DateStart = dateStart;
            DateFinish = dateFinish;
            IdObject = idObject;
        }

        public DateTime DateStart { get; set; }
        public DateTime DateFinish { get; set; }
        public int IdObject { get; set; }
    }
}
