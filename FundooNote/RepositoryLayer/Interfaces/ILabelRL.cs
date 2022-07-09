using DatabaseLayer.Label;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelRL
    {
        Task AddLabel(int userid, int noteid, string labelName);
        Task DeleteLabel(int userid, int noteid);
        Task UpdateLabel(int userid, int noteId, string labelName);

        Task<Label> GetLabel(int userid, int noteId);

        Task<List<LabelResponseModel>> Get_Label_Join(int userid);
    }
}
