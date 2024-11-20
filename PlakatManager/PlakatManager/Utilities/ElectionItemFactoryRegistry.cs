using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using System;
using System.Text.Json;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateElectionItem;

namespace ElectionMaterialManager.Utilities
{
    public class ElectionItemFactoryRegistry
    {
        private readonly Dictionary<string, IElectionItemFactory> _factories;

        public ElectionItemFactoryRegistry(IServiceProvider serviceProvider)
        {
            _factories = new Dictionary<string, IElectionItemFactory>
            {   {"Billboard", serviceProvider.GetRequiredService<BillboardFactory>()},
                {"Poster", serviceProvider.GetRequiredService<PosterFactory>() },
                {"LED", serviceProvider.GetRequiredService<LEDFactory>()  }
            };
            
        }

        public ElectionItem CreateElectionItem(string type, CreateElectionItemCommand command)
        {
            if(_factories.TryGetValue(type, out IElectionItemFactory value)) {
                return value.Create(command);
            }

            throw new Exception("Type not found");
        }
    }
}
