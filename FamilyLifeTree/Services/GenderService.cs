namespace FamilyLifeTree.Services
{
    using FamilyLifeTree.Core.Interfaces;
    using FamilyLifeTree.Core.Models;
    using FamilyLifeTree.ViewModels.Entities;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GenderService : IGenderService<GenderModel, GenderViewModel>
    {
        public IEnumerable<GenderViewModel> ViewModels { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsInitialized => throw new NotImplementedException();

        public event EventHandler Initialized;

        public GenderViewModel CreateVM(GenderModel model)
        {
            throw new NotImplementedException();
        }

        public Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
