using System;

namespace H24.Models
{
    public interface IProject
    {
        string ThumbnailUri { get; }

        string Title { get; }

        DateTime Date { get; }

        Tag[] Tags { get; } 
    }
}