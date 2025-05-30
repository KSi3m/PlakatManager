﻿using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using System.ComponentModel;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreatePoster
{
    public class CreatePosterCommand: IRequest<Response>
    {
        public LocationDto Location { get; set; }
        public int Priority { get; set; }
        public string? Size { get; set; }

        public decimal? Cost { get; set; }

    
        public int StatusId { get; set; }
        public IEnumerable<int> Tags { get; set; }

        public string? PaperType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


    }
}
