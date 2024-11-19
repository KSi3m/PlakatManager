using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using System;
using System.Text.Json;

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

        public ElectionItem CreateElectionItem(string type, ElectionItemRequestDTO dto)
        {
            if(_factories.TryGetValue(type, out IElectionItemFactory value)) {
                return value.Create(dto);
            }

            throw new Exception("Type not found");
        }
    }
}
