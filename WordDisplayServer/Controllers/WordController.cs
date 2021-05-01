using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace WordDisplayServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WordController : ControllerBase
    {
        private WordService wordService;
        public WordController(WordService wordService) => this.wordService = wordService;

        [HttpGet]
        public IActionResult Get() {
            wordService.Notify();
            return Ok();
        }
        [HttpPost]
        public IActionResult Post(JObject payload) {
            try
            {
                var data = payload["Words"].Value<JArray>();
                wordService.Start(data);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
