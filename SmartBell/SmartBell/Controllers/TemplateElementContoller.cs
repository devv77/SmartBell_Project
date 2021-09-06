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
    [Route("TemplateElement")]
    public class TemplateElementContoller:ControllerBase
    {
        ModificationLogic modlogic;
        ReadLogic readlogic;

        public TemplateElementContoller(ModificationLogic logic, ReadLogic readlogic)
        {
            this.modlogic = logic;
            this.readlogic = readlogic;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTemplateElement(string id)
        {
            try
            {
                TemplateElement templateElement = readlogic.GetOneTemplateElement(id);
                modlogic.DeleteTemplateElement(templateElement);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Bad request error: {ex}");
            }
        }

        [HttpGet]
        public IActionResult GetAllTemplate()
        {
            try
            {  
                return Ok(readlogic.GetAllTemplateElement());
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Bad request error: {ex}");
            }
        }
        [HttpGet("GetTemplateElementsForTemplate/{id}")]
        public IActionResult GetTemplateElementsForTemplate(string id)
        {
            try
            {
                return Ok(readlogic.GetElementsForTemplate(id));
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Bad request error: {ex}");
            }
        }
    }
}
