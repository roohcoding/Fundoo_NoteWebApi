using DatabaseLayer.Note;
using DatabaseLayer.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class NoteRL : INoteRL
    {
        FundooContext fundooContext;

        IConfiguration configuration;

        private readonly string _secret;


        public NoteRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;

        }
        public async Task AddNote(int UserId, NotePostModel notePostModel)
        {
            try
            {
                Note note = new Note();
                note.UserId = UserId;
                note.Title = notePostModel.Title;
                note.Description = notePostModel.Description;
                note.Colour = notePostModel.Colour;
                note.CreatedDate = DateTime.Now;
                note.ModifiedDate = DateTime.Now;
                fundooContext.Add(note);

                await fundooContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<Note>> GetAllNote(int UserId)
        {
             try
            {
                var note = fundooContext.Notes.Where(u => u.UserId == UserId).FirstOrDefault();

                if (note == null)
                {
                    return null; 
                }
                return await fundooContext.Notes.ToListAsync();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteNote(int UserId, int noteId)
        {
            try
            {

                var note = fundooContext.Notes.Where(x => x.UserId == UserId && x.noteID == noteId).FirstOrDefault();
                if (note != null)
                {
                    fundooContext.Notes.Remove(note);

                    await fundooContext.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

       



     
         async Task<Note> INoteRL.GetNote(int UserId, int NoteId)
        {

            try
            {
                var note = fundooContext.Notes.Where(u => u.UserId == UserId && u.noteID == NoteId).FirstOrDefault();

                if (note == null)
                {
                    return null;
                }
                return await fundooContext.Notes.FirstOrDefaultAsync(u => u.noteID == NoteId);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task UpdateNote(int UserId, int noteId, NoteUpdatePostModel noteUpdatePostModel)
        {
            try
            {
                var note = await fundooContext.Notes.FirstOrDefaultAsync(u => u.UserId == UserId && u.noteID == noteId);

                if (note != null)
                {
                    note.Title = noteUpdatePostModel.Title;
                    note.Description = noteUpdatePostModel.Description;
                    note.Colour = noteUpdatePostModel.Colour;

                    note.IsArchive = noteUpdatePostModel.IsArchive;
                    note.IsPin = noteUpdatePostModel.IsPin;
                    note.IsReminder = noteUpdatePostModel.IsReminder;
                    note.IsTrash = noteUpdatePostModel.IsTrash;
                    await fundooContext.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task ReminderNote(int UserId, int noteId, DateTime dateTime)
        {
            try
            {
                var note = fundooContext.Notes.Where(x => x.UserId == UserId && x.noteID == noteId).FirstOrDefault();
                if (note != null)
                {
                    if (note.IsTrash == false)
                    {
                        if (note.IsReminder == false)
                        {
                            note.IsReminder = true;
                            note.Reminder = dateTime;
                        }
                        else
                        {
                            note.IsReminder = false;
                        }
                    }
                }
                await fundooContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task PinNote(int UserId, int noteId)
        {
            try
            {
                var note = fundooContext.Notes.Where(x => x.UserId == UserId && x.noteID == noteId).FirstOrDefault();
                if (note != null)
                {
                    if (note.IsTrash == false)
                    {
                        if (note.IsPin == false)
                        {
                            note.IsPin = true;
                        }
                        else
                        {
                            note.IsPin = false;
                        }
                    }
                }
                await fundooContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task ArchiveNote(int UserId, int noteId)
        {
            try
            {
                var note = fundooContext.Notes.Where(x => x.UserId == UserId && x.noteID == noteId).FirstOrDefault();
                if (note != null)
                {
                    if (note.IsTrash == false)
                    {
                        if (note.IsArchive == false)
                        {
                            note.IsArchive = true;
                        }
                        else
                        {
                            note.IsArchive = false;
                        }
                    }
                }
                await fundooContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async  Task Trash(int UserId, int noteId)
        {
            try
            {
                var note = fundooContext.Notes.Where(x => x.UserId == UserId && x.noteID == noteId).FirstOrDefault();
                if (note != null)
                {
                    
                        if (note.IsTrash == false)
                        {
                            note.IsTrash = true;
                        }
                        else
                        {
                            note.IsTrash = false;
                        }
                    
                }
                await fundooContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    } 
}
