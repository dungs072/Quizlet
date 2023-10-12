using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizletClass.DBContexts;
using QuizletClass.Models;
using QuizletClass.ViewModels;

namespace QuizletClass.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly ClassDBContext dBContext;
        public ClassController(ClassDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ClassViewModel>> GetLOP()
        {
            List<ClassViewModel> classes = new List<ClassViewModel>();
            foreach(var lop in dBContext.lops)
            {
                ClassViewModel model = new ClassViewModel();
                model.Copy(lop);
                model.NumberParticipants = lop.chitietdangkilop.Count;
                model.NumberLearningModules = lop.chitiethocphan.Count;
                classes.Add(model);
            }
            return classes;
        }
        [HttpGet("{userId}")]
        public IEnumerable<LOP> GetLOPByUserId(int userId)
        {
            var lops = dBContext.lops.Where(e => e.UserId == userId).ToList();
            return lops;
        }
        [HttpGet("find/{ClassId}")]
        public async Task<ActionResult<LOP>> GetLOPById(int ClassId)
        {
            var LOP = await dBContext.lops.FindAsync(ClassId);
            return LOP;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateLOP(LOP lop)
        {
            if(HasDuplicateClassName(lop.UserId,lop.ClassName))
            {
                return BadRequest();
            }
            lop.CreatedDate = DateTime.Now;
            lop.NGUOIDUNG = await dBContext.nguoidungs.FindAsync(lop.UserId);
            await dBContext.lops.AddAsync(lop);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        //[HttpGet("findd/{userId}")]
        //public async Task<NGUOIDUNG> GetNguoiDung(int userId)
        //{
        //    var nguoidung= await dBContext.nguoidungs.FindAsync(userId);
        //    return nguoidung;
        //}
        private bool HasDuplicateClassName(int userId, string className)
        {
            var lop = dBContext.lops.FirstOrDefault(u => (u.UserId == userId && u.ClassName == className));
            return lop != null;
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateLOP(LOP lop)
        {
            if (HasDuplicateClassNameForUpdate(lop.UserId,lop.ClassId, lop.ClassName))
            {
                return BadRequest();
            }
            dBContext.lops.Update(lop);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        private bool HasDuplicateClassNameForUpdate(int userId, int classId, string className)
        {
            var cl = dBContext.lops.FirstOrDefault(u => (u.UserId == userId && u.ClassName == className && u.ClassId != classId));
            return cl != null;
        }
        [HttpDelete("{ClassId}")]
        public async Task<ActionResult> DeleteLOP(int ClassId)
        {
            var lop = await dBContext.lops.FindAsync(ClassId);
            dBContext.lops.Remove(lop);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
    }
}
