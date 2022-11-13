using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using razor.Models;
using Microsoft.AspNetCore.Authorization;

namespace razor.Pages.Networki
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : NetworkNamePageModel
    {
        private readonly razor.Data.ApplicationDbContext _context;
        string oldMask="";
        //Logger log;
        public CreateModel(razor.Data.ApplicationDbContext context)
        {
            _context = context;            
        }

        public IActionResult OnGet()
        {
            PopulateMasksDropDownList(_context);
            return Page();
        }

        [BindProperty]
        public Network Network { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
        var emptyCourse = new Network();

            if (await TryUpdateModelAsync<Network>(
                emptyCourse,
                "Network",   // Prefix for form value.
                s => s.NetworkID,
                s => s.network,
                s => s.allocatedIP,
                s => s.MaskID, 
                s => s.Mask, 
                s => s.Nomadizm,                
                s => s.InUseNet)
                )
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }         

                foreach(Network net in _context.Networks)
                {
                    if(net.NetworkID== emptyCourse.NetworkID){
                        ModelState.AddModelError(string.Empty, 
                        $"Pool {emptyCourse.NetworkID} has already been added.");
                        PopulateMasksDropDownList(_context, emptyCourse.MaskID);
                        return Page() ;
                    }
                    
                }
                _context.Networks.Add(emptyCourse);
                await _context.SaveChangesAsync();
                Logger log = new Logger();
                foreach(Mask m in _context.Masks)
                {
                    if(m.MaskID == emptyCourse.MaskID){
                        oldMask = m.mask;
                    }
                }                
                log.LogAddNet(User.Identity.Name,emptyCourse.network,oldMask);
                return RedirectToPage("./Index");
            }

            // Select DepartmentID if TryUpdateModelAsync fails.
            PopulateMasksDropDownList(_context, emptyCourse.MaskID);

            return Page();
        }
    }
}
