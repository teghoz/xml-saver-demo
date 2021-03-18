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
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IRepository _repository;

        public IndexModel(ILogger<IndexModel> logger, IRepository repository)
        {
            _repository = repository;
            _logger = logger;
        }

        public void OnGet(UserFilter filter)
        {
            Users = _repository.Users(filter);
        }

        public IActionResult OnGetDelete(int id)
        {
            _repository.DeleteUser(id);
            return RedirectToPage("Index");
        }

        public List<User> Users { get; set; }
    }
}
