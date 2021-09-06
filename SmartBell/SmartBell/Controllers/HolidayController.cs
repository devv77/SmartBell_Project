using Logic;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBell.Controllers
{
    [ApiController]
    [Route("Holiday")]
    public class HolidayController : ControllerBase
    {
        ModificationLogic modlogic;
        ReadLogic readlogic;

        public HolidayController(ModificationLogic logic, ReadLogic readlogic)
        {
            this.modlogic = logic;
            this.readlogic = readlogic;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteHoliday(string id)
        {
            try
            {
                Holiday holiday = readlogic.GetOneHoliday(id);
                modlogic.DeleteHoliday(holiday);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Bad request error: {ex}");
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetHoliday(string id)
        {
            try
            {
                return Ok(readlogic.GetOneHoliday(id));
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Bad request error: {ex}");
            }
            
        }
        [HttpGet]
        public IActionResult GetAllHolidays()
        {
            try
            {
                return Ok(readlogic.GetAllHoliday());
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Bad request error: {ex}");
            }
        }

        [HttpPost]
        public IActionResult AddHoliday([FromBody] Holiday item)
        {
            try
            {
                modlogic.InsertHoliday(item);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpDelete("DeleteBellRingsInRange/{startDate}&{endDate}")]
        public IActionResult DeleteBellRingsInRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                modlogic.DeleteBellRingsInRange(startDate,endDate);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpGet("GetAllCalendarHoliday")]
        public IActionResult GetAllCalenderHoliday()
        {
            try
            {
                return Ok(readlogic.GetAllCalendarHoliday());
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Bad request error: {ex}");
            }
        }

        [HttpDelete("RemoveAllHolidays")]
        public IActionResult RemoveAllHolidays()
        {
            try
            {
                modlogic.RemoveAllHolidays();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Bad request error: {ex}");
            }
        }

        [HttpDelete("RemoveByHoliday/{id}")]
        public IActionResult RemoveByHoliday(string id)
        {
            try
            {
                modlogic.RemoveByHoliday(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Bad request error: {ex}");
            }
        }
        
        [HttpGet("GetBreaksForDay")]
        public IActionResult GetBreaksForDay(DateTime dayDate)
        {
            try
            {
                return Ok(readlogic.GetBreaksForDay(dayDate));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }

        }
    }
}
