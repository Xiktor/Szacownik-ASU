using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class SuccessPageModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public double Result { get; set; }
        public string? Prediction { get; set; }

        public void OnGet()
        {
            if (Result != null)
            {
                Prediction = Result.ToString();
            }
        }
    }
}
