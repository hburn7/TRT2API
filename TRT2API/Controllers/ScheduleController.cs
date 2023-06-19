using Microsoft.AspNetCore.Mvc;
using TRT2API.Data.Models;
using TRT2API.Data.Repositories.Interfaces;

namespace TRT2API.Controllers;

[Route("api/schedules")]
public class ScheduleController : ControllerBase
{
	private readonly IDataWorker _dataWorker;
	private readonly ILogger<ScheduleController> _logger;

	public ScheduleController(IDataWorker dataWorker, ILogger<ScheduleController> logger)
	{
		_dataWorker = dataWorker ?? throw new ArgumentNullException(nameof(dataWorker));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	[HttpGet("all")]
	public async Task<ActionResult<List<Schedule>>> All()
	{
		var schedules = await _dataWorker.Schedules.GetAllAsync();
		if (!schedules.Any())
		{
			return NotFound("No schedules exist.");
		}

		return schedules;
	}

	[HttpGet("{scheduleID:int}")]
	public async Task<ActionResult<Schedule>> Get(int scheduleID)
	{
		var schedule = await _dataWorker.Schedules.GetByIdAsync(scheduleID);
		if (schedule == null)
		{
			return NotFound($"No schedule exists for the provided ID: {scheduleID}.");
		}

		return schedule;
	}

	[HttpPost("add")]
	public async Task<IActionResult> Add([FromBody] Schedule schedule)
	{
		if (schedule == null)
		{
			return BadRequest("Provided schedule data is null.");
		}

		try
		{
			var addedSchedule = await _dataWorker.Schedules.AddAsync(schedule);
			return Ok(addedSchedule.Id);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error when adding schedule.");
			return Conflict("There was a conflict when adding the schedule.");
		}
	}

	[HttpPut("{scheduleID:int}")]
	public async Task<IActionResult> Update(int scheduleID, [FromBody] Schedule schedule)
	{
		if (schedule == null)
		{
			return BadRequest("Provided schedule data is null.");
		}

		if (scheduleID != schedule.Id)
		{
			return BadRequest("The scheduleID in the URL must match the scheduleID in the provided data.");
		}

		try
		{
			await _dataWorker.Schedules.UpdateAsync(schedule);
			return NoContent(); // HTTP 204 - success, but no content to return
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error when updating schedule.");
			return StatusCode(500, "An error occurred while updating the schedule.");
		}
	}

	[HttpDelete("{scheduleID:int}")]
	public async Task<IActionResult> Delete(int scheduleID)
	{
		try
		{
			var scheduleToDelete = await _dataWorker.Schedules.GetByIdAsync(scheduleID);
			if (scheduleToDelete == null)
			{
				return NotFound($"No schedule exists for the provided ID: {scheduleID}.");
			}

			await _dataWorker.Schedules.DeleteAsync(scheduleToDelete);
			return NoContent(); // HTTP 204 - success, but no content to return
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error when deleting schedule.");
			return StatusCode(500, "An error occurred while deleting the schedule.");
		}
	}
}