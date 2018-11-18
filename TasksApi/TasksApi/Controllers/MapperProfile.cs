using AutoMapper;
using Tasks.DataLayer.Models;
using TasksApi.Dto;

namespace TasksApi.Controllers
{
    /// <summary>
    /// Mapper dto models into database models and vice versa.
    /// </summary>
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TaskModel, TaskDetails>();
            CreateMap<TaskDetails, TaskModel>();
        }
    }
}
