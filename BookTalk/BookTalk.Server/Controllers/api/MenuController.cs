using BookTalk.BusinessLogic.Interfaces;
using BookTalk.BusinessLogic.Services;
using BookTalk.Shared.Common;
using BookTalk.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookTalk.Server.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        public readonly MenuService _menuService;

        public MenuController(MenuService menuService)
        {
            // DBContext 클래스와 관련된 별도의 작업이 없어도
            // menuService 인스턴스에는 DBContext 타입이 주입되어 있다.
            _menuService = menuService;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            ResponseMessage<IEnumerable<Menu>> responseData = new ResponseMessage<IEnumerable<Menu>>();

            try
            {
                responseData.Data = _menuService.GetAll();
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                responseData.ErrorCode = "-1";
                responseData.DeveloperErrorMessage = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseData);
            }
        }
    }
}
