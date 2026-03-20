using ConfigService.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using WebApplication1.Data;
using WebApplication1.Inputs;
using WebApplication1.Models;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{

    public class DocumentController : Controller
    {
        private readonly ILogger<DocumentController> _logger;
        private readonly ApplicationContext _context;
        private readonly IEntityRepository<Actor, Guid> _actorRepository;
        //private readonly IService<DocumentCreateInput, DocumentUpdateInput, Document, Guid> _actorService;
        private readonly IService<DocumentCreateInput, DocumentUpdateInput, Document, Guid> _documentService;


        public DocumentController(ILogger<DocumentController> logger, ApplicationContext context,
            IEntityRepository<Actor, Guid> actorRepository,
            //IService<DocumentCreateInput, DocumentUpdateInput, Document, Guid> actorService)
            IService<DocumentCreateInput, DocumentUpdateInput, Document, Guid> documentService)
        {
            _logger = logger;
            _context = context;
            _actorRepository = actorRepository;
            _documentService = documentService;
        }

        

        [HttpPost]
        public async Task<IActionResult> PostDocument(IFormFile fileUpload)
        {
            var test = _context.Actors;
            var filename = fileUpload.FileName;
            var test2 = await _actorRepository.FindByConditionAsync(x => x.Where(y => y.AnnualIncome == "1000"), false);
            using var ms = new MemoryStream();
            await fileUpload.CopyToAsync(ms);
            byte[] bytes = ms.ToArray();
            var shapedInput = new DocumentCreateInput() { BinaryData = bytes, DocumentName = filename };
            await _documentService.CreateAsync(shapedInput);
            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
