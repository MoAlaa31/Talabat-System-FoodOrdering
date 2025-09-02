using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Talabat.API.Dtos;
using Talabat.API.Errors;
using Talabat.API.Filters;
using Talabat.Core;
using Talabat.Core.Entities.Calender;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.WorkScheduleSpecs;
using Talabat.Repository;

namespace Talabat.API.Controllers
{
    public class ScheduleController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IUnitOfWork unitOfWork, IMapper mapper, IScheduleService scheduleService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _scheduleService = scheduleService;
        }

        [HttpPost("AddSchedule")]
        public async Task<ActionResult> AddSchedule(WorkScheduleFromUserDto workScheduleFromUser)
        {
            //Get current Doctor
            // 1️⃣ Validate Doctor Exists
            var doctor = await _unitOfWork.Repository<Doctor>().GetAsync(workScheduleFromUser.doctorId);
            if (doctor == null)
                return NotFound(new ApiResponse(404, "Doctor not found"));

            // Check if the new schedule overlaps with an existing one
            bool isOverlapping = await _scheduleService
                .IsScheduleOverlappingAsync(workScheduleFromUser.doctorId, workScheduleFromUser.Day,
                                            workScheduleFromUser.StartTime, workScheduleFromUser.EndTime);

            if (isOverlapping)
                return BadRequest(new ApiResponse(400, "Schedule conflicts with an existing schedule."));

            var workSchedule = _mapper.Map<WorkScheduleFromUserDto, WorkSchedule>(workScheduleFromUser);

            try
            {
                await _unitOfWork.Repository<WorkSchedule>().AddAsync(workSchedule);
                var result = await _unitOfWork.CompleteAsync();
                if (result <= 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Failed to add WorkSchedule"));
                }
                return Ok(workScheduleFromUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
        }


        //[ServiceFilter(typeof(ExistingIdFilter<WorkSchedule>))]
        [HttpGet("GetAllDoctorSchedules/{id:int}")]
        public async Task<ActionResult<IReadOnlyList<WorkSchedule>>> GetAllDoctorSchedules(int id)
        {
            // Validate Doctor Exists
            var doctor = await _unitOfWork.Repository<Doctor>().GetAsync(id);
            if (doctor == null)
                return NotFound(new ApiResponse(404, "Doctor not found"));

            // Get Work Schedules for the Doctor
            var wsSpec = new WorkSheduleSpecifications(id);
            var schedules = await _unitOfWork.Repository<WorkSchedule>().GetAllWithSpecAsync(wsSpec);
            if (schedules.IsNullOrEmpty())
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(_mapper.Map<IReadOnlyList<WorkSchedule>, IReadOnlyList<WorkScheduleFromDatabaseDto>>(schedules));
        }


        //[ServiceFilter(typeof(ExistingIdFilter<WorkSchedule>))]
        [HttpDelete("DeleteSchedule/{id:int}")]
        public async Task<ActionResult> DeleteSchedule(int id)
        {
            // Check if the schedule exists
            var schedule = await _unitOfWork.Repository<WorkSchedule>().GetAsync(id);
            if (schedule == null)
                return NotFound(new ApiResponse(404, "Schedule not found"));

            try
            {
                // Delete the schedule
                _unitOfWork.Repository<WorkSchedule>().Delete(schedule);
                var result = await _unitOfWork.CompleteAsync();

                if (result <= 0)
                    return BadRequest(new ApiResponse(400, "Failed to delete schedule"));

                return Ok(new ApiResponse(200, "Schedule deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }



    }
}
