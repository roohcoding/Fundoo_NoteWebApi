using BusinessLayer.Interfaces;
using DatabaseLayer.User;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class NoteBL : INoteBL
    {
        INoteRL noteRL;
        public NoteBL(INoteRL noteRL)
        {
            this.noteRL = noteRL;

        }
        public async Task AddNote(int UserId, NotePostModel notePostModel)
        {
            try
            {
                await this.noteRL.AddNote(UserId, notePostModel);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task DeleteNote(int UserId, int NoteId)
            {
                try
                {
                    await this.noteRL.DeleteNote(UserId, NoteId);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

        public Task<List<Note>> GetAllNote(int UserId)
        {
            try
            {
                return this.noteRL.GetAllNote(UserId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Note> GetNote(int UserId, int NoteId)
        {
            try
            {
                return await  this.noteRL.GetNote(UserId, NoteId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task UpdateNote(int UserId, NoteUpdatePostModel noteUpdatePostModel)
        {
            try
            {
                await this.noteRL.UpdateNote(UserId, noteUpdatePostModel);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}

