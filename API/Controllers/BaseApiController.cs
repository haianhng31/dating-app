using Microsoft.AspNetCore.Mvc;

namespace API.Controllers 
{ 
    [ApiController]
    // This attribute is applied to the class to denote that it is an API controller.
    [Route("/api/[controller]")] // /api/users
    // [controller]: This will take the first part of the name controller where it says Users and use that as the route

    public class BaseApiController : ControllerBase
    {

    }

}