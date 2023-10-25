using System;

namespace ReportPrint.Model
{
    internal class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Birth { get; set; }
        public string Sex { get; set; }
        public int Age => DateTime.Now.Year - Birth.Year;
    }
}
