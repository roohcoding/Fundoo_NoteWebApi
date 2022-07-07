using DatabaseLayer.Note;
using DatabaseLayer.User;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface INoteBL
    {
        Task AddNote( int UserId, NotePostModel notePostModel);
        Task<List<Note>> GetAllNote(int UserId);
        Task DeleteNote(int UserId, int noteId);
        Task UpdateNote(int UserId, int noteId, NoteUpdatePostModel noteUpdatePostModel);
        Task<Note> GetNote(int UserId, int NoteId);
        Task RemainderNote(int UserId, int noteId, ReminderModel reminderModel  );
        Task PinNote(int UserId, int noteId);
        Task ArchiveNote(int UserId, int noteId);
        Task Trash(int UserId, int noteId);

    }
}
