using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCGastenBoek.Custom;
using MVCGastenBoek.Database;
using MVCGastenBoek.Models;

namespace MVCGastenBoek.Controllers
{
    public class CommentsController : Controller
    {
        private readonly GastenBoekContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        //private readonly UserManager<IdentityUser> userManager;
        public CommentsController(GastenBoekContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        [Authorize]
        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var vm = new CommentIndexViewModel() 
            {
                 Comments = await _context.Comments.ToListAsync(),
                 Comment = new CommentViewModel()
            };
            return View(vm);
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            return View(new CommentViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFromIndex([Bind("Comment")] CommentIndexViewModel commentIndexViewModel)
        {
            if (ModelState.IsValid)
            {
                Comment newComment = new Comment();
                ViewModelToModel(commentIndexViewModel.Comment, newComment);
                _context.Add(newComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(commentIndexViewModel);
        }
        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,Naam,Created,ImgPath,Mood,NewImage,MyComment,Title")] CommentViewModel commentVM)
        {
            if (ModelState.IsValid)
            {
                Comment newComment = new Comment();
                ViewModelToModel(commentVM, newComment);
                _context.Add(newComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(commentVM);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(ModelToViewModel(comment));
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentId,Naam,Created,ImgPath,Mood,NewImage,MyComment,Title")] CommentViewModel commentVM)
        {
            if (id != commentVM.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var DbComment = _context.Find<Comment>(id);
                    if (DbComment != null)
                        ViewModelToModel(commentVM, DbComment);
                    _context.Update(DbComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(commentVM.CommentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(commentVM);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }
        private CommentViewModel ModelToViewModel(Comment comment)
        {
            return ObjMethods.CopyProperties<Comment, CommentViewModel>(comment);
        }
        private void ViewModelToModel(CommentViewModel commentVM, Comment comment)
        {
            ObjMethods.EditProperties(commentVM, comment);
            if (String.IsNullOrEmpty(comment.Naam))
                comment.Naam = User.FindFirstValue(ClaimTypes.Name);
            comment.Created = DateTime.Now;
            if (commentVM.NewImage != null)
            {
                if (!String.IsNullOrEmpty(comment.ImgPath))
                    DeleteFoto(comment.ImgPath);
                comment.ImgPath = "/" + Path.Combine("Images", UploadFoto(commentVM.NewImage));
            }
        }

        private string UploadFoto(IFormFile foto)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(foto.FileName);
            string pathName = Path.Combine(_hostEnvironment.WebRootPath, "Images");
            string fileNameWithPath = Path.Combine(pathName, uniqueFileName);
            foto.CopyTo(new FileStream(fileNameWithPath, FileMode.Create));
            return uniqueFileName;
        }
        private void DeleteFoto(string filename)
        {
            string path = Path.Combine(_hostEnvironment.WebRootPath, filename.Substring(1));
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }
    }
}
