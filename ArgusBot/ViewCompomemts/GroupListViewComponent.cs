using ArgusBot.BL.DTO;
using ArgusBot.BL.Services.Interfaces;
using ArgusBot.DAL.Models;
using ArgusBot.Models.Components;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ArgusBot.ViewCompomemts
{
    public class GroupListViewComponent : ViewComponent
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;
        public GroupListViewComponent(IMapper mapper, IGroupService groupService)
        {
            _mapper = mapper;
            _groupService = groupService;
        }

        public IViewComponentResult Invoke()
        {
            if (HttpContext.Request.Cookies.ContainsKey("telegram_id") && HttpContext.Request.Cookies.ContainsKey("attached_telegram"))
            {
                if (HttpContext.Request.Cookies["attached_telegram"] == "true")
                {
                    var adminId = long.Parse(HttpContext.Request.Cookies["telegram_id"]);

                    IEnumerable<GroupDTO> groups = _groupService.GetGroupsForCurrentAdmin(adminId)?.Select(_mapper.Map<Group, GroupDTO>);

                    if (groups != null)
                    {
                        return View(new GroupListComponentViewModel()
                        {
                            Groups = groups,
                            NeedToAttachTelegramAccount = false
                        });
                    }
                    else
                    {
                        return View(new GroupListComponentViewModel()
                        {
                            Groups = null,
                            NeedToAttachTelegramAccount = false
                        });
                    }
                }
            }
            return View(new GroupListComponentViewModel()
            {
                Groups = null,
                NeedToAttachTelegramAccount = true
            });
        }
    }
}
