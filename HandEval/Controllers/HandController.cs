using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokerTool.Models;

namespace PokerTool.Controllers
{
    public class HandController : Controller
    {
        private readonly ILogger<HandController> _logger;

        public HandController(ILogger<HandController> logger)
        {
            _logger = logger;
        }

        public IActionResult Eval()
        {
            var evalViewModel = new EvalViewModel();
            var orderedDeck = new Deck();

            evalViewModel.deck = orderedDeck;

            return View("Eval", evalViewModel);
        }

        [BindProperty]
        public string deck { get; set; }
        [BindProperty]
        public string card1 { get; set; }
        [BindProperty]
        public string card2 { get; set; }
        [BindProperty]
        public string card3 { get; set; }
        [BindProperty]
        public string card4 { get; set; }
        [BindProperty]
        public string card5 { get; set; }
        public IActionResult SubmitHand()
        {
            var hand = new Hand();
            
            hand.AddCardByShorthand(card1);
            hand.AddCardByShorthand(card2);
            hand.AddCardByShorthand(card3);
            hand.AddCardByShorthand(card4);
            hand.AddCardByShorthand(card5);

            return View("Hand", hand);
        }

        public IActionResult Show()
        {
            var deck = new Deck();
            deck.Shuffle();

            var hand = new Hand();
            for(int i = 1; i < 6; i++)
                hand.AddCard(deck.GetCard(i));

            return View("Hand", hand);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
