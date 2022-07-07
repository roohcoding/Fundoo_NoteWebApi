using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace FundooNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        ILabelBL labelBL;
        FundooContext fundooContext;
        public LabelController(ILabelBL labelBL, FundooContext fundooContext)
        {
            this.labelBL = labelBL;
            this.fundooContext = fundooContext;
        }
        [Authorize]
        [HttpPost("AddLabel/{noteId}/{labelName}")]
        public async Task<ActionResult> AddLabel(int noteId, string labelName)
        {
            try
            {
                var currentUser = HttpContext.User;
                int UserId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == UserId && u.noteID == noteId);

                if (note == null)
                {
                    return this.BadRequest(new { success = false, Message = "Note doesn't Exists" });
                }
                await this.labelBL.AddLabel(UserId, noteId, labelName);
                return this.Ok(new { success = true, Message = "Label is added" });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpDelete("DeleteLabel/{noteId}")]

        public async Task<ActionResult> DeleteLabel(int noteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int UserId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var label = fundooContext.Labels.FirstOrDefault(u => u.UserId == UserId && u.NoteId == noteId);
                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == UserId && u.noteID == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, Message = "Note Doesn't Exits" });
                }

                if (label == null)
                {
                    return this.BadRequest(new { success = false, Message = "Label Doesn't Exists" });
                }
                await this.labelBL.DeleteLabel(UserId, noteId);
                return this.Ok(new { success = true, Message = "Label Deleted Successfully" });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpPut("UpdateLabel/{noteId}")]

        public async Task<ActionResult> UpdateLabel(int noteId, string LabelName)
        {
            try
            {
                var currentUser = HttpContext.User;
                int UserId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                var note = fundooContext.Notes.Where(u => u.UserId == UserId && u.noteID == noteId).FirstOrDefault();
                if (note == null)
                {
                    return this.BadRequest(new { success = false, Message = "Note Doesn't Exists" });
                }
                var label = fundooContext.Labels.Where(u => u.UserId == UserId && u.NoteId == noteId).FirstOrDefault();
                if (label == null)
                {
                    return this.BadRequest(new { success = true, Message = "Label Doesn't Exists" });
                }
                await this.labelBL.UpdateLabel(UserId, noteId, LabelName);
                return this.Ok(new { success = true, Message = "Label Updated successfully" });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //[Authorize]
        //[HttpGet("GetLabel/{noteId}")]
        //public async Task<ActionResult> GetLabel(int noteId)
        //{
        //    try
        //    {
        //        var currentUser = HttpContext.User;
        //        int UserId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        //        var note = fundooContext.Notes.FirstOrDefault(x => x.UserId == UserId && x.noteID == noteId);
        //        if (note == null)
        //        {
        //            return this.BadRequest(new { success = false, Message = "Note Doesn't Exists" });
        //        }
        //        var label = fundooContext.Labels.FirstOrDefault(x => x.UserId == UserId && x.NoteId == noteId);
        //        if (label == null)
        //        {
        //            return this.BadRequest(new { success = false, Message = "Label Doesn't Exists" });
        //        }
        //        var label1 = await this.labelBL.GetLabel(UserId, noteId);
        //        return this.Ok(new { success = true, Message = $"Label Obtained Successfully for {label.LabelName}", data = label1 });
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
        [Authorize]
        [HttpGet("GetAllLabel")]

        public async Task<ActionResult> GetAllLabel()
        {
            try
            {
                var currentUser = HttpContext.User;

                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var labels = await labelBL.GetAllLabel(userId);
                if (labels != null)
                {
                    return this.Ok(new { status = 200, Success = true, Message = "lables are ready", data = labels });
                }
                else
                {
                    return this.NotFound(new { iSuccess = false, Message = "No label found" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = 401, isSuccess = false, message = e.InnerException.Message });
            }
        }

    }
}
