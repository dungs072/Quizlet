using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizletAchivement.DBContexts;
using QuizletAchivement.Models;
using QuizletAchivement.Respository;
using QuizletAchivement.ViewModels;
using System;

namespace QuizletAchivement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchivementController : ControllerBase
    {
        private readonly IAchieveRespository achieveRespository;
        public AchivementController(IAchieveRespository achieveRespository)
        {
            this.achieveRespository = achieveRespository;
        }
        #region Achivement

        [HttpGet]
        public async Task<ActionResult<IEnumerable<THANHTUU>>> GetTHANHTUU()
        {
            return await achieveRespository.GetTHANHTUU();
        }
        [HttpGet("{AchivementId}")]
        public async Task<ActionResult<THANHTUU>> GetTHANHTUUById(int AchivementId)
        {
            return await achieveRespository.GetTHANHTUUById(AchivementId);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CreateTHANHTUU(THANHTUU thanhtuu)
        {
            var result = await achieveRespository.CreateTHANHTUU(thanhtuu);
            if(result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateTHANHTUU(THANHTUU thanhtuu)
        {
            var result = await achieveRespository.UpdateTHANHTUU(thanhtuu);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpDelete("{AchivementId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTHANHTUU(int AchivementId)
        {
            var result = await achieveRespository.DeleteTHANHTUU(AchivementId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        #endregion

        #region UserAchieve
        [HttpGet("AchieveStatistics/{userId}")]
        public async Task<AchieveStatistics> GetAchieveStatistics(int userId)
        {
            return await achieveRespository.GetAchieveStatistics(userId);
        }

        [HttpGet("GetSequenceCalender/{userId}")]
        public async Task<List<string>> GetSequenceCalender(int userId)
        {
            return await achieveRespository.GetSequenceCalender(userId);
        }
        [HttpPost("MarkAttendance")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> MarkAttendance(MarkAttendance mark)
        {
            var result = await achieveRespository.MarkAttendance(mark);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet("GetBadges/{userId}")]
        public async Task<List<Badge>> GetBadges(int userId)
        {
            return await achieveRespository.GetBadges(userId);
        }
        [HttpGet("UpdateBadge/{userId}/{typeBadge}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<AchivementBadge>> GetUpdateBadge(int userId, string typeBadge)
        {
            var result = await achieveRespository.GetUpdateBadge(userId, typeBadge);
            if(result==null)
            {
                return NotFound();
            }
            return result;
        }
        [HttpPost("UpdateBadge")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddUpdateBadge(AchieveBadge achieveBadge)
        {
            var result = await achieveRespository.AddUpdateBadge(achieveBadge);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        #endregion



    }
}
