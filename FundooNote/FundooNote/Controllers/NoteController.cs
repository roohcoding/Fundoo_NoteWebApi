using BusinessLayer.Interfaces;
using DatabaseLayer.Note;
using DatabaseLayer.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RepositoryLayer.Services;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FundooNote.Controllers
{


    [Route("api/[controller]")]
    [ApiController]

    public class NoteController : ControllerBase
    {
        INoteBL noteBL;
        FundooContext fundooContext;
        private readonly IDistributedCache distributedCache;
        private string cacheKey;
        IConfiguration configuraton;
        public NoteController(INoteBL noteBL, FundooContext fundooContext, IDistributedCache distributedCache, IConfiguration configuration)
        {
            this.noteBL = noteBL;
            this.fundooContext = fundooContext;
            this.distributedCache = distributedCache;
            this.configuraton = configuration;
            cacheKey = configuraton.GetSection("redis").GetSection("CacheKey").Value;
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddNote(NotePostModel notePostModel)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                await this.noteBL.AddNote(userId, notePostModel);
                return this.Ok(new { success = true, message = "Note Added Sucessfully" });


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpGet]

        public async Task<ActionResult> GetAllNote()
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId);
                if (note == null)
                {

                    return this.BadRequest(new { success = true, message = "Note Doesn't Exits" });

                }
                List<Note> noteList = new List<Note>();

                noteList = await this.noteBL.GetAllNote(userId);

                return Ok(new { success = true, message = "GetAllNote Successfully", data = noteList });

            }
            catch (Exception e)
            {
                throw e;
            }

        }
        [Authorize]
        [HttpDelete("{NoteId}")]

        public async Task<ActionResult> DeleteNote(int NoteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId && u.noteID == NoteId);
                if (note == null)
                {

                    return this.BadRequest(new { success = true, message = "Sorry! Note Doesn't Exist Please Create a Notes" });

                }
                await this.noteBL.DeleteNote(userId, NoteId);

                return Ok(new { success = true, message = $"Note Deleted Successfully for the note, {note.Title} " });

            }
            catch (Exception e)
            {
                throw e;
            }
            //$"cartList fetched Fail {e.Message}"
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateNote(int noteId, NoteUpdatePostModel noteUpdatePostModel)
        {

            try
            {
                var currentUser = HttpContext.User;
                int UserId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                var noteCheck = fundooContext.Notes.FirstOrDefault(u => u.UserId == UserId && u.noteID == noteId);
                if (noteCheck == null)
                {
                    return this.BadRequest(new { success = true, message = "Note Doesn't Exists" });
                }

                await this.noteBL.UpdateNote(UserId, noteId, noteUpdatePostModel);

                return Ok(new { success = true, message = "Update Note Successfully" });
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpGet("{NoteId}")]

        public async Task<ActionResult> GetNote(int NoteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId && u.noteID == NoteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = true, message = "Note Doesn't Exists" });
                }

                var noteElement = await this.noteBL.GetNote(userId, NoteId);

                return Ok(new { success = true, message = "Get Note Successfully", data = noteElement });
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        [Authorize]
        [HttpPut("ReminderNote/{NoteId}")]

        public async Task<ActionResult> ReminderNote(int NoteId, ReminderModel noteReminderModel)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId && u.noteID == NoteId);
                if (note == null)
                {

                    return this.BadRequest(new { success = false, message = "Sorry! Note Doesn't Exist Please Create a Notes" });

                }
                await this.noteBL.ReminderNote(userId, NoteId, Convert.ToDateTime(noteReminderModel.Reminder));

                return Ok(new { success = true, message = $"Note Reminder Successfully for the note, {note.Title} " });

            }
            catch (Exception e)
            {
                throw e;
            }
        }



        [Authorize]
        [HttpPut("ArchiveNote/{NoteId}")]

        public async Task<ActionResult> ArchiveNote(int NoteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId && u.noteID == NoteId);
                if (note == null)
                {

                    return this.BadRequest(new { success = true, message = "Sorry! Note Doesn't Exist Please Create a Notes" });

                }
                await this.noteBL.ArchiveNote(userId, NoteId);

                return Ok(new { success = true, message = $"Note Archive Successfully for the note, {note.Title} " });

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpPut("PinNote/{NoteId}")]

        public async Task<ActionResult> PinNote(int NoteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId && u.noteID == NoteId);
                if (note == null)
                {

                    return this.BadRequest(new { success = true, message = "Sorry! Note Doesn't Exist Please Create a Notes" });

                }
                await this.noteBL.PinNote(userId, NoteId);

                return Ok(new { success = true, message = $"Note Pinned Successfully for the note, {note.Title} " });

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        [Authorize]
        [HttpPut("Trash/{NoteId}")]
        public async Task<ActionResult> Trash(int NoteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId && u.noteID == NoteId);
                if (note == null)
                {

                    return this.BadRequest(new { success = true, message = "Sorry! Note Doesn't Exist Please Create a Notes" });

                }
                await this.noteBL.Trash(userId, NoteId);

                return Ok(new { success = true, message = $"Note sent to Trash Successfully, {note.Title} " });

            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [Authorize]
        [HttpGet("AllNoteByRedis")]

        public async Task<ActionResult> GetAllNoteByRedis()
        {
            try
            {

                var CacheKey = "NoteList";

                string SerializeNoteList;
                var notelist = new List<Note>();
                var redisnotelist = await distributedCache.GetAsync(CacheKey);
                if (redisnotelist != null)
                {
                    SerializeNoteList = Encoding.UTF8.GetString(redisnotelist);
                    notelist = JsonConvert.DeserializeObject<List<Note>>(SerializeNoteList);
                }

                else
                {
                    var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                    int userId = Int32.Parse(userid.Value);
                    notelist = await this.noteBL.GetAllNote(userId);
                    SerializeNoteList = JsonConvert.SerializeObject(notelist);
                    redisnotelist = Encoding.UTF8.GetBytes(SerializeNoteList);
                    var option = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20)).SetAbsoluteExpiration(TimeSpan.FromHours(6));
                    await distributedCache.SetAsync(CacheKey, redisnotelist, option);
                }
                return this.Ok(new { success = true, message = $"Get Note Successful", data = notelist });


            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
