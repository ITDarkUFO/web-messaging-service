using AutoMapper;
using SharedLibrary.DTOs;
using SharedLibrary.Models;

namespace WebServer.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ChatMessage, ChatMessageDTO>();
        }
    }
}
