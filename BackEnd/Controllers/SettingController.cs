using BackEnd.Core;
using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        Services _services;
        Validators _validators;
        public SettingController(Services services,Validators validators)
        {
            _services = services;
            _validators = validators;
        }
        [HttpPost("SaveSettings")]
        public IActionResult SaveSettings([FromBody] SettingsModel model)
        {
            if (!_validators.IsValidSessionPeriod(model.SessionPeriod))
            {
                return BadRequest(new BaseResponse()
                {
                    Status = false,
                    Message = Constants.NotValidPeriod
                });
            }
            if (!_validators.IsValidWaitingPeriod(model.WaitPeriod))
            {
                return BadRequest(new BaseResponse()
                {
                    Status = false,
                    Message = Constants.NotValidPeriod
                });
            }
            if (!_validators.IsValidRestPeriod(model.RestPeriod))
            {
                return BadRequest(new BaseResponse()
                {
                    Status = false,
                    Message = Constants.NotValidPeriod
                });
            }
            var save = _services.SaveSettings(model);
            if (!save.Result)
            {
                return StatusCode(500, new BaseResponse() { Message = Constants.ExceptionOccoured, Status = false });
            }
            return Ok(new BaseResponse() { Status = true });

        }

        [HttpGet("GetSettings")]
        public IActionResult GetSettings()
        {
            var getSettings = _services.GetSettings();
            if (!getSettings.Result)
            {
                return StatusCode(500, new BaseResponse() { Message = Constants.ExceptionOccoured, Status = false });
            }
            return Ok(new BaseResponse
            {
                Status = true,
                Data = getSettings.Settings
            });
        }
    }
}
