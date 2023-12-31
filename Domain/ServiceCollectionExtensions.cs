﻿using System;
using Eveneum;
using System.Linq;
using CrossCutting;
using Domain.Entities;
using System.Threading.Tasks;
using Domain.Events;
using Domain.Repositories;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Domain.Services.CreateNewBbq;
using Domain.Services.RunModerateBbq;
using Domain.Services.GetProposedBbqs;
using Domain.Services.GetInvite;
using Domain.Services.ShoppingListBbq;
using Domain.Services.DeclineInvite;

namespace Domain
{
    public static partial class ServiceCollectionExtensions
    {
        private const string DATABASE = "Churras";
        public static IServiceCollection AddDomainDependencies(this IServiceCollection services)
            => services.AddSingleton(new Person { Id = "e5c7c990-7d75-4445-b5a2-700df354a6a0" })
                .AddEventStoreDependencies()
                .AddDomainServiceDependencies()
                .AddRepositoriesDependencies();

        public static IServiceCollection AddEventStoreDependencies(this IServiceCollection services)
        {
            var client = new CosmosClient(Environment.GetEnvironmentVariable(nameof(EventStore)));

            var bbqStore = new EventStore<Bbq>(client, DATABASE, "Bbqs");
            bbqStore.Initialize().GetAwaiter().GetResult();

            var peopleStore = new EventStore<Person>(client, DATABASE, "People");
            peopleStore.Initialize().GetAwaiter().GetResult();

            var snapshots = new SnapshotStore(client.GetDatabase(DATABASE));

            client.GetDatabase(DATABASE)
                .GetContainer("Lookups")
                .UpsertItemAsync(new Lookups { PeopleIds = Data.People.Where(p => p.IsCoOwner is false).Select(o => o.Id).ToList(), ModeratorIds = Data.People.Where(p => p.IsCoOwner).Select(o => o.Id).ToList() })
                .GetAwaiter()
                .GetResult();

            try
            {
                foreach (var person in Data.People)
                {
                    peopleStore.WriteToStream(person.Id, new[] { new EventData(person.Id, new PersonHasBeenCreated(person.Id, person.Name, person.IsCoOwner), null, 0, DateTime.Now.ToString()) })
                        .GetAwaiter()
                        .GetResult();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("skipping already included data.");
            }

            services.AddSingleton(snapshots);

            services.AddSingleton<IEventStore<Bbq>>(bbqStore);
            services.AddSingleton<IEventStore<Person>>(peopleStore);

            return services;
        }

        public static IServiceCollection AddDomainServiceDependencies(this IServiceCollection services)
        {
            services.AddSingleton<ICreateNewBbqService,CreateNewBbqService>();
            services.AddSingleton<IModerateBbqService, ModerateBbqService>();
            services.AddSingleton<IGetProposedBbqsService, GetProposedBbqsService>();
            services.AddSingleton<IGetInviteService, GetInviteService>();
            services.AddSingleton<IGetShoppingListBbqService, GetShoppingListBbqService>();
            services.AddSingleton<IAcceptInviteService, AcceptInviteService>();
            services.AddSingleton<IDeclineInviteService, DeclineInviteService>();

            return services;
        }

        public static IServiceCollection AddRepositoriesDependencies(this IServiceCollection services)
            => services.AddTransient<IBbqRepository, BbqRepository>()
            .AddTransient<IPersonRepository, PersonRepository>();

        private async static Task CreateIfNotExists(this CosmosClient client, string database, string collection)
        {
            var databaseResponse = await client.CreateDatabaseIfNotExistsAsync(database);
            await databaseResponse.Database.CreateContainerIfNotExistsAsync(new ContainerProperties(collection, "/StreamId"));
        }
    }

    public static class Data
    {
        public static List<Person> People => new List<Person>
        {
            new Person { Id = "e5c7c990-7d75-4445-b5a2-700df354a6a0", Name = "João da Silva", IsCoOwner = false },
            new Person { Id = "171f9858-ddb1-4adf-886b-2ea36e0f0644", Name = "Marcos Oliveira", IsCoOwner = true },
            new Person { Id = "3f74e6bd-11b2-4d48-a294-239a7a2ce7d5", Name = "Gustavo Sanfoninha", IsCoOwner = true },
            new Person { Id = "795fc8f2-1473-4f19-b33e-ade1a42ed123", Name = "Alexandre Morales", IsCoOwner = false },
            new Person { Id = "addd0967-6e16-4328-bab1-eec63bf31968", Name = "Leandro Espera", IsCoOwner = false },
            new Person { Id = "a7c0f9d8-3d4e-4c6d-9f8a-9e3b8cf201d", Name = "Ronaldo Nazario", IsCoOwner = false },
            new Person { Id = "65b6a7e7-8a1b-47f1-925c-8b303829decc", Name = "Rogerio Dutra", IsCoOwner = false },
            new Person { Id = "f345012a-7dd0-4fe3-af4e-67bb32f64fa2", Name = "Adriano Dias", IsCoOwner = false }
        };
    }
}
