using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace interview2.Models
{
    public class Employee
    {
        public int ID { get; set; }

        
        public string? NAME { get; set; }
        public string? POSITION { get; set; }
        public decimal SALARY { get; set; }
        public string? DEPARTMENT { get; set; }
    }
}
