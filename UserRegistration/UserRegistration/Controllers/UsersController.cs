using Microsoft.AspNetCore.Mvc;
using UserRegistration.Models;
using Microsoft.EntityFrameworkCore;

namespace UserRegistration.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Users Users { get; set; }

        public UsersController(ApplicationDbContext db)
        {
            _db = db;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Users = new Users();
            if (id == null)
            {
                //create
                return View(Users);
            }
            //update
            Users = _db.Users_Table.FirstOrDefault(u => u.Id == id);
           
            if (Users == null)
            {
                return NotFound();
            }
            return View(Users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Users.Id == 0)
                {
                    //create
                    _db.Users_Table.Add(Users);
                }
                else
                {
                    _db.Users_Table.Update(Users);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Users);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Users_Table.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bookFromDb = await _db.Users_Table.FirstOrDefaultAsync(u => u.Id == id);
            if (bookFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Users_Table.Remove(bookFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
