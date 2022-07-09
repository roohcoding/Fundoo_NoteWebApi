using BuisnessLayer.Interface;
using DatabaseLayer.Label;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Services
{
    public class LabelBL : ILabelBL
    {
        ILabelRL labelRL;

        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }
        public async Task AddLabel(int userid, int noteid, string labelName)
        {
            try
            {
                await this.labelRL.AddLabel(userid, noteid, labelName);
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
                await this.labelRL.DeleteLabel(userid, noteid);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<LabelResponseModel>> Get_Label_Join(int userid)
        {
            try
            {
                return await this.labelRL.Get_Label_Join(userid);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Label> GetLabel(int userid, int noteid)
        {
            try
            {
                return await this.labelRL.GetLabel(userid, noteid);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task UpdateLabel(int userid, int noteid, string labelName)
        {
            try
            {
                await this.labelRL.UpdateLabel(userid, noteid, labelName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
