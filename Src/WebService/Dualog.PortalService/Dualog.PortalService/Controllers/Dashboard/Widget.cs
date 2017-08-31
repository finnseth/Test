using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dualog.Data.Oracle.Common.Model;
using Dualog.PortalService.Core.Validation;
using Newtonsoft.Json;

namespace Dualog.PortalService.Controllers.Dashboard
{
    public class Widget : IValidatable
    {
        [Display(Name = "Title", Description = "The title of the widget")]
        [Required( AllowEmptyStrings = true, ErrorMessage = "Title is required." )]
        [StringLength( 100 )]
        [NoWhitespace]
        public string Title { get; set; }

        [Display(Name = "Widget Type", Description = "The type of the widget")]
        [Required( AllowEmptyStrings = true, ErrorMessage = "Widget Type is required." )]
        [StringLength( 100 )]
        [NoWhitespace]
        public string WidgetType { get; set; }

        [Display(Name = "Widget Type", Description = "The type of the widget")]
        [Required( AllowEmptyStrings = true, ErrorMessage = "Widget Name is required." )]
        [StringLength( 100 )]
        [NoWhitespace]
        public string WidgetName { get; set; }

        public long? Id { get; set; }

        [Display(Name = "Height", Description = "The widgets height in pixels")]
        [Range(0, 999999, ErrorMessage = "The number must be between 9 and 999999.")]
        public int? Height { get; set; }

        [Display(Name = "Width", Description = "The widgets width in pixels")]
        [Range(0, 999999, ErrorMessage = "The number must be between 9 and 999999.")]
        public int? Width { get; set; }

        [Display(Name = "Horizontal Rank", Description = "The widgets horizontal rank")]
        [Range(0, 9999, ErrorMessage = "The number must be between 9 and 9999.")]
        public int? HorizontalRank { get; set; }

        [Display(Name = "Vertical Rank", Description = "The widgets vertical rank")]
        [Range(0, 9999, ErrorMessage = "The number must be between 9 and 9999.")]
        public int? VerticalRank { get; set; }


        [JsonIgnore]
        public string TranslationScope { get; set; }

        [JsonIgnore]
        public long DashboardId { get; set; }


        public bool Validate( out string message )
        {
            var validationResults = new List<ValidationResult>();
            if( Validator.TryValidateObject( this, new ValidationContext( this ), validationResults, true ) == false )
            {
                message = validationResults.First().ErrorMessage;
                return false;
            }

            message = null;
            return true;
        }
        public static Widget FromApDashboardWidgetInstance(ApDashboardWidgetInstance w)
        {
            return new Widget
            {

                Id = w.Id,
                Height = w.Height,
                HorizontalRank = w.HorizontalRank,
                Title = w.Title,
                VerticalRank = w.VerticalRank,
                Width = w.Widht,
                WidgetType = w.DashboardWidgetGUI.DashboardWidgetType.Name,
                WidgetName = w.DashboardWidgetGUI.DashboardWidget.Name,
                TranslationScope = w.DashboardWidgetGUI.DashboardWidget.LanguageRef,
                DashboardId = w.DashboardInstance.Id
            };

        }
    }
}
