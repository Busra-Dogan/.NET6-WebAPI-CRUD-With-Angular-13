using Cards.API.Data;
using Cards.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly CardsDbContext _cardsDbContext;
        public CardsController(CardsDbContext cardsDbContext)
        {
            _cardsDbContext = cardsDbContext;
        }

        //Get All Cards
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await _cardsDbContext.Cards.ToListAsync();
            return Ok(cards);
        }

        //Get single card
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCard([FromRoute] Guid id)
        {
            var card = await _cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (card != null)
            {
                return Ok(card);
            }
            return NotFound("Card not found");
        }

        //Add card
        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] Card card)
        {
            card.Id = Guid.NewGuid();
            await _cardsDbContext.Cards.AddAsync(card);
            await _cardsDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCard), new { id = card.Id }, card);
        }

        //Updated a card
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCard([FromRoute] Guid id,[FromBody] Card card)
        {
            var existingCard = await _cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCard != null)
            {
                existingCard.CardHolderName = card.CardHolderName;
                existingCard.CardNumber = card.CardNumber;
                existingCard.ExpiryYear = card.ExpiryYear;
                existingCard.ExpiryMonth = card.ExpiryMonth;
                existingCard.CVC = card.CVC;
                await _cardsDbContext.SaveChangesAsync();
                return Ok(existingCard);
            }

            return NotFound("Card not found");
        }

        //Delete a card
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var existingCard = await _cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCard != null)
            {
                _cardsDbContext.Remove(existingCard);
                await _cardsDbContext.SaveChangesAsync();
                return Ok(existingCard);
            }

            return NotFound("Card not found");
        }
    }
}
