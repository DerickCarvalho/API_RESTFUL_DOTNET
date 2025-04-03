using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiReservas.Controllers
{
    [ApiController]
    [Route("api/testar_autenticacao")]
    [Authorize]
    public class TestJwt : ControllerBase
    {
        [HttpGet]
        public IActionResult GetMessage()
        {
            return Ok(new { message = "Você tem acesso à esse endpoint pois está logado!" });
        }
    }
}
