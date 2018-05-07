using AutoMapper;
using FWA.Api.Models.ViewModels;
using FWA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FWA.Api
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize((config) =>
            {
                config.CreateMap<Movie, MovieViewModel>()
                      .ForMember(dest => dest.id, vm => vm.MapFrom(i => i.Id))
                      .ForMember(dest => dest.title, vm => vm.MapFrom(i => i.Title))
                      .ForMember(dest => dest.yearOfRelease, vm => vm.MapFrom(i => i.Year))
                      .ForMember(dest => dest.averageRating, vm => vm.MapFrom((i => i.Ratings.Count > 0 ? (float?)(Math.Round(i.Ratings.Average(avg => avg.Value) * 2, MidpointRounding.AwayFromZero) / 2) : null)));
            });
        }
    }
}