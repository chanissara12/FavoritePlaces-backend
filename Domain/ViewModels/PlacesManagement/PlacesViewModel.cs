using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels.PlacesManagement
{
    public class PlacesViewModel
    {
        public int PlaceId { get; set; }

        public string Title { get; set; }

        public string ImgSrc { get; set; }

        public string ImgAlt { get; set; }

        public string AddBy { get; set; }

        public string IsApproved { get; set; }

        public string IsDeleted { get; set; }

        public int UploadedUserId { get; set; }

        public string UploadedUserName { get; set; }

        public bool IsFav { get; set; }

        public bool HasComment { get; set; }

        public int CommentCount { get; set; }

        public bool AllowDelete { get; set; }
    }
}
