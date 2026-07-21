using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels.PlacesManagement
{
    public class PlacesCommentViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PlaceId { get; set; }
        public string Title { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }
    }
}
