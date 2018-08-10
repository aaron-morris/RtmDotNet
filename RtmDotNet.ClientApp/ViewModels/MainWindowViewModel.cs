using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReactiveUI;
using RtmDotNet.ClientApp.Models;
using RtmDotNet.Lists;
using RtmDotNet.Users;

namespace RtmDotNet.ClientApp.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private IRtmUser _user;

        private IList<RtmList> _lists;

        public MainWindowViewModel()
        {
            _lists = new List<RtmList>();
            Initialize();
        }

        private async Task Initialize()
        {
            var rtmConfigs = JsonConvert.DeserializeObject<RtmConfigs>(File.ReadAllText("rtmConfigs.json"));
            Rtm.Init(rtmConfigs.ApiKey, rtmConfigs.SharedSecret);
            _user = Rtm.GetUserFactory().LoadFromJson(File.ReadAllText("myRtmUser.json"));
            var lists = await Rtm.GetListRepository(_user.Token).GetAllListsAsync();
            Children = lists.ToList().OrderBy(x => x.Position).ThenBy(x => x.Name).ToList();
        }

        public IList<RtmList> Children
        {
            get => _lists;
            set => this.RaiseAndSetIfChanged(ref _lists, value);
        }
    }
}
