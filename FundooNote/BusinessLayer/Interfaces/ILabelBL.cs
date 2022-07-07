using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ILabelBL
    {
        Task AddLabel(int userId, int noteId, string LabelName);
        Task DeleteLabel(int userId, int noteId);
        Task UpdateLabel(int userId, int noteId, string LabelName);
        //Task<Label> GetLabel(int userId, int noteId);

        Task<List<Label>> GetAllLabel(int userid);
    }
}
