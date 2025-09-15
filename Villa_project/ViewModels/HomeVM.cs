using Villa_project.Domain.Entities;

namespace Villa_project.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Villa>? VillaList { get; set; } 
        public DateOnly CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set;}

        public int Nights {  get; set; }    
    }
}
