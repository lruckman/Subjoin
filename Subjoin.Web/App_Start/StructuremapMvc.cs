using System.Web.Mvc;
using AutoMapper;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Subjoin.Web.App_Start;
using Subjoin.Web.DependencyResolution;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof (StructuremapMvc), "Start")]
[assembly: ApplicationShutdownMethod(typeof (StructuremapMvc), "End")]

namespace Subjoin.Web.App_Start
{
    public static class StructuremapMvc
    {
        #region Public Properties

        public static StructureMapDependencyScope StructureMapDependencyScope { get; set; }

        #endregion

        #region Public Methods and Operators

        public static void End()
        {
            StructureMapDependencyScope.Dispose();
        }

        public static void Start()
        {
            var container = IoC.Initialize();
            StructureMapDependencyScope = new StructureMapDependencyScope(container);
            DependencyResolver.SetResolver(StructureMapDependencyScope);
            DynamicModuleUtility.RegisterModule(typeof (StructureMapScopeModule));

            Mapper.Initialize(cfg =>
            {
                foreach (var profile in DependencyResolver.Current.GetServices<Profile>())
                    cfg.AddProfile(profile);
            });
        }

        #endregion
    }
}