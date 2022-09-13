using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VersaoController : ControllerBase
    {

        public VersaoController()
        {
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(new
            {
                Versao = "0.0.0"
            });
        }
    }
}