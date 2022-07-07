using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Services.Entities;
using Label = RepositoryLayer.Services.Entities.Label;

namespace RepositoryLayer.Services
{
    public class LabelRL : ILabelRL
    {
        FundooContext fundooContext;
        IConfiguration configuration;

        public LabelRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;

        }

        public async Task AddLabel(int userId, int noteId, string LabelName)
        {
            try
            {

                var label1 = await fundooContext.Labels.Where(c => c.UserId == userId && c.NoteId == noteId).FirstOrDefaultAsync();
                if (label1 == null)
                {


                    Label label = new Label();

                    label.UserId = userId;
                    label.NoteId = noteId;
                    label.LabelName = LabelName;

                    await fundooContext.Labels.AddAsync(label);
                    await fundooContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteLabel(int userId, int noteId)
        {
            try
            {
                var label = fundooContext.Labels.Where(u => u.UserId == userId && u.NoteId == noteId).FirstOrDefault();
                if (label != null)
                {
                    fundooContext.Labels.Remove(label);
                    await fundooContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<Label>> GetAllLabel(int userid)
        {
            try
            {
                var label = fundooContext.Labels.FirstOrDefault(u => u.UserId == userid);
                if (label != null)
                {
                    return null;
                }
                return await fundooContext.Labels.ToListAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public async Task<Label> GetLabel(int userid, int noteId)
        //{
        //    try
        //    {
        //        return await fundooContext.Labels.FirstOrDefaultAsync(u => u.UserId == userid && u.NoteId == noteId);
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public async Task UpdateLabel(int userId, int noteId, string LabelName)
        {
            try
            {
                var label = fundooContext.Labels.Where(u => u.UserId == userId && u.NoteId == noteId).FirstOrDefault();
                if (label != null)
                {

                    label.LabelName = LabelName;
                    await fundooContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        Task<List<System.Reflection.Emit.Label>> ILabelRL.GetAllLabel(int userId)
        {
            throw new NotImplementedException();
        }

        Task<System.Reflection.Emit.Label> ILabelRL.GetLabel(int userid, int noteId)
        {
            throw new NotImplementedException();
        }
    }
}
