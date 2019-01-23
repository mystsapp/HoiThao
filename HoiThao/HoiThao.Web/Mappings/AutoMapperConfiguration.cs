using AutoMapper;
using HoiThao.Web.Data.Models;
using HoiThao.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoiThao.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void configure()
        {
            Mapper.CreateMap<account, accountViewModel>();
            Mapper.CreateMap<asean, aseanViewModel>();
        }
    }
}