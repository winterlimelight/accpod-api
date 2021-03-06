﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Commands;
using Business.CommandHandlers;
using Business.Queries;
using Business.Models;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        /// <summary>
        /// Get all courses
        /// </summary>
        /// <returns>A list of all courses</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> Get([FromServices] ICourseQueries courseQueries)
        {
            var courses = await courseQueries.GetAll();
            return Ok(courses);
        }

        /// <summary>
        /// Get a single course by Id
        /// </summary>
        /// <returns>The course with the given Id or 404 if not found</returns>
        [Route("[controller]/{id}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> Get(Guid id)
        {
            // Although this method wasn't requested, it must exist for Action() in CreateAsync to succeed.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create a new course
        /// </summary>
        /// <param name="request">Information about the course</param>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] AddCourseRequest request, [FromServices] ICommandHandler<AddCourseRequest> handler)
        {
            Guid? id = await handler.Handle(request);
            return CreatedAtRoute(new { id }, id);
        }
    }
}
