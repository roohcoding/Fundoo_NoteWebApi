using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelRL
    {
        Task AddLabel(int userId, int noteId, string LabelName);
        Task DeleteLabel(int userId, int noteId);
        Task UpdateLabel(int userId, int noteId, string LabelName);

        //Task<Label> GetLabel(int userid, int noteId);

        Task<List<Label>> GetAllLabel(int userId);
    }
}
