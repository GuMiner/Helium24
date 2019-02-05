using System;
using H24.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages.LaserCut
{
    public class ChessBoardModel : PageModel, IProject
    {
        public string ThumbnailUri => "/Blobs/ProjectThumbnails/LaserCut/ChessBoard.png";

        public string Title => "Chess Board";

        public DateTime Date => new DateTime(2015, 4, 1);

        public Tag[] Tags => new[] { Tag.Hardware, Tag.LaserMill, Tag.Games };

        public void OnGet()
        {
        }
    }
}
