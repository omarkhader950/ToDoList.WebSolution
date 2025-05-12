using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Infrastructure.Mapping;

namespace ToDoList.Infrastructure.Services
{
    public class MappingService : IMappingService
    {
        public TDestination Map<TSource, TDestination>(TSource source)
            => source.Adapt<TDestination>();

        public List<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> source)
            => source.Adapt<List<TDestination>>();
    }
}
