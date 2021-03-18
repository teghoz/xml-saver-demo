using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xml_demo_saver.Models;
using xml_demo_saver.Repository;

namespace xml_demo_saver.Pages
{
    public class EditModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IRepository _repository;

        public EditModel(ILogger<IndexModel> logger, IRepository repository)
        {
            _repository = repository;
            _logger = logger;
        }

        public void OnGet(int userId)
        {
            var filter = new UserFilter { Id = userId };
            PassedUser = _repository.Users(filter).FirstOrDefault();
        }

        public IActionResult OnPost()
        {
            _repository.UpdateUser(PassedUser);
            return RedirectToPage("Index");
        }

        [BindProperty]
        public User PassedUser { get; set; }
    }
}
