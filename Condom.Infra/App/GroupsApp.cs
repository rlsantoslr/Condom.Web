using Condom.Domain.Global;
using Condom.Domain.Models;
using Condom.Infra.App.Base;
using Condom.Infra.Repositories;
using Condom.Infra.Repositories.Identity;
using Condom.Infra.Validations;
using Condom.Infra.Validations.Base;
using Condom.Views.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Infra.App
{
    public class GroupsApp : BaseApp<GroupsView, Groups>
    {
        readonly GroupsStore _GroupStore;


        public GroupsApp(GroupValidator validator, GroupsStore groupStore) : base(validator)
        {
            _GroupStore = groupStore;
        }

        protected override async Task<GroupsView> OnAfterValidate(GroupsView view)
        {
            bool commit = false;
            switch (view.GetCrud())
            {
                case Domain.Global.CondEnum.CrudEnum.Create:
                    await _GroupStore.CreateAsync(view.Domain);
                    commit = true;
                    break;

                case Domain.Global.CondEnum.CrudEnum.Read:

                    break;
            }


            if (commit)
            {
                view.GetTracker().Merge(await _GroupStore.SaveChangeAsync());
            }

            return view;
        }

        public async Task<List<GroupsView>> GetGroups(Guid userId)
        {
            var rt = new List<GroupsView>();
            var data = await _GroupStore.OwnDbSet.Where(x => x.OwnerId == userId).ToListAsync();
            if (data == null) return rt;

            foreach (var group in data)
            {
                rt.Add(new GroupsView(group));
            }

            return rt;
        }
    }
}
