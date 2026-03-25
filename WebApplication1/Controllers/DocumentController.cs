using ConfigService.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using WebApplication1.Data;
using WebApplication1.Inputs;
using WebApplication1.Models;
using WebApplication1.Service.AuthService;
using WebApplication1.Service.DBService;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplication1.Controllers;

public class DocumentModel
{ 
    public Guid Id { get; set; }
    public string DocumentName { get; set; }
    public byte[] BinaryData { get; set; }
    public string Token { get; set; }
}




public class DocumentController : Controller
{
    private readonly ILogger<DocumentController> _logger;
    private readonly ApplicationContext _context;
    private readonly IEntityRepository<Actor, Guid> _actorRepository;
    //private readonly IService<DocumentCreateInput, DocumentUpdateInput, Document, Guid> _actorService;
    private readonly IService<DocumentCreateInput, DocumentUpdateInput, Document, Guid> _documentService;
    private readonly TokenService _tokenService;

    public DocumentController(ILogger<DocumentController> logger, ApplicationContext context,
        IEntityRepository<Actor, Guid> actorRepository,
        //IService<DocumentCreateInput, DocumentUpdateInput, Document, Guid> actorService)
        IService<DocumentCreateInput, DocumentUpdateInput, Document, Guid> documentService, TokenService tokenService)
    {
        _logger = logger;
        _context = context;
        _actorRepository = actorRepository;
        _documentService = documentService;
        _tokenService = tokenService;
    }

    public IActionResult DocumentsExample()
    {
     
       List<DocumentModel> listResult = _context.Documents.Select(x => new DocumentModel{
           Id = x.Id, DocumentName = x.DocumentName, BinaryData = x.BinaryData,
           Token = _tokenService.GenerateToken("fddsfdf", "test")
       }).ToList();

       
        return View(listResult);
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

    [HttpGet]
    public async Task<IActionResult> GetDocument(Guid id, string token)
    {
        var documentData = _context.Documents.First(x => x.Id == id);

        var principal = _tokenService.ValidateToken(token);

        if (principal == null)
            return Unauthorized();

        // ✅ Token is valid — optionally read claims
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return File(documentData.BinaryData, "image/png", documentData.DocumentName);

        //var test = _context.Actors;
        //var filename = fileUpload.FileName;
        //var test2 = await _actorRepository.FindByConditionAsync(x => x.Where(y => y.AnnualIncome == "1000"), false);
        //using var ms = new MemoryStream();
        //await fileUpload.CopyToAsync(ms);
        //byte[] bytes = ms.ToArray();
        //var shapedInput = new DocumentCreateInput() { BinaryData = bytes, DocumentName = filename };
        //await _documentService.CreateAsync(shapedInput);
        //return RedirectToAction("Index", "Home");
        //return RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
