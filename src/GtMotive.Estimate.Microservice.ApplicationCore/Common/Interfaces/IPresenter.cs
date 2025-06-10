using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces
{
    /// <summary>
    /// Base interface for presenters.
    /// </summary>
    public interface IPresenter
    {
        IActionResult ActionResult { get; }
    }
}
