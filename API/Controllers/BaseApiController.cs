using Microsoft.AspNetCore.Mvc;

namespace API.Controllers 
{ 
    [ApiController]
    // This attribute is applied to the class to denote that it is an API controller.
    [Route("/api/[controller]")] // /api/users
    // [controller]: This will take the first part of the name controller where it says Users and use that as the route

    public class BaseApiController : ControllerBase
    // ControllerBase: A base class for an MVC controller without view support.
    // 1. ControllerBase is designed for building APIs, providing a variety of methods and properties 
    // to simplify the development of RESTful services.
    // 2. It does not include view support, which is provided by the Controller class that inherits from ControllerBase.
    {

    }

}