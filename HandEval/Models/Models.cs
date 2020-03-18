using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PokerTool.Models
{
    public enum Suit
    {
        Club,
        Diamond,
        Heart,
        Spade
    }
    
    public enum Rank
    {
        Two = 1,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }
    
    public class Card
    {
        public readonly Suit suit;
        public readonly Rank rank;
        
        private Card()
        {

        }
        
        public Card(Suit newSuit, Rank newRank)
        {
            suit = newSuit;
            rank = newRank;
        }
        
        public override string ToString()
        {
            return rank + " of " + suit + "s";
        }

        public string ToShortHand()
        {
            return ((int)this.rank < 9 ? ((int)this.rank + 1).ToString() : this.rank.ToString().Substring(0,1)) 
                    + this.suit.ToString().Substring(0,1).ToLower();
        }
    }

    public class Hand
    {
        public readonly IList<Card> cards;
        public Hand()
        {
            this.cards = new List<Card>();
        }

        public void AddCard(Card newCard)
        {
            if (cards.Count < 5)
                cards.Add(newCard);
        }

        public void AddCardByShorthand(string card)
        {
            var deck = new Deck();
            this.cards.Add(deck.cards.SingleOrDefault(x => x.ToShortHand() == card));
        }

        public override string ToString()
        {
            string handShortHand = "";
            foreach(Card card in cards.OrderBy(x => x.rank).ThenBy(x => x.suit))
                handShortHand += card.ToShortHand();
            return handShortHand;
        }

        private string OrderedSplit()
        {
            string partOne = "";
            string partTwo = "";

            foreach(Card card in cards.OrderBy(x => (int)x.rank).ThenBy(x => x.suit))
            {
                partOne += card.ToShortHand().Substring(0,1);
                partTwo += card.ToShortHand().Substring(1,1);
            }
            
            return partOne + "#" + partTwo;
        }

        public string Rank()
        {
            if (Regex.IsMatch(this.OrderedSplit(), @"(2345A|23456|34567|45678|56789|6789T|789TJ|89TJQ|9TJQK|TJQKA)#(.)\2{4}.*"))
                return "Straight flush";
            else if (Regex.IsMatch(this.OrderedSplit(), @"(.)\1{3}.*#.*"))
                return "Four of a kind";
            else if (Regex.IsMatch(this.OrderedSplit(), @"((.)\2{2}(.)\3{1}#.*|(.)\4{1}(.)\5{2}#.*)"))
                return "Full house";
            else if (Regex.IsMatch(this.OrderedSplit(), @".*#(.)\1{4}.*"))
                return "Flush";
            else if (Regex.IsMatch(this.OrderedSplit(), @"(2345A|23456|34567|45678|56789|6789T|789TJ|89TJQ|9TJQK|TJQKA)#.*"))
                return "Straight";
            else if (Regex.IsMatch(this.OrderedSplit(), @"(.)\1{2}.*#.*"))
                return "Three of a kind";
            else if (Regex.IsMatch(this.OrderedSplit(), @"(.)\1{1}.*(.)\2{1}.*#.*"))
                return "Two pair";
            else if (Regex.IsMatch(this.OrderedSplit(), @".*(\w)\1.*#.*"))
                return "One pair"; 
            else
                return "High Card";
        }
    }

    public class Deck
    {
        public Card[] cards;
        
        public Deck()
        {
            cards = new Card[52];
            for (int suitVal = 0; suitVal < 4; 
                 suitVal++)
            {
                for (int rankVal = 1; rankVal < 14; 
                     rankVal++)
                {
                    cards[suitVal * 13 + rankVal - 1] 
                    = new Card((Suit)suitVal, 
                    (Rank)rankVal);
                }
            }
        }

        public Card GetCard(int cardNum)
        {
            if (cardNum >= 0 && cardNum <= 51)
                return cards[cardNum];
            else
                throw 
                (new System.ArgumentOutOfRangeException
                ("cardNum", cardNum, 
                 "Value must be between 0 and 51."));
        }

        public void Shuffle()
        {
            Card[] newDeck = new Card[52];
            bool[] assigned = new bool[52];
            Random sourceGen = new Random();

            for (int i = 0; i < 52; i++)
            {
                int destCard = 0;
                bool foundCard = false;
                while (foundCard == false)
                {
                    destCard = sourceGen.Next(52);
                    if (assigned[destCard] == false)
                        foundCard = true;
                }
                assigned[destCard] = true;
                newDeck[destCard] = cards[i];
            }
            newDeck.CopyTo(cards, 0);
        }
    }
}