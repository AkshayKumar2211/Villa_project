using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Villa_project.Domain.Entities;

namespace Villa_project.ViewModels
{
    public class AmenityVM
    {
        public Amenity? Amenity { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Villalist { get; set; }
    }
}
