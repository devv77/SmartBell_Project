using Logic;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.UI.VM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBell.Controllers
{
    [ApiController]
    [Route("Bellring")]
    public class BellRingController : ControllerBase
    {
        ModificationLogic modlogic;
        ReadLogic readlogic;

        public BellRingController(ModificationLogic logic, ReadLogic readlogic)
        {
            this.modlogic = logic;
            this.readlogic = readlogic;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBellRing(string id)
        {
            try
            {
                BellRing bellring = readlogic.GetOneBellring(id);
                modlogic.DeleteBellring(bellring);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Bad request error: {ex}");
            }
            
        }
        [HttpDelete("DeleteAllBellRings")]
        public IActionResult DeleteAllBellRing()
        {
            try
            {
                IList<BellRing> brings = readlogic.GetAllBellring().Where(x=>x.Type==BellRingType.Start).ToList();
                foreach (var item in brings)
                {
                    modlogic.DeleteBellring(item);
                }
                brings = readlogic.GetAllBellring().Where(x => x.Type == BellRingType.End).ToList();
                foreach (var item in brings)
                {
                    modlogic.DeleteBellring(item);
                }
                brings = readlogic.GetAllBellring().Where(x => x.Type == BellRingType.Special).ToList();
                foreach (var item in brings)
                {
                    modlogic.DeleteBellring(item);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Bad request error: {ex}");
            }

        }
        [HttpGet("{id}")]
        public IActionResult GetBellRing(string id)
        {
            try
            {
                return Ok(readlogic.GetOneBellring(id));
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Bad request error: {ex}");
            }
        }

        [HttpGet]
        public IActionResult GetAllBellRings()
        {
            try
            {
                return Ok(readlogic.GetAllBellring());
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Bad request error: {ex}");
            }
        }

        [HttpPost("InsertLessonBellrings")]
        public IActionResult InsertLessonBellrings([FromBody]LessonViewModel Lesson)
        {
            try
            {
                modlogic.InsertLessonBellrings(Lesson.startBellRing, Lesson.endBellring, Lesson.startFilename, Lesson.endFilename);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost("InsertSequencedBellRings")]
        public IActionResult InsertSequencedBellRings([FromBody] SequencedBellRingViewModel sequencedBellRingViewModel)
        {
            try
            {
                modlogic.InsertSequencedBellRings(sequencedBellRingViewModel.bellRing, sequencedBellRingViewModel.outputPaths);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpPost("UpdateAssignedSequencedBellRing")]
        public IActionResult UpdateSequencedBellRings([FromBody] SequencedBellRingViewModel sequencedBellRingViewModel)
        {
            try
            {
                modlogic.UpdateAssignedSequencedBellRing(sequencedBellRingViewModel.bellRing, sequencedBellRingViewModel.outputPaths);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost("UpdateSequencedBellRing")]
        public IActionResult UpdateSequencedBellRing([FromBody] SequencedBellRingViewModel sequencedBellRingViewModel)
        {
            try
            {
                modlogic.UpdateSequencedBellRing(sequencedBellRingViewModel.bellRing, sequencedBellRingViewModel.outputPaths);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpPost("AssignTimeToSequencedBellRing/{id}&{startTime}")]
        public IActionResult AssignTimeToSequencedBellRing(string id,DateTime startTime)
        {
            try
            {
                BellRing b = readlogic.GetOneBellring(id);
                modlogic.AssignTimeToSequencedBellRing(b, startTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost("InsertSpecialBellring")]
        public IActionResult InsertSpecialBellring([FromBody] SpecialBellRingViewModel specialBellRingViewModel)
        {
            try
            {
                modlogic.InsertSpecialBellring(specialBellRingViewModel.bellRing, specialBellRingViewModel.fileName);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("GetAllSequencedBellRings")]
        public IActionResult GetAllSequencedBellRings()
        {
            try
            {
                return Ok(readlogic.GetAllSequencedBellRings());
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Bad request error: {ex}");
            }
        }

        [HttpGet("GetOneSequencedBellRing/{id}")]
        public IActionResult GetAllSequencedBellRings(string id)
        {
            try
            {
                BellRing b = readlogic.GetSequencedBellring(id);
                return Ok(b);

            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Bad request error: {ex}");
            }
        }

        [HttpPost("SetBellRingIntervalByPath/{id}")]
        public IActionResult SetBellRingIntervalByPath(string id)
        {
            try
            {
                modlogic.SetBellRingIntervalByPath(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"Bad request error: {ex}");
            }
        }

    }
}
