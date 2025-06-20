using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Uniftec.PPW.Redesocial.FeedAPI.Aplicacao.DTO;
using Uniftec.PPW.Redesocial.FeedAPI.Aplicacao.Adapter;
using Uniftec.PPW.Redesocial.FeedAPI.Dominio.Repositorio;
using Uniftec.PPW.Redesocial.FeedAPI.Dominio.Entidades;

namespace Uiftec.PPW.Redesocial.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class FeedController : Controller
    {
       
       
            private readonly IFeedRepositorio _feedRepository;

            public FeedController(IFeedRepositorio feedRepository)
            {
                _feedRepository = feedRepository;
            }

            [HttpGet("{userId}")]
            public ActionResult<List<FeedItemDTO>> GetFeedByUserId(Guid userId)
            {
                var feedItems = _feedRepository.GetFeedByUserId(userId);
                var feedDTOs = new List<FeedItemDTO>();

                foreach (var item in feedItems)
                {
                    feedDTOs.Add(FeedItemAdapter.ToDTO(item));
                }

                return Ok(feedDTOs);
            }

            [HttpPost]
            public ActionResult AddFeedItem([FromBody] FeedItemDTO feedItemDTO)
            {
                var feedItem = FeedItemAdapter.ToEntity(feedItemDTO);
                _feedRepository.AddFeedItem(feedItem);
                return Ok("Feed item adicionado com sucesso.");
            }

            [HttpPut("{id}")]
            public ActionResult UpdateFeedItem(Guid id, [FromBody] FeedItemDTO feedItemDTO)
            {
                if (id != feedItemDTO.Id)
                    return BadRequest("ID do feedItem não corresponde ao da URL.");

                var feedItem = FeedItemAdapter.ToEntity(feedItemDTO);
                _feedRepository.UpdateFeedItem(feedItem);
                return Ok("Feed item atualizado com sucesso.");
            }

            [HttpDelete("{id}")]
            public ActionResult DeleteFeedItem(Guid id)
            {
                _feedRepository.RemoveFeedItem(id);
                return Ok("Feed item removido com sucesso.");
            }

            [HttpGet("geral")]
            public ActionResult<List<FeedItemDTO>> GetFeedGeral()
            {
                var feedItems = _feedRepository.GetAllFeedItems();
                var feedDTOs = new List<FeedItemDTO>();

                foreach (var item in feedItems)
                {
                    feedDTOs.Add(FeedItemAdapter.ToDTO(item));
                }

                return Ok(feedDTOs);
            }
        }

    }

