using Hahn.ApplicationProcess.July2021.Data.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hahn.ApplicationProcess.July2021.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IGetAssetsService _getAssetsService;

        public AssetsController(IGetAssetsService getAssetsService)
        {
            _getAssetsService = getAssetsService;
        }

        // GET: api/<AssetsController>
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get() => Ok((await _getAssetsService.ExecuteAsync()).AsQueryable());
    }
}
