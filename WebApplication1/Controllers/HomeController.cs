using ConfigService.Database;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using WebApplication1.Data;
using WebApplication1.Inputs;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;
        private readonly IEntityRepository<Actor, Guid> _actorRepository;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context, IEntityRepository<Actor, Guid> actorRepository)
        {
            _logger = logger;
            _context = context;
            _actorRepository = actorRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var test = await _actorRepository.FindAllByConditionAsync(x => x.Where(y => true), false);
            return View();
        }


        public IActionResult JSONExample()
        {
            return View();
        }

        public IActionResult DocumentsExample()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostJson(DataToBeSerializedInput inputdata)
        {
            var test = (JsonSerializer.Serialize(inputdata));
            //var actor = new Actor()
            //{
            //    Name = "test",
            //    AnnualIncome = "1900000",
            //    AdditionalInformation = JsonDocument.Parse(JsonSerializer.Serialize(inputdata))
            //};
            //_context.Actors.AddAsync(actor);
            //_context.SaveChanges();



            var firstActor = _context.Actors.First(x => true);
            var JsonElement = firstActor.AdditionalInformation.RootElement;
            JsonNode jo = JsonNode.Parse(firstActor.AdditionalInformation.RootElement.GetRawText());
            return View("Index");
        }



        //public async Task<IActionResult> PostDocument(IFormFile fileUpload)
        //{
        //    return Ok();

        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
