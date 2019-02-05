using System;

namespace H24.Models
{
    public interface IProjectWithUri : IProject
    {
        string ProjectUri { get; }
    }
}