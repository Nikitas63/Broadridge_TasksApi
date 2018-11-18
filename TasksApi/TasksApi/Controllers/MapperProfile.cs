using AutoMapper;
using Tasks.DataLayer.Models;
using TasksApi.Dto;

namespace TasksApi.Controllers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TaskModel, TaskDetails>();
            CreateMap<TaskDetails, TaskModel>();
        }
    }
}
