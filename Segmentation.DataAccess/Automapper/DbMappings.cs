using AutoMapper;
using Segmentation.DataAccess.Models;
using Segmentation.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Segmentation.DataAccess.Automapper
{
    public class DbMappings: Profile
    {
        public DbMappings()
        {
            // Basic mapping
            CreateMap<SegmentDbModel, Segment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.Expression, opt => opt.MapFrom(src => src.Expression))
                .ReverseMap();
        }
    }
}
