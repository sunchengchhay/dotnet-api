using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using dotnet_webapi.Dtos.Blog;

namespace dotnet_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IRepository<Blog> _blogRepository;
        public BlogController(IRepository<Blog> blogRepository)
        {
            _blogRepository = blogRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var blogs = await _blogRepository.GetAllAsync();
            return Ok(blogs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var blog = await _blogRepository.GetByIdAsnyc(id);
            if (blog == null)
            {
                return NotFound();
            }
            return Ok(blog);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostBlogDto blog)
        {
            var blogEntity = new Blog()
            {
                Name = blog.Name,
                Description = blog.Description
            };

            var createdBlogResponse = await _blogRepository.AddAsync(blogEntity);
            return CreatedAtAction(nameof(GetById), new { id = createdBlogResponse.Id }, createdBlogResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PostBlogDto blog)
        {
            var blogEntity = await _blogRepository.GetByIdAsnyc(id);
            if (blogEntity == null)
            {
                return NotFound();
            }

            blogEntity.Name = blog.Name;
            blogEntity.Description = blog.Description;

            await _blogRepository.UpdateAsync(blogEntity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _blogRepository.GetByIdAsnyc(id);
            if (blog == null)
            {
                return NotFound();
            }

            await _blogRepository.DeleteAsync(blog);
            return NoContent();
        }
    }
}