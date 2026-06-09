using Microsoft.AspNetCore.Mvc;

namespace Commerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController: ControllerBase
    {
    }
}
