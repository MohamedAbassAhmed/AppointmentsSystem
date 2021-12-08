using BackEnd.Core;
using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        Services _services;
        Validators _validators;
        public AppointmentsController(Services services, Validators validators)
        {
            _services = services;
            _validators = validators;
        }
        [HttpPost("CreateAppointment")]
        public IActionResult CreateAppointment([FromBody] AppointmentRequest model)
        {
            //if (!_validators.IsDateTime(model.Start))
            //{
            //    return BadRequest(new BaseResponse
            //    {
            //        Message = Constants.NotValidStartDate,
            //        Status = false
            //    });
            //}
            //if (!_validators.IsDateTime(model.End))
            //{
            //    return BadRequest(new BaseResponse
            //    {
            //        Message = Constants.NotValidEndDate,
            //        Status = false
            //    });
            //}
            var result = _services.CreateAppointment(model);
            if (!result.Result)
                return Ok(new BaseResponse
                {
                    Status = result.Result,
                    Message = Constants.NoAvilabeAppointment
                });
            else
            {
                return Ok(new BaseResponse
                {
                    Status = result.Result,
                    Data = result.AppointmentDate.ToString("dd-MM-yyyy HH:mm")
                });
            }
        }
        [HttpGet("GetAppointments")]
        public IActionResult GetAppointments()
        {
            var getAppointments = _services.GetAppointments();
            if (!getAppointments.Result)
            {
                return StatusCode(500, new BaseResponse()
                {
                    Status = false,
                    Message = Constants.ExceptionOccoured
                });
            }
            return Ok(new BaseResponse()
            {
                Status = true,
                Data=getAppointments.Appointments
            });
        }
    }
}
