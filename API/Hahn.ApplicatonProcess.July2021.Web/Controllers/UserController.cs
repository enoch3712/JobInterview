using Hahn.ApplicationProcess.July2021.Data.Repository;
using Hahn.ApplicationProcess.July2021.Data.Service.Interfaces;
using Hahn.ApplicationProcess.July2021.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Logging;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hahn.ApplicationProcess.July2021.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets the list of all Users.
        /// </summary>
        /// <returns>The list of Users.</returns>
        // GET api/<UserController>
        [HttpGet]
        [EnableQuery]
        [Produces("application/json")]
        public IActionResult Get([FromServices] IUnitOfWork unitOfWork)
        {
            return Ok(((Context)unitOfWork.Context).Users);
        }

        /// <summary>
        /// Gets the user with the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the user with the id.</returns>
        // GET api/<UserController>/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        public IActionResult Get(int id, [FromServices] IUserRepository repository)
        {
            var user = repository.GetById(id);

            if (user is null)
            {
                _logger.LogInformation($"GET NotFound id: {id}");
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Creates a User.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A newly created User</returns>
        /// <remarks>
        /// Sample request:
        ///
        /// POST /user
        /// {
        ///    "age": 20,
        ///    "firstName": "asdad",
        ///    "lastName": "adas",
        ///    "address": "12313",
        ///    "email": "dasd@fsfs.com",
        ///    "assets": [
        ///    {
        ///        "id": "bitcoin",
        ///        "symbol": "BTC",
        ///        "name": "Bitcoin"
        ///    }]
        ///}
        /// </remarks>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If the user is not in the proper format</response>
        // POST api/<UserController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public IActionResult Post([FromBody] User user, [FromServices] IAddUserService service)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"POST BadRequest input: {JsonSerializer.Serialize(user)}");
                return BadRequest();
            }

            var createdUser = service.Execute(user);

            _logger.LogInformation($"POST Successful input: {JsonSerializer.Serialize(user)}");

            return Created($"api/user/{createdUser.Id}", createdUser);
        }

        /// <summary>
        /// Updates the user with the id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public IActionResult Put(int id, [FromBody] User user, [FromServices] IUpdateService service)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"PUT BadRequest input: {JsonSerializer.Serialize(user)}");
                return BadRequest();
            }

            var result = service.Execute(id, user);

            if (result is false)
            {
                _logger.LogInformation($"PUT NotFound id: {id}");
                return NotFound();
            }

            _logger.LogInformation($"PUT Successful input: {JsonSerializer.Serialize(user)}");

            return Ok();
        }

        /// <summary>
        /// Deletes the user with the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteUserService service)
        {
            var result = service.Execute(id);

            if (result is false)
            {
                _logger.LogInformation($"DELETE NotFound id: {id}");
                return NotFound();
            }

            _logger.LogInformation($"DELETE Successful id: {id}");

            return Ok();
        }
    }
}
