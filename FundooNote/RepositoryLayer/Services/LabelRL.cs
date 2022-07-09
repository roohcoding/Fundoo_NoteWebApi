using DatabaseLayer.Label;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

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

        public async Task AddLabel(int userid, int noteid, string labelName)
        {
            try
            {

                var label1 = await fundooContext.Labels.Where(c => c.UserId == userid && c.NoteId == noteid).FirstOrDefaultAsync();
                if (label1 == null)
                {


                    Label label = new Label();

                    label.UserId = userid;
                    label.NoteId = noteid;
                    label.LabelName = labelName;

                    await fundooContext.Labels.AddAsync(label);
                    await fundooContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }





        public async Task DeleteLabel(int userid, int noteid)
        {
            try
            {
                var label = fundooContext.Labels.Where(u => u.UserId == userid && u.NoteId == noteid).FirstOrDefault();
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

        //public async Task<List<Label>> GetAllLabel(int userid)
        //{
        //    try
        //    {
        //        var label = fundooContext.Labels.FirstOrDefault(u => u.UserId == userid);
        //        if (label != null)
        //        {
        //            return null;
        //        }
        //        return await fundooContext.Labels.ToListAsync();
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
        public async Task<List<LabelResponseModel>> Get_Label_Join(int userid)
        {
            try
            {

                var label = fundooContext.Labels.FirstOrDefault(u => u.UserId == userid);
                if (label == null)
                {
                    return null;
                }
                //return await fundooContext.Labels.ToListAsync();


                // Get All Label By Linq Join query

                return await fundooContext.Labels
                    .Where(l => l.UserId == userid)
                    .Join(fundooContext.Notes
                    .Where(n => n.noteID == label.NoteId),
                    l => l.NoteId,
                    n => n.noteID,
                    (l, n) => new LabelResponseModel
                    {
                        UserId = l.UserId,
                        NoteId = n.noteID,
                        Title = n.Title,
                        Description = n.Description,
                        LabelName = l.LabelName,

                    }).ToListAsync();


                //var label = from l in fundooContext.Labels
                //            join n in fundooContext.Notes on l.NoteId equals n.noteID
                //            join u in fundooContext.Users on l.UserId equals u.UserId
                //            select new LabelResponseModel()
                //            {
                //                UserId = l.UserId,
                //                NoteId = n.noteID,
                //                FirstName = u.FirstName,
                //                LastName = u.LastName,
                //                Title = n.Title,
                //                Description = n.Description,
                //                LabelName = l.LabelName,
                //            }.ToListAsync();
                //return label;







            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Label> GetLabel(int userid, int noteId)
        {
            try
            {
                return await fundooContext.Labels.FirstOrDefaultAsync(u => u.UserId == userid && u.NoteId == noteId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task UpdateLabel(int userid, int noteId, string labelName)
        {
            try
            {
                var label = fundooContext.Labels.Where(u => u.UserId == userid && u.NoteId == noteId).FirstOrDefault();
                if (label != null)
                {

                    label.LabelName = labelName;
                    await fundooContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
