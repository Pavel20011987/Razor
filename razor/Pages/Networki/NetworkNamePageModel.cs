using razor.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace razor.Pages.Networki
{
    public class NetworkNamePageModel : PageModel
    {
        public SelectList maskNameSL { get; set; }

        public void PopulateMasksDropDownList(ApplicationDbContext _context,
            object selectedMask = null)
        {
            var masksQuery = from d in _context.Masks
                                   orderby d.mask // Sort by name.
                                   select d;           

            maskNameSL = new SelectList(masksQuery.AsNoTracking(),
                        "MaskID", "mask", selectedMask);
        }
    }
}