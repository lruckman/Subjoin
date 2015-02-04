using AutoMapper;
using ShortBus;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using Subjoin.Web.Engine.DataAccess.Models;

namespace Subjoin.Web.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                    scan.AddAllTypesOf<Profile>();

                    scan.AssemblyContainingType<IMediator>();
                    scan.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                });

            For<IDependencyResolver>().Use(() => DependencyResolver.Current);

            DependencyResolver.SetResolver(new ShortBus.StructureMap.StructureMapDependencyResolver(ObjectFactory.Container));

            For<SubjoinEntities>()
                .Use(() => new SubjoinEntities());

            For<IMappingEngine>().Use(() => Mapper.Engine);
        }

        #endregion
    }
}