using Autofac;
using CodefictionApi.Core.Contracts;
using CodefictionApi.Core.Repositories;
using CodefictionApi.Core.Services;
using CodefictionApi.Core.Services.Mappers;

namespace CodefictionApi.Core
{
    public class ApiCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<MeetupRepository>().As<IMeetupRepository>().InstancePerDependency();
            builder.RegisterType<DatabaseProvider>().As<IDatabaseProvider>().InstancePerDependency();
            builder.RegisterType<PersonRepository>().As<IPersonRepository>().InstancePerDependency();
            builder.RegisterType<PodcastRepository>().As<IPodcastRepository>().InstancePerDependency();
            builder.RegisterType<SponsorRepository>().As<ISponsorRepository>().InstancePerDependency();
            builder.RegisterType<VideoRepository>().As<IVideoRepository>().InstancePerDependency();

            builder.RegisterType<PodcastModelMapper>().As<IPodcastModelMapper>().InstancePerDependency();
            builder.RegisterType<MeetupModelMapper>().As<IMeetupModelMapper>().InstancePerDependency();
            builder.RegisterType<VideoModelMapper>().As<IVideoModelMapper>().InstancePerDependency();

            builder.RegisterType<MeetupService>().As<IMeetupService>().InstancePerDependency();
            builder.RegisterType<PersonService>().As<IPersonService>().InstancePerDependency();
            builder.RegisterType<PodcastService>().As<IPodcastService>().InstancePerDependency();
            builder.RegisterType<PersonService>().As<IPersonService>().InstancePerDependency();
            builder.RegisterType<SponsorService>().As<ISponsorService>().InstancePerDependency();
            builder.RegisterType<VideoService>().As<IVideoService>().InstancePerDependency();
        }
    }
}
